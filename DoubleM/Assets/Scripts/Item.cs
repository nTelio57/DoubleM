using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("Item")]
    public int ID;
    public int price;
    public string itemName;

    [Header("Text fields")]
    public GameObject stats;
    public GameObject upgrades;
    public Text priceText;
    public Text quantityText;
    public Text nameText;

    [Header("Statistic text fields")]
    public Text healthText;
    public Text attackText;
    public Text defenseText;

    [Header("Upgradables")]
    public Upgrade health;
    public Upgrade attack;
    public Upgrade defense;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price + "";
        nameText.text = itemName;
        setUpgradePrices();
        setStatisticText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setStatisticText()
    {
        healthText.text = AllHeros.GetFighter(ID).maxHP+"";
        attackText.text = AllHeros.GetFighter(ID).attackDamage + "";
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
        float newPrice = temp.price * temp.priceMultiplier;
        temp.price = (int)newPrice;


        for (int i = 0; i < Heroes.count; i++)
            Heroes.getHero(i).Upgrade(type, temp.amount);
        
        AllHeros.GetFighter(ID).Upgrade(type, temp.amount);

        FindObjectOfType<Shop>().updateBalance();
        setStatisticText();
        setUpgradePrices();
    }
}
