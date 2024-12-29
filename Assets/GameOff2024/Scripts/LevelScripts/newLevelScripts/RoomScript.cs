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
    private Transform allObstacles;

    private void Start()
    {
        spawnedRoom = false;
        player = FindObjectOfType<PlayerKCC>().transform;
        level.SetActive(false);

        // Find the "AllObstacles" GameObject within "level"
        allObstacles = level.transform.Find("AllObstacles");

        // Ensure all obstacles are initially deactivated
        if (allObstacles != null)
        {
            foreach (Transform obstacle in allObstacles)
            {
                obstacle.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (!spawnedRoom)
        {
            if (Mathf.Abs(player.position.y - transform.position.y) < spawnTriggerDistance)
            {
                level.SetActive(true);
                spawnedRoom = true;

                // Activate a random obstacle
                ActivateRandomObstacle();
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

    private void ActivateRandomObstacle()
    {
        if (allObstacles == null) return;

        // Deactivate all obstacles
        foreach (Transform obstacle in allObstacles)
        {
            obstacle.gameObject.SetActive(false);
        }

        // Get a random child from "AllObstacles"
        int randomIndex = Random.Range(0, allObstacles.childCount);
        Transform randomObstacle = allObstacles.GetChild(randomIndex);

        // Activate the randomly selected obstacle
        randomObstacle.gameObject.SetActive(true);
    }
}
