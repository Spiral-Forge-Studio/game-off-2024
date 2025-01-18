using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    private void Start()
    {
        int musicNumber = Random.Range(0, 2);
        AudioManager.instance.PlayMusic(AudioManager.instance.menuMusicSource, musicNumber);
        Cursor.visible = true;
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
