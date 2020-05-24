using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTrackingData 
{

    public int enemySlainCount;
    public int fightsTotalCount;
    public int fightsWonCount;
    public int stageCount;

    public GameTrackingData()
    {
        enemySlainCount = GameTracking.enemySlainCount;
        fightsTotalCount = GameTracking.fightsTotalCount;
        fightsWonCount = GameTracking.fightsWonCount;
        stageCount = GameTracking.stageCount;
    }

}
