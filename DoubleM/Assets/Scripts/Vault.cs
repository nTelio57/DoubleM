using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour
{
    public bool loadOnStart = false;
    static int money;
    public int startingMoney;

    // Start is called before the first frame update
    void Start()
    {
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
}
