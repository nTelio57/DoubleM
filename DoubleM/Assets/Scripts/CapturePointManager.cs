using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CapturePointManager : MonoBehaviour
{
    public string nextLevelScene;
    int mainCapturePointTotal;
    public int mainCapturePointCurrentCount;

    public CapturePoint[] capturePoints;
    public CapturePoint[] bonusCapturePoints;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCapturePointCurrentCount = 0;
        mainCapturePointTotal = capturePoints.Length;
    }

    public void setCapturePointTaken()
    {
        mainCapturePointCurrentCount++;
        if (mainCapturePointCurrentCount == mainCapturePointTotal)
            SceneManager.LoadScene(nextLevelScene, LoadSceneMode.Single);
    }

    public int getCapturedPointsAmount()
    {
        int sum = 0;
        for (int i = 0; i < capturePoints.Length; i++)
        {
            sum += capturePoints[i].getVictoriesAmount();
        }
        return sum;
    }

    public int getCapturePointsTotalAmount()
    {
        return capturePoints.Length;
    }
}
