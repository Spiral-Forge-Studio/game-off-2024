using KinematicCharacterController;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform targetRoom;  // Target room to teleport the player to

    // For debugging purposes, add targetRoomId
    public int targetRoomId;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        // Check if the player (or another object) enters the trigger collider
        if (other.CompareTag("Player") && targetRoom != null)
        {
            Debug.Log("Teleporting");
            TeleportPlayer(other.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        // Teleport the player to the target room's position
        if (targetRoom != null)
        {
            //player.transform.position = targetRoom.position;
            PlayerKCC playerKCC = player.GetComponent<PlayerKCC>();
            playerKCC.Motor.SetPosition(targetRoom.position);
            Debug.Log($"Teleported player to Room {targetRoomId} at position {targetRoom.position}");
        }
        else
        {
            Debug.LogWarning("Target room is not set for this teleporter!");
        }
    }
}
