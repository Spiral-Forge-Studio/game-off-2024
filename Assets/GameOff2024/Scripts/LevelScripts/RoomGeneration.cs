using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public int Id;
        public string Type;
        public List<int> ConnectedRooms = new List<int>();
        public Transform PlayerSpawnPoint; // Add a spawn point for teleporting the player

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
                otherRoom.ConnectedRooms.Add(Id); // Ensure the other room knows about this connection
                Debug.Log($"Room {Id} connected to Room {otherRoom.Id}");
            }
        }


    }
    public GameObject player; // Reference to the player GameObject

    public GameObject teleporterPrefab; // Assign this in the Unity Editor

    public int roomCount = 10;
    public GameObject startRoomPrefab;
    public GameObject bossRoomPrefab;
    public List<GameObject> uniqueRoomPrefabs;
    public GameObject defaultRoomPrefab;

    private List<Room> dungeon = new List<Room>();
    private System.Random random = new System.Random();

    private Dictionary<int, GameObject> roomObjects = new Dictionary<int, GameObject>();
    private Room currentRoom;

    public Room startRoom; // The designated start room

    void Start()
    {
        GenerateDungeon(roomCount);
        RenderDungeon();
    }

    private void GenerateDungeon(int roomCount)
    {
        dungeon.Clear();

        // Create Start Room
        dungeon.Add(new Room(0, "Start"));

        // Create Unique Rooms
        for (int i = 1; i <= roomCount - 2; i++)
        {
            dungeon.Add(new Room(i, "Unique"));
        }

        // Create Boss Room
        dungeon.Add(new Room(roomCount - 1, "Boss"));

        for (int i = 0; i < roomCount - 1; i++) // Exclude the last room
        {
            // Connect the current room to the next room
            dungeon[i].ConnectTo(dungeon[i + 1]);
        }

        // Connect the last regular room to the boss room
        dungeon[roomCount - 2].ConnectTo(dungeon[roomCount - 1]);

        foreach (var roomObject in roomObjects.Values)
        {
            Room room = dungeon[roomObject.GetComponent<RoomComponent>().RoomId];

            // Example: Assume each room prefab has a child object named "PlayerSpawn"
            Transform spawnPoint = roomObject.transform.Find("PlayerSpawn");

            if (spawnPoint == null)
            {
                spawnPoint = roomObject.GetComponentInChildren<Transform>(true)?.Find("PlayerSpawn");
            }
            if (spawnPoint != null)
            {
                room.PlayerSpawnPoint = spawnPoint;
            }
            else
            {
                Debug.LogWarning($"Spawn point not found for Room {room.Id}!");
            }
        }

    }





    private void RenderDungeon()
    {
        float roomSpacing = 100f; // Minimum spacing between rooms
        Dictionary<int, Vector3> roomPositions = new Dictionary<int, Vector3>(); // Track room positions
        Dictionary<int, GameObject> roomObjects = new Dictionary<int, GameObject>(); // Track room GameObjects

        // Place all rooms in a grid layout and store their positions
        for (int i = 0; i < dungeon.Count; i++)
        {
            // Grid layout calculation
            Vector3 position = new Vector3(
                (i % 5) * roomSpacing, // Column index
                0f,                    // Keep all rooms at the same height
                (i / 5) * roomSpacing  // Row index
            );

            roomPositions[i] = position; // Save position for this room

            if (dungeon[i].Type == "Start")
            {
                startRoom = dungeon[i]; // Assign the start room
                startRoom.Id = 0;       // Explicitly set ID to 0
                startRoom.Type = "Start";

                // Connect the start room to the first actual room
                if (dungeon.Count > 1)
                {
                    startRoom.ConnectedRooms.Add(1); // Add connection to Room ID 1
                }
            }


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

            // Add room GameObject to the dictionary
            roomObjects[dungeon[i].Id] = roomObject;

            // Initialize the room's behavior
            RoomComponent roomComponent = roomObject.GetComponent<RoomComponent>();
            if (roomComponent != null)
            {
                roomComponent.Initialize(dungeon[i], OnRoomVisited);
            }

            // Assign the player spawn point for this room
            Transform spawnPoint = roomObject.transform.Find("PlayerSpawn");
            if (spawnPoint != null)
            {
                dungeon[i].PlayerSpawnPoint = spawnPoint; // Assign to the Room's PlayerSpawnPoint
            }
            else
            {
                Debug.LogWarning($"Player spawn point not found for Room {dungeon[i].Id}!");
            }
        }



        // Draw connections and add teleporters
        foreach (Room room in dungeon)
        {
            Vector3 roomPosition = roomPositions[room.Id];
            if (roomObjects.TryGetValue(room.Id, out GameObject currentRoomObject))
            {
                foreach (int connectedRoomId in room.ConnectedRooms)
                {
                    // Draw debug lines for connections
                    if (roomPositions.ContainsKey(connectedRoomId))
                    {
                        Vector3 connectedRoomPosition = roomPositions[connectedRoomId];
                        Debug.DrawLine(roomPosition, connectedRoomPosition, Color.red, 100f); // Visible for 100 seconds
                    }
                    else
                    {
                        Debug.LogWarning($"Room {room.Id} references a non-existent room {connectedRoomId}");
                    }

                }
            }
        }



        // Initialize BuffSelectionUI
        BuffSelectionUI buffUI = FindObjectOfType<BuffSelectionUI>();
        if (buffUI != null)
        {
            PlayerKCC playerKCC = player.GetComponent<PlayerKCC>();
            if (playerKCC != null)
            {
                buffUI.Initialize(playerKCC, currentRoom, roomObjects);
            }
            else
            {
                Debug.LogWarning("Player GameObject does not have a PlayerKCC component!");
            }
        }
        else
        {
            Debug.LogWarning("BuffSelectionUI could not be found in the scene!");
        }
    }


    public Room GetRoomById(int roomId)
    {
        return dungeon.FirstOrDefault(room => room.Id == roomId);
    }
    // Update currentRoom as the player teleports
    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }

    private List<Room> ShuffleRooms(List<Room> rooms)
    {
        Debug.Log("Room Order After Shuffling:");
        // Create rooms
        for (int i = 0; i < roomCount; i++)
        {
            dungeon.Add(new Room(i, $"Room {i}"));
            Debug.Log($"Room {i}: {dungeon[i]}");
        }

        // Shuffle rooms (excluding the start and boss rooms)
        List<Room> shuffledRooms = ShuffleRooms(dungeon.GetRange(1, roomCount - 2)); // Exclude start (0) and boss room (last)
        shuffledRooms.Insert(0, dungeon[0]); // Add the start room back at the beginning
        shuffledRooms.Add(dungeon[roomCount - 1]); // Add the boss room at the end

        dungeon = shuffledRooms;

        return rooms.OrderBy(x => UnityEngine.Random.value).ToList();

       
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

    public void TeleportPlayerToNextRoom()
    {
        Debug.Log("Teleport method entered!");
        if (currentRoom != null && currentRoom.ConnectedRooms.Count > 0)
        {
            Debug.Log($"Current Room: {currentRoom.Id}, Connected Rooms Count: {currentRoom.ConnectedRooms.Count}");

            int nextRoomId = currentRoom.ConnectedRooms[0];
            Debug.Log($"Attempting to teleport to Room {nextRoomId}");
            Debug.Log($"Next Room ID: {nextRoomId}");
            if (roomObjects.ContainsKey(nextRoomId))
            {
                GameObject nextRoomObject = roomObjects[nextRoomId];
                Transform spawnPoint = nextRoomObject.transform.Find("PlayerSpawn");

                if (spawnPoint != null)
                {
                    Debug.Log($"PlayerSpawn found at position {spawnPoint.position}");
                    PlayerKCC playerKCC = player.GetComponent<PlayerKCC>();
                    if (player != null)
                    {
                        playerKCC.Motor.SetPosition(spawnPoint.position);
                        Debug.Log($"Player teleported to Room {nextRoomId} at position {spawnPoint.position}");

                        currentRoom = dungeon[nextRoomId];
                    }
                    else
                    {
                        Debug.LogError("Player reference is null in TeleportPlayerToNextRoom!");
                    }
                }
                else
                {
                    Debug.LogError($"PlayerSpawn not found in Room {nextRoomId}");
                }
            }
            else
            {
                Debug.LogError($"Room {nextRoomId} does not exist in roomObjects!");
            }
        }
        else
        {
            Debug.LogWarning("No connected rooms to teleport to!");
        }
    }





}
