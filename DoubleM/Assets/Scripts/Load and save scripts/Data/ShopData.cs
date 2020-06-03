using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopData
{
    public int[,] upgradePrices;

    public ShopData(Item[] items, int attributeCount)
    {
        upgradePrices = new int[items.Length, attributeCount];

        for (int i = 0; i < items.Length; i++)
        {
            upgradePrices[i,0] = items[i].health.price;
            upgradePrices[i, 1] = items[i].attack.price;
            upgradePrices[i, 2] = items[i].defense.price;
        }
    }

}
