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

    public GameObject triggerCollider;

    private int nextLevel;
    private Vector3 newWallPosition;

    private bool raisingPlatformWallls;
    private bool liftingPlatform;
    private bool loweringPlatformWalls;
    
    private GameStateManager gameStateManager;

    private void Awake()
    {
        gameStateManager = FindObjectOfType<GameStateManager>();
    }

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

                triggerCollider.SetActive(false);
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

                gameStateManager.StartWave();
            }

            transform.position = nextPosition;
        }
        else if (loweringPlatformWalls)
        {
            Vector3 nextPosition = Vector3.MoveTowards(platformWalls.transform.position, newWallPosition, wallRaiseSpeed * Time.deltaTime);

            if (nextPosition == newWallPosition)
            {
                loweringPlatformWalls = false;
            }

            platformWalls.transform.position = nextPosition;
        }
    }

    public void StartLiftSequence()
    {
        EnableWalls();
    }

    private void EnableWalls()
    {
        platformWalls.SetActive(true);
        newWallPosition = platformWalls.transform.position + new Vector3(0, platformWallsOffsetY, 0);
        raisingPlatformWallls = true;
    }
}
