using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour
{
    public int chances = 3;
    public bool loadOnStart = false;
    static int money;
    public int startingMoney;

    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (loadOnStart)
        {
            money = startingMoney;
        }
    }

    public void addMoney(int amount)
    {
        money += amount;
    }

    public int getMoney()
    {
        return money;
    }

    public int getChances()
    {
        return chances;
    }

    public void addChances(int amount)
    {
        chances += amount;
    }
}
