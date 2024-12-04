using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayMusic(AudioManager.instance.menuMusicSource, 0);
    }

    public void PlayGame()
    {
        AudioManager.instance.menuMusicSource.Stop();
        SceneManager.LoadScene(1); 
    }

    public void Options()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
