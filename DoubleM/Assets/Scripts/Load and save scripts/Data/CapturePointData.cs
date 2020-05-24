using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CapturePointData 
{
    public bool[] isCaptured;

    public CapturePointData(CapturePointManager manager)
    {
        isCaptured = new bool[manager.capturePoints.Length];

        for (int i = 0; i < manager.capturePoints.Length; i++)
        {
            isCaptured[i] = manager.capturePoints[i].isCaptured;
        }
    }
}
