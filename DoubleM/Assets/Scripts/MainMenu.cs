using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startingSceneName;

    void Start()
    {
       
    }

    public void OnPlayClick()
    {
        GameOver.status = GameOverStatus.InGame;
        if (!SaveOptions.isGameSaved)
        {
            SaveOptions.currentStage = 1;
            SceneManager.LoadScene(startingSceneName, LoadSceneMode.Single);
        }
        else
        {
            string stageName = "Stage " + SaveOptions.currentStage;
            SceneManager.LoadScene(stageName, LoadSceneMode.Single);
        }

        if (!(SaveOptions.isGameSaved && !SaveOptions.isShopActive))
        {
            SceneManager.LoadScene("HeroShop", LoadSceneMode.Additive);
        }
            
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
