using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameStateManager : MonoBehaviour
{
    public PlayerKCC playerKCC;
    private PlayerScript playerScript;

    public int FPSCap;
    public float _timeScale;

    public bool isPaused;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public float restartDelay = 2f;
    public BuffMenu BuffMenu;


    void Awake()
    {
        Application.targetFrameRate = FPSCap;
        //gameOverPanel.SetActive(false);

        playerScript = FindObjectOfType<PlayerScript>();
        if (BuffMenu != null)
        {
            if (BuffMenu.FirstTimeRun)
            {
                BuffMenu.InitializeMenu();//run this only once per game 
                BuffMenu.FirstTimeRun = false;
            }
            

        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }


    private void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void GameOver()
    {
        //isPaused = true;
        gameOverPanel.SetActive(true);
        //Time.timeScale = 0f;
        // Start a coroutine to reload the game after a delay

        playerScript._disableMovement = true;
        playerScript._disableRotation = true;
        
        StartCoroutine(ReloadGameAfterDelay());
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
