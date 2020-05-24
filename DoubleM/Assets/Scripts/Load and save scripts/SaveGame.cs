using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{

    private void OnApplicationQuit()
    {
        if(GameOver.status == GameOverStatus.InGame)
            Save();
    }

    private void Start()
    {
        if (!SaveOptions.isGameSaved)
            return;
        
        LoadPlayer();
        LoadVault();
        LoadHeroes();
        LoadCapturePoints();
        LoadGameTracking();
        LoadEnemies();
    }

    public void Save()
    {
        SaveOptions.isGameSaved = true;
        SaveSystem.SavePlayer(GetComponent<Combat>(), GetComponent<Movement>());
        SaveSystem.SaveVault();
        SaveSystem.SaveHeroes();
        SaveSystem.SaveOptions();
        SaveSystem.SaveCP(FindObjectOfType<CapturePointManager>());
        SaveSystem.SaveGameTracking();
        SaveSystem.SaveEnemies(FindObjectOfType<CapturePointManager>());
    }

    void LoadEnemies()
    {
        EnemyData data = SaveSystem.LoadEnemies();
        CapturePointManager manager = FindObjectOfType<CapturePointManager>();

        Vector3 pos;
        Spawner spawner;
        GameObject prefab;
        GameObject newPrefab;

        for (int i = 0; i < data.count; i++)
        {

            spawner = manager.capturePoints[data.spawnerId[i]].spawner;
            prefab = spawner.fighters[data.prefabId[i]];
            pos.x = data.x[i];
            pos.y = data.y[i];
            pos.z = data.z[i];

            newPrefab = Instantiate(prefab, pos, Quaternion.identity, spawner.transform);
            newPrefab.GetComponent<ParentSpawnerInfo>().prefabId = data.prefabId[i];
            newPrefab.GetComponent<ParentSpawnerInfo>().spawnerId = data.spawnerId[i];
            spawner.entitiesCount++;
        }
        
    }

    void LoadGameTracking()
    {
        GameTrackingData data = SaveSystem.LoadGameTracking();

        GameTracking.enemySlainCount = data.enemySlainCount;
        GameTracking.fightsTotalCount = data.fightsTotalCount;
        GameTracking.fightsWonCount = data.fightsWonCount;
        GameTracking.stageCount = data.stageCount;
    }

    void LoadCapturePoints()
    {
        CapturePointData data = SaveSystem.LoadCP();
        CapturePointManager manager = FindObjectOfType<CapturePointManager>();
        int count = 0;

        for (int i = 0; i < manager.capturePoints.Length; i++)
        {
            manager.capturePoints[i].isCaptured = data.isCaptured[i];
            if (data.isCaptured[i])
                count++;
        }
        manager.mainCapturePointCurrentCount = count;
    }

    void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        Combat player = GetComponent<Combat>();
        Movement movement = GetComponent<Movement>();

        if (data == null)
            return;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
        player.currentHealth = (int)data.currentHealth;
        player.maxHealth = (int)data.maxHealth;

        movement.stamina = data.currentStamina;
        movement.maxStamina = data.maxStamina;
    }

    void LoadVault()
    {
        VaultData data = SaveSystem.LoadVault();

        Vault.money = data.money;
        Vault.chances = data.chances;
    }

    void LoadHeroes()
    {
        HeroesData data = SaveSystem.LoadHeroes();

        //all heroes
        for (int i = 0; i < AllHeros.heroes.Length; i++)
        {
            AllHeros.heroes[i].maxHP = (int)data.allMaxHp[i];
            AllHeros.heroes[i].attackDamage = (int)data.allAttackDamage[i];
            AllHeros.heroes[i].critChance = (int)data.allCritChance[i];
        }

        //heroes
        for (int i = 0; i < data.count; i++)
        {
            Fighter f = new Fighter(AllHeros.GetFighter(data.id[i]));
            f.setID(data.id[i]);
            f.currentHP =(int)data.currentHP[i];
            Heroes.AddHero(f);
        }
    }
}
