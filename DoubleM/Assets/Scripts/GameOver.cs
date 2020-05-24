using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameOverStatus { Victory, Defeat, InGame}

public class GameOver : MonoBehaviour
{
    public Text stageCount;
    public Text enemiesCount;
    public Text fightsCount;
    public Text totalCount;

    [Header("Game status text")]
    public Text statusText;
    public static GameOverStatus status;
    public string victoryText;
    public Color victoryColor;
    public string defeatText;
    public Color defeatColor;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;

        if (status == GameOverStatus.Victory)
        {
            statusText.text = victoryText;
            statusText.color = victoryColor;
        }
        else if(status == GameOverStatus.Defeat)
        {
            statusText.text = defeatText;
            statusText.color = defeatColor;
        }

        stageCount.text = GameTracking.stageCount + "";
        enemiesCount.text = GameTracking.enemySlainCount + "";
        fightsCount.text = GameTracking.fightsWonCount + "";
        totalCount.text = GameTracking.getTotalScore() + "";

        SaveOptions.isGameSaved = false;
        SaveSystem.SaveOptions();

        GameTracking.Reset();
        FindObjectOfType<Vault>().Reset();
        Heroes.Reset();
    }

    public void onMainMenuClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void onPlayAgainClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Stage 1", LoadSceneMode.Single);
        SceneManager.LoadScene("HeroShop", LoadSceneMode.Additive);
    }

}
