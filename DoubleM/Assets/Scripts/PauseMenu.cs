using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenu;
    public Canvas thisScenesCanvas;
    public JoystickManager controls;
    

    public void onMenuClick()
    {
        FindObjectOfType<SaveGame>().Save();
        Time.timeScale = 1f;
        GameIsPaused = false;
        Destroy(FindObjectOfType<AllHeros>().gameObject);
        Destroy(FindObjectOfType<Vault>().gameObject);
        Destroy(FindObjectOfType<AudioManager>().gameObject);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void onOptionsCLick()
    {
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
    }

    public void onPauseClick(bool value)
    {
        if (value)
            Pause();
        else
            Resume();
    }
    

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    
}
