using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public int Id;
        public string Type;
        public List<int> ConnectedRooms;
        public bool IsVisited;

        public Room(int id, string type)
        {
            Id = id;
            Type = type;
            ConnectedRooms = new List<int>();
            IsVisited = false;
        }

        public void ConnectTo(Room otherRoom)
        {
            if (!ConnectedRooms.Contains(otherRoom.Id))
            {
                ConnectedRooms.Add(otherRoom.Id);
                otherRoom.ConnectedRooms.Add(Id); // Ensure bidirectional connection
            }
        }
    }

    public int roomCount = 10;
    public GameObject startRoomPrefab;
    public GameObject bossRoomPrefab;
    public List<GameObject> uniqueRoomPrefabs;
    public GameObject defaultRoomPrefab;

    private List<Room> dungeon = new List<Room>();
    private System.Random random = new System.Random();

    void Start()
    {
        GenerateDungeon(roomCount);
        RenderDungeon();
    }

    private void GenerateDungeon(int roomCount)
    {

        //Ensure correct number of rooms
        if(roomCount < 2 || uniqueRoomPrefabs.Count < roomCount - 2)
        {
            Debug.LogError("Not Enough Unique room prefabs or invalid room count");
            return;
        }

        List<GameObject> shuffledUniqueRooms = new List<GameObject>(uniqueRoomPrefabs);
        ShuffleList(shuffledUniqueRooms);

        //Create Start room
        dungeon.Add(new Room(0, "Start"));

        //Creat unique rooms
        for(int i = 1; i <=roomCount -2; i++)
        {
            dungeon.Add(new Room(i, "Unique"));
        }

        // Assign unique room prefabs randomly (using shuffledUniqueRooms)
        for (int i = 1; i <= roomCount - 2; i++)
        {
            dungeon[i].Type = shuffledUniqueRooms.Count > 0 ? shuffledUniqueRooms[0].name : "Default";
            shuffledUniqueRooms.RemoveAt(0);
        }

        // Create boss room
        dungeon.Add(new Room(roomCount - 1, "Boss"));

        // Connect rooms randomly
        for (int i = 0; i < roomCount; i++)
        {
            // Skip connecting the boss room until the end
            if (i == roomCount - 1)
                continue;

            int connections = UnityEngine.Random.Range(1, 3); // Each room has 1-2 connections
            for (int j = 0; j < connections; j++)
            {
                int targetRoomId;

                // Ensure the start room doesn't directly connect to the boss room
                do
                {
                    targetRoomId = UnityEngine.Random.Range(0, roomCount - 1); // Exclude the boss room
                } while (targetRoomId == i); // Avoid self-connections

                dungeon[i].ConnectTo(dungeon[targetRoomId]);
            }
        }

        // Connect the boss room to the final room
        int lastRoomBeforeBoss = UnityEngine.Random.Range(1, roomCount - 1); // Ensure it’s not the start room
        dungeon[roomCount - 1].ConnectTo(dungeon[lastRoomBeforeBoss]);

    }

    private void RenderDungeon()
    {
        float roomSpacing = 20f; // Minimum spacing between rooms
        float floorHeight = 0f; // All rooms are at this height
        Dictionary<int, Vector3> roomPositions = new Dictionary<int, Vector3>(); // Track room positions
        HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); // Track used positions

        // First pass: Place all rooms and store their positions
        for (int i = 0; i < dungeon.Count; i++)
        {
            Vector3 position;

            // Ensure no overlap by generating positions until one is free
            do
            {
                position = new Vector3(
                    UnityEngine.Random.Range(-5, 5) * roomSpacing,
                    floorHeight,
                    UnityEngine.Random.Range(-5, 5) * roomSpacing
                );
            } while (occupiedPositions.Contains(position));

            occupiedPositions.Add(position); // Mark this position as occupied
            roomPositions[i] = position; // Save position for this room

            // Instantiate the correct prefab
            GameObject roomPrefab = dungeon[i].Type switch
            {
                "Start" => startRoomPrefab,
                "Boss" => bossRoomPrefab,
                _ => uniqueRoomPrefabs.Count > 0 ? uniqueRoomPrefabs[0] : defaultRoomPrefab,
            };

            if (dungeon[i].Type != "Start" && dungeon[i].Type != "Boss")
            {
                uniqueRoomPrefabs.RemoveAt(0); // Use and remove prefab
            }

            GameObject roomObject = Instantiate(roomPrefab, position, Quaternion.identity);
            roomObject.name = $"Room {dungeon[i].Id} ({dungeon[i].Type})";

            // Initialize the room's behavior
            RoomComponent roomComponent = roomObject.GetComponent<RoomComponent>();
            if (roomComponent != null)
            {
                roomComponent.Initialize(dungeon[i], OnRoomVisited);
            }
        }

        // Second pass: Draw connections using Debug.DrawLine
        foreach (Room room in dungeon)
        {
            Vector3 roomPosition = roomPositions[room.Id];
            foreach (int connectedRoomId in room.ConnectedRooms)
            {
                if (roomPositions.ContainsKey(connectedRoomId))
                {
                    Vector3 connectedRoomPosition = roomPositions[connectedRoomId];
                    Debug.DrawLine(roomPosition, connectedRoomPosition, Color.red, 100f); // Red line visible for 10 seconds
                }
                else
                {
                    Debug.LogWarning($"Room {room.Id} references a non-existent room {connectedRoomId}");
                }
            }
        }
    }


    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    private void OnRoomVisited(Room room)
    {
        if (room.IsVisited)
        {
            Debug.Log($"Room {room.Id} already visited.");
            return;
        }

        room.IsVisited = true;
        Debug.Log($"Visited Room {room.Id}: Gained a buff!");

        // Example of a buff effect
        ApplyBuff();
    }

    private void ApplyBuff()
    {
        Debug.Log("Buff applied! (e.g., +10 health, +5 damage, etc.)");
        // Implement actual buff logic here
    }
}
