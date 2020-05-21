using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startingSceneName;

    void Start()
    {
        JoystickManager.init();
    }

    public void OnPlayClick()
    {
        SceneManager.LoadScene(startingSceneName, LoadSceneMode.Single);
        SceneManager.LoadScene("HeroShop", LoadSceneMode.Additive);
    }

    public void OnOptionsClick()
    {
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
    }

    public void OnCreditsClick()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Additive);
    }
}
