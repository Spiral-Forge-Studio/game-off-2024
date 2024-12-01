using KinematicCharacterController;
using UnityEngine;

public class TutorialExit : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    private DungeonGenerator dungeonGenerator; // Reference to the Room Generator

    private void Awake()
    {
        dungeonGenerator = FindObjectOfType<DungeonGenerator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Entered Collider. Triggered by: {other.gameObject.name}, Player: {player.name}");

        PlayerKCC playerKCC = player.GetComponent<PlayerKCC>();
        // Check if the player enters the trigger zone
        if (playerKCC != null)
        {
            Debug.Log("I have entered the first if statement");
            if (dungeonGenerator != null)
            {
                dungeonGenerator.TeleportPlayerToNextRoom(); // Delegate teleport logic
                Debug.Log("Triggered teleportation to the next room from the tutorial.");
            }
            else
            {
                Debug.LogWarning("DungeonGenerator reference is missing in TutorialExit!");
            }
        }
    }
}
