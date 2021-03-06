﻿using System.Collections;
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
    public int victoryLoot;
    public Transform checkpoint;
    public Spawner spawner;
    public bool mainPoint;
    public bool isCaptured = false;
    bool loaded = false;
    public ParticleSystem particles;
    public string TBCScene;
    public static string TBCScenePass;

    public Fighter[] fighters;
    static public Fighter[] fighterPass;

    
    static int victoryLootPass;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInParent<CapturePointManager>();
    }
    public static Fighter[] getFighters()
    {
        return fighterPass;
    }

    public static int getVictoryLoot()
    {
        return victoryLootPass;
    }

    public int getVictoriesAmount()
    {
        return victories;
    }

    public void Victory()
    {
        if(particles != null)
            particles.Stop();
        victories++;
        isCaptured = true;
        if(spawner != null)
            spawner.isActive = false;
        GameTracking.fightsWonCount += 1;
        if(mainPoint)
            manager.setCapturePointTaken();
        
        if (checkpoint != null)
        {
            CheckpointManager.currentCheckpoint = checkpoint;
            FindObjectOfType<CheckpointManager>().Respawn();
        }
        
    }
    


    public void StartTheFight(Collider2D collision)
    {
        if (!loaded && !isCaptured && !isTBCopen())
        {
            fighterPass = fighters;
            victoryLootPass = victoryLoot;
            TBCScenePass = TBCScene;
            currentCapturePoint = this;
            GameTracking.fightsTotalCount += 1;
            loaded = true;
            attempts++;
            FindObjectOfType<AudioManager>().Stop("Background1");
            SceneManager.LoadScene(TBCScene, LoadSceneMode.Additive);
            
        }
    }

    bool isTBCopen()
    {
        return SceneManager.GetSceneByName(TBCScene).isLoaded;
    }

    public void ExitedCollision(Collider2D collision)
    {
        loaded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            StartTheFight(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            ExitedCollision(collision);
    }
}
