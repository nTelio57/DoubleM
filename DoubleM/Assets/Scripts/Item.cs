using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GameObject stats;
    public GameObject upgrades;
    public Text priceText;
    public Text quantityText;
    public Text nameText;
    public int ID;
    public int price;
    public string itemName;

    public Upgrade health;
    public Upgrade attack;
    public Upgrade defense;

    public string name1;
    [TextArea(3,5)]
    public string text1;

    public string name2;
    [TextArea(3, 5)]
    public string text2;

    public string name3;
    [TextArea(3, 5)]
    public string text3;

    public string name4;
    [TextArea(3, 5)]
    public string text4;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price + "";
        nameText.text = itemName;
        setUpgradePrices();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setQuantityTexts(int quantity)
    {
        quantityText.text = "(" + quantity + ")";
    }

    public void setUpgradePrices()
    {
        health.priceText.text = health.price+"";
        attack.priceText.text = attack.price + "";
        defense.priceText.text = defense.price + "";
    }

    public void onUpgradeClick(string type)
    {
        Upgrade temp = null;

        if (type == "health")
            temp = health;
        if (type == "attack")
            temp = attack;
        if (type == "defense")
            temp = defense;

        if (Vault.money < temp.price)
            return;
        Vault.addMoney(-temp.price);

        for (int i = 0; i < Heroes.count; i++)
            Heroes.getHero(i).Upgrade(type, temp.amount);
        
        AllHeros.GetFighter(ID).Upgrade(type, temp.amount);

        FindObjectOfType<Shop>().updateBalance();
    }
}
