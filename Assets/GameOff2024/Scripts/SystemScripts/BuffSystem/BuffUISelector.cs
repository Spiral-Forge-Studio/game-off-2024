using KinematicCharacterController;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DungeonGenerator;

public class BuffSelectionUI : MonoBehaviour
{
    private GameStateManager gameStateManager;
    public GameObject buffUI; // Reference to the UI canvas or panel
    public PlayerKCC player; // Reference to the player
    private Room currentRoom; // Reference to the current room
    private Dictionary<int, GameObject> roomObjects; // Reference to all room objects

    public Button buffOption1Button;        // Button for the first buff
    public Button buffOption2Button;        // Button for the second buff

    private Action<int> onBuffSelectedCallback;

    private void Awake()
    {
        gameStateManager = FindObjectOfType<GameStateManager>();
    }

    public void DisplayBuffOptions(Action<int> callback)
    {
        // Assign the callback to be used when a buff is selected
        onBuffSelectedCallback = callback;

        // Ensure the UI is visible
        if (buffUI != null)
        {
            buffUI.SetActive(true);
        }

        // Hook up the buttons to call the callback with their respective buff indices
        buffOption1Button.onClick.RemoveAllListeners();
        buffOption1Button.onClick.AddListener(() => BuffSelected(0));

        buffOption2Button.onClick.RemoveAllListeners();
        buffOption2Button.onClick.AddListener(() => BuffSelected(1));
    }
    public void Initialize(PlayerKCC player, Room currentRoom, Dictionary<int, GameObject> roomObjects)
    {
        this.player = player;
        this.currentRoom = currentRoom;
        this.roomObjects = roomObjects;
    }

    public void ShowBuffOptions()
    {
        buffUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game while the buff UI is active
    }

    public void HideBuffOptions()
    {
        buffUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void OnBuffSelected(int buffId)
    {
        // Apply the chosen buff
        ApplyBuff(buffId);

        // Teleport to a connected room
        TeleportToConnectedRoom();

        // Hide the UI after the selection
        HideBuffOptions();
    }

    private void ApplyBuff(int buffId)
    {
        // Logic to apply buffs to the player
        Debug.Log($"Buff {buffId} selected and applied to the player.");
    }

    public void BuffSelected(int buffIndex)
    {
        // Call the callback with the selected index
        onBuffSelectedCallback?.Invoke(buffIndex);

        // Optionally, hide the UI after a selection is made
        if (buffUI != null)
        {
            buffUI.SetActive(false);
        }
    }

    private void TeleportToConnectedRoom()
    {
        if (currentRoom.ConnectedRooms.Count == 0)
        {
            Debug.LogWarning("No connected rooms to teleport to!");
            return;
        }

        // Choose the first connected room (you can modify this for more advanced logic)
        int targetRoomId = currentRoom.ConnectedRooms[0];

        if (roomObjects.TryGetValue(targetRoomId, out GameObject targetRoom))
        {
            Vector3 targetPosition = targetRoom.transform.position;
            player.Motor.SetPosition(targetPosition);

            Debug.Log($"Teleported to Room {targetRoomId} at position {targetPosition}");
        }
        else
        {
            Debug.LogWarning($"Target room {targetRoomId} not found in roomObjects dictionary.");
        }
    }

    // Helper method to get a Room by ID
    private Room GetRoomById(int roomId)
    {
        // Assuming your dungeon generation script has access to all room data
        return FindObjectOfType<DungeonGenerator>().GetRoomById(roomId);
    }
}