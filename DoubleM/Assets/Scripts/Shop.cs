using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public bool isMandatory;
    public Button nextLevelButton;
    public Text balance;
    public Item[] items;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        checkIfMandatory();
        updateBalance();
        if (isMandatory)
            nextLevelButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkIfMandatory()
    {
        if (Heroes.count <= 0)
            isMandatory = true;
        else
            isMandatory = false;
    }

    public void onNextLevelClick()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("HeroShop");
    }

    public void onHeroBuyClick(int itemID)
    {
        if (getItemByID(itemID).price <= Vault.money)
        {
            Heroes.AddHero(new Fighter(AllHeros.GetFighter(itemID)));
            Vault.addMoney(-getItemByID(itemID).price);
            nextLevelButton.interactable = true;
            isMandatory = false;
        }
        updateBalance();

    }

    void updateBalance()
    {
        balance.text = Vault.money + "";
    }

    Item getItemByID(int ID)
    {
        foreach (Item i in items)
        {
            if (i.ID == ID)
                return i;
        }
        return null;
    }
}
