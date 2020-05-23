using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Start()
    {
        if (!SaveOptions.isGameSaved)
            return;

        LoadPlayer();
        LoadVault();
        LoadHeroes();
    }

    public void Save()
    {
        SaveOptions.isGameSaved = true;
        SaveSystem.SavePlayer(GetComponent<Combat>(), GetComponent<Movement>());
        SaveSystem.SaveVault();
        SaveSystem.SaveHeroes();
        SaveSystem.SaveOptions();
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
