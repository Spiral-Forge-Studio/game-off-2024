using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [SerializeField] private List<Transform> levels;
    [SerializeField] private GameObject platformWalls;
    [SerializeField] private float platformWallsOffsetY;
    [SerializeField] private float platformSpeed;
    [SerializeField] private float wallRaiseSpeed;

    public Collider triggerCollider;

    private int nextLevel;
    private Vector3 newWallPosition;

    private bool raisingPlatformWallls;
    private bool liftingPlatform;
    private bool loweringPlatformWalls;


    // Start is called before the first frame update
    void Start()
    {
        nextLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (raisingPlatformWallls)
        {
            Vector3 nextPosition = Vector3.MoveTowards(platformWalls.transform.position, newWallPosition, wallRaiseSpeed * Time.deltaTime);

            if (nextPosition == newWallPosition)
            {
                raisingPlatformWallls = false;
                liftingPlatform = true;
            }

            platformWalls.transform.position = nextPosition;
        }
        else if (liftingPlatform)
        {
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, levels[nextLevel].position, platformSpeed * Time.deltaTime);

            if (nextPosition == levels[nextLevel].position)
            {
                nextLevel++;
                liftingPlatform = false;
                loweringPlatformWalls = true;
                newWallPosition = platformWalls.transform.position - new Vector3(0, platformWallsOffsetY, 0);
            }

            transform.position = nextPosition;
        }
        else if (loweringPlatformWalls)
        {
            Vector3 nextPosition = Vector3.MoveTowards(platformWalls.transform.position, newWallPosition, wallRaiseSpeed * Time.deltaTime);

            if (nextPosition == newWallPosition)
            {
                loweringPlatformWalls = false;
                triggerCollider.enabled = true;
            }

            platformWalls.transform.position = nextPosition;
        }
    }

    public void StartLiftSequence()
    {
        EnableWalls();
        triggerCollider.enabled = false;
    }

    private void EnableWalls()
    {
        platformWalls.SetActive(true);
        newWallPosition = platformWalls.transform.position + new Vector3(0, platformWallsOffsetY, 0);
        raisingPlatformWallls = true;
    }
}
