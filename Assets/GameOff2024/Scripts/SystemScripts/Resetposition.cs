using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    [SerializeField] private PlayerKCC _player;
    [SerializeField] private GameObject _gameoverUI;
    private Vector3 lastSavedPosition; // The last valid position saved
    private float fallThreshold = -10f; // Y-value threshold to detect falling
    private float saveInterval = 3f; // Time interval (seconds) to save position
    private float timeSinceLastSave = 0f; // Tracks time since last save
    private int maxlist = 3;

    private List<Vector3> _lastpos = new List<Vector3>();


    //List 1 ,2 ,3 ,4 ,5

    void Start()
    {
        // Initialize the last saved position to the object's starting position
        _player = GameObject.Find("Player Controller").GetComponent<PlayerKCC>();
        _gameoverUI = GameObject.Find("GameOverPanel");
        if( _player == null ) { Debug.LogWarning("Player not Found"); }
        transform.position = _player.transform.position;
    }

    void Update()
    {
        transform.position = _player.transform.position;
        Debug.Log("Position: " + transform.position.y);
        // Track time and save position periodically
        timeSinceLastSave += Time.deltaTime;
        if (timeSinceLastSave >= saveInterval)
        {
            SavePosition();
            timeSinceLastSave = 0f; // Reset timer
        }

        // Check if the object is falling below the threshold
        if (_player.transform.position.y < fallThreshold)
        {
            //Time.timeScale = 0f;
            //_gameoverUI.SetActive(true);
            ResetToLastSavedPosition();
        }
    }

    /// <summary>
    /// Saves the object's current position.
    /// </summary>
    private void SavePosition()
    {
        _lastpos.Add(_player.transform.position);

        if(_lastpos.Count > maxlist)
        {
            _lastpos.RemoveAt(0);
        }


        Debug.Log("Position Saved. History Count: " + _lastpos.Count);
    }

    /// <summary>
    /// Resets the object to the last saved position.
    /// </summary>
    private void ResetToLastSavedPosition()
    {
        //_player.transform.position = lastSavedPosition;
        //_player.Motor.SetPosition(lastSavedPosition);
        //Debug.LogWarning("Object reset to last saved position: " + lastSavedPosition);
        if (_lastpos.Count < 2)
        {
            Debug.LogWarning("Not enough saved positions");
        }
        Vector3 resetpos = _lastpos[_lastpos.Count - 1];
        _player.Motor.SetPosition(resetpos);
        Debug.LogWarning("Player reset to second-to-last saved position: " + resetpos);
    }
}


