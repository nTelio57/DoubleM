using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour
{
    public static int chances;
    public bool loadOnStart = false;
    public static int money;
    public int startingMoney;
    public int startingChances;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (loadOnStart)
        {
            money = startingMoney;
            chances = startingChances;
        }
    }

    public static void addMoney(int amount)
    {
        money += amount;
    }

    public static int getMoney()
    {
        return money;
    }

    public static int getChances()
    {
        return chances;
    }

    public static void addChances(int amount)
    {
        chances += amount;
    }
}
