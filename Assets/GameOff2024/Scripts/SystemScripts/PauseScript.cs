using UnityEngine;
using MoreMountains.Feedbacks;

public class PauseScript : MonoBehaviour
{
    public void Pause()
    {
        // Trigger a time scale event to set the time scale to 0, effectively pausing the game
        MMTimeScaleEvent.Trigger(
            MMTimeScaleMethods.For, // Method to modify the time scale
            0f,                    // Time scale value (0 pauses the game)
            0f,                    // Duration (0 means infinite until manually reset)
            false,                 // No interpolation
            0f,                    // Lerp speed (not used in this case)
            true                   // Infinite duration
        );
    }

    public void Resume()
    {
        // Reset the time scale to the default value
        MMTimeScaleEvent.Trigger(
            MMTimeScaleMethods.Reset,
            1f, // This value is ignored when using Reset
            0f, // This value is ignored when using Reset
            false,
            0f,
            true
        );
    }
}
