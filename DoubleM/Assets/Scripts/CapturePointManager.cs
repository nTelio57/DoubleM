using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CapturePointManager : MonoBehaviour
{
    public bool isFinalStage;
    public string nextLevelScene;
    int mainCapturePointTotal;
    public int mainCapturePointCurrentCount;

    public CapturePoint[] capturePoints;
    public CapturePoint[] bonusCapturePoints;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!SaveOptions.isGameSaved)
            mainCapturePointCurrentCount = 0;
        mainCapturePointTotal = capturePoints.Length;

        for(int i = 0; i < mainCapturePointTotal; i++)
        {
            capturePoints[i].spawner.spawnerId = i;
        }
    }

    public Spawner getSpawner(int id)
    {
        for (int i = 0; i < capturePoints.Length; i++)
        {
            if (capturePoints[i].spawner.spawnerId == id)
                return capturePoints[i].spawner;
        }
        return null;
    }

    public void setCapturePointTaken()
    {
        mainCapturePointCurrentCount++;
        if (mainCapturePointCurrentCount == mainCapturePointTotal)
        {
            SaveOptions.isGameSaved = false;
            if (!isFinalStage)
            {
                GameTracking.stageCount += 1;
                SaveOptions.currentStage++;
                SceneManager.LoadScene(nextLevelScene, LoadSceneMode.Single);
                SceneManager.LoadScene("HeroShop", LoadSceneMode.Additive);
            }
            else
            {
                GameOver.status = GameOverStatus.Victory;
                SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
            }
            
        }
    }

    public int getCapturedPointsAmount()
    {
        int sum = 0;
        for (int i = 0; i < capturePoints.Length; i++)
        {
            if (capturePoints[i].isCaptured)
                sum++;
        }
        return sum;
    }

    public int getCapturePointsTotalAmount()
    {
        return capturePoints.Length;
    }
}
