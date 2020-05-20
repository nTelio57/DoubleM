using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text stageCount;
    public Text enemiesCount;
    public Text fightsCount;
    public Text totalCount;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        stageCount.text = GameTracking.stageCount + "";
        enemiesCount.text = GameTracking.enemySlainCount + "";
        fightsCount.text = GameTracking.fightsWonCount + "";
        totalCount.text = GameTracking.getTotalScore() + "";

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
