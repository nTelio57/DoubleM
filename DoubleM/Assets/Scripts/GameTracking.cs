using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracking
{
    public static int enemySlainCount = 0;
    public static int stageCount = 1;
    public static int fightsWonCount = 0;
    public static int fightsTotalCount = 0;
    public static int totalScore = 0;

    public static int getTotalScore()
    {
        int temp;
        temp = stageCount * (fightsWonCount) + enemySlainCount/2;
        return temp;
    }

    public static void Reset()
    {
        enemySlainCount = 0;
        stageCount = 1;
        fightsWonCount = 0;
        fightsTotalCount = 0;
        totalScore = 0;
    }
}
