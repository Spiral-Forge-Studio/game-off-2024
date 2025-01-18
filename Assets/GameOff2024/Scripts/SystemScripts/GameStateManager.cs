using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameStateManager : MonoBehaviour
{
    [Header("Mob Spawners")]
    public MobSpawnerScript[] mobSpawners;
    public int currentMobSpawnerIndex;

    public PlayerKCC playerKCC;
    private PlayerScript playerScript;
    private PlatformScript platformScript;

    public int FPSCap;
    public float _timeScale;

    public bool isPaused;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public float restartDelay = 2f;
    public BuffMenu BuffMenu;

    [Header("Game Completed UI")]
    public GameObject congratsPanel;
    private PauseScript _pauseScript;

    void Awake()
    {
        Application.targetFrameRate = FPSCap;
        //gameOverPanel.SetActive(false);

        BuffMenu.FirstTimeRun = true;

        playerScript = FindObjectOfType<PlayerScript>();
        platformScript = FindObjectOfType<PlatformScript>();

        if (BuffMenu != null)
        {
            if (BuffMenu.FirstTimeRun)
            {
                BuffMenu.InitializeMenu();//run this only once per game 
                BuffMenu.FirstTimeRun = false;
            }
        }

        currentMobSpawnerIndex = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        _pauseScript = FindObjectOfType<PauseScript>();
        isPaused = false;

        StartWave();

        playerScript._disableRotation = false;
    }

    

    //private void Update()
    //{
    //    if (isPaused)
    //    {
    //        Time.timeScale = 0f;
    //    }
    //    else
    //    {
    //        Time.timeScale = 1.0f;
    //    }
    //}

    public void MobDied()
    {
        mobSpawners[currentMobSpawnerIndex].ReduceMobCount();
    }

    public void StartWave()
    {
        if (mobSpawners[currentMobSpawnerIndex].AllWavesCompleted)
        {
            platformScript.triggerCollider.SetActive(true);
            Debug.Log("[GAMESTATE] nextLevel");
            currentMobSpawnerIndex++;
        }
        else
        {
            mobSpawners[currentMobSpawnerIndex].SpawnWave();
        }
    }

    public void GameComplete()
    {
        congratsPanel.SetActive(true);
        _pauseScript.Pause();
    }

    public void GameOver()
    {
        //isPaused = true;
        gameOverPanel.SetActive(true);
        //Time.timeScale = 0f;
        // Start a coroutine to reload the game after a delay

        playerScript._disableRotation = true;

        Cursor.visible = true;

        _pauseScript.Pause();
        
        //StartCoroutine(ReloadGameAfterDelay());
    }

    private IEnumerator ReloadGameAfterDelay()
    {
        // Wait for the specified delay (restartDelay)
        yield return new WaitForSecondsRealtime(restartDelay);

        playerScript._disableMovement = false;
        playerScript._disableRotation = false;

        // After the delay, reload the game
        ReloadGame();
    }

    public void ReloadGame()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
