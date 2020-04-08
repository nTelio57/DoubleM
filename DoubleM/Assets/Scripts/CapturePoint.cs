using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleReward { None, Health, Attack, DodgeChance, Money, Armor}

public class CapturePoint : MonoBehaviour
{
    public static CapturePoint currentCapturePoint;
    CapturePointManager manager;
    static int attempts = 0;
    public int victories = 0;
    public BattleReward reward;
    public bool mainPoint;
    public bool isCaptured = false;
    public bool isSending = true;
    bool loaded = false;
    public string TBCScene;

    public Fighter[] fighters;
    static public Fighter[] fighterPass;

    public int victoryLoot;
    static int victoryLootPass;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<CapturePointManager>();
    }
    public Fighter[] getFighters()
    {
        return fighterPass;
    }

    public int getVictoryLoot()
    {
        return victoryLootPass;
    }

    public int getVictoriesAmount()
    {
        return victories;
    }

    public void Victory()
    {
        victories++;
        isCaptured = true;
        if(mainPoint)
            manager.setCapturePointTaken();
    }
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!loaded)
        {
            if (isSending)
            {
                fighterPass = fighters;
                victoryLootPass = victoryLoot;
                currentCapturePoint = this;
            }

            loaded = true;
            attempts++;
            SceneManager.LoadScene(TBCScene, LoadSceneMode.Additive);
        }
    }

}
