using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    private GameObject _player;
    private Vector3 lastSavedPosition; // The last valid position saved
    private float fallThreshold = -10f; // Y-value threshold to detect falling
    private float saveInterval = 3f; // Time interval (seconds) to save position
    private float timeSinceLastSave = 0f; // Tracks time since last save

    void Start()
    {
        // Initialize the last saved position to the object's starting position
        _player = GameObject.Find("Player Controller");
        if( _player == null ) { Debug.LogWarning("Player not Found"); }
        lastSavedPosition = _player.transform.position;
    }

    void Update()
    {
        // Track time and save position periodically
        timeSinceLastSave += Time.deltaTime;
        if (timeSinceLastSave >= saveInterval)
        {
            SavePosition();
            timeSinceLastSave = 0f; // Reset timer
        }

        // Check if the object is falling below the threshold
        if (transform.position.y < fallThreshold)
        {
            ResetToLastSavedPosition();
        }
    }

    /// <summary>
    /// Saves the object's current position.
    /// </summary>
    private void SavePosition()
    {
        lastSavedPosition = _player.transform.position;
        Debug.Log("Position Saved: " + lastSavedPosition);
    }

    /// <summary>
    /// Resets the object to the last saved position.
    /// </summary>
    private void ResetToLastSavedPosition()
    {
        _player.transform.position = lastSavedPosition;
        Debug.LogWarning("Object reset to last saved position: " + lastSavedPosition);
    }
}


