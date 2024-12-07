using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    private Transform player;
    [SerializeField] private GameObject level;
    [SerializeField] private float spawnTriggerDistance;
    [SerializeField] private float despawnTriggerDistance;

    private bool spawnedRoom = false;

    private void Start()
    {
        spawnedRoom = false;
        player = FindObjectOfType<PlayerKCC>().transform;
        level.SetActive(false);
    }

    private void Update()
    {
        if (!spawnedRoom)
        {
            if (Mathf.Abs(player.position.y - transform.position.y) < spawnTriggerDistance)
            {
                level.SetActive(true);
                spawnedRoom = true;
            }
        }

        if (spawnedRoom)
        {
            if (Mathf.Abs(player.position.y - transform.position.y) > despawnTriggerDistance)
            {
                level.SetActive(false);
                spawnedRoom = false;
            }
        }
    }
}
