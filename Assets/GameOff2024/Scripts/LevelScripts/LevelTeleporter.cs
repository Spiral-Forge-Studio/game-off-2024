using KinematicCharacterController;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform targetRoom; // The next room's transform
    private bool isActive = true;


    public void TeleportPlayer()
    {
        if (isActive && targetRoom != null)
        {
            Debug.Log($"[Teleporter] Teleporting player to {targetRoom.name}");

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerKCC playerKCC = player.GetComponent<PlayerKCC>();
                if (playerKCC != null)
                {
                    playerKCC.Motor.SetPosition(targetRoom.position);
                    isActive = false;
                }
            }
        }
        else
        {
            Debug.LogWarning("[Teleporter] Teleporter is not active or targetRoom is not set.");
        }
    }
}
