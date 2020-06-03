using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Panel management")]
    public GameObject shopPanel;
    public GameObject infoPanel;
    public GameObject moneyPanel;
    [Header("Buttons")]
    public GameObject nextLevelButtonPanel;
    public GameObject upgradeButtonPanel;
    [Header("Misc")]
    public InfoPanelManager infoPanelManager;

    public bool isMandatory;
    public Button nextLevelButton;
    public Button upgradeButton;
    public Text balance;
    public Text upgradeButtonText;
    [Header("Item list")]
    public Item[] items;

    bool isUpgradesVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        checkIfMandatory();
        updateBalance();
        if (isMandatory)
        {
            nextLevelButton.interactable = false;
            upgradeButton.interactable = false;
        }
        setQuantityTexts();
        loadUpgradePrices();
        SaveOptions.isShopActive = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void setQuantityTexts()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].setQuantityTexts(Heroes.getCountOfHeroByID(i));
        }
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
        saveUpgradePrices();
        SaveOptions.isShopActive = false;
        SceneManager.UnloadSceneAsync("HeroShop");
        FindObjectOfType<CheckpointManager>().Respawn();
    }

    public void onHeroBuyClick(int itemID)
    {
        if (getItemByID(itemID).price <= Vault.money)
        {
            Fighter f = new Fighter(AllHeros.GetFighter(itemID));
            f.setID(itemID);
            Heroes.AddHero(f);
            Vault.addMoney(-getItemByID(itemID).price);
            nextLevelButton.interactable = true;
            upgradeButton.interactable = true;
            isMandatory = false;
        }
        updateBalance();
        setQuantityTexts();

        //Tutorial load
        if (!Tutorial.isCompleted && !FindObjectOfType<TutorialShop>().isUpgradesCompleted)
        {
            FindObjectOfType<TutorialShop>().loadTutorialUpgrade();
            FindObjectOfType<TutorialShop>().isUpgradesCompleted = true;
        }

    }

    public void updateBalance()
    {
        balance.text = Vault.money + "";
    }

    public void onUpgradesClick()
    {
        if (isUpgradesVisible)
        {
            isUpgradesVisible = false;
            for (int i = 0; i < items.Length; i++)
            {
                items[i].upgrades.SetActive(false);
                items[i].stats.SetActive(true);
                upgradeButtonText.text = "Upgrades";
            }
        }
        else
        {
            isUpgradesVisible = true;
            for (int i = 0; i < items.Length; i++)
            {
                items[i].upgrades.SetActive(true);
                items[i].stats.SetActive(false);
                upgradeButtonText.text = "Stats";
            }

        }
    }

    public Item getItemByID(int ID)
    {
        foreach (Item i in items)
        {
            if (i.ID == ID)
                return i;
        }
        return null;
    }

    public void onInfoPanelGoBackClick()
    {
        shopPanel.SetActive(true);
        moneyPanel.SetActive(true);
        nextLevelButtonPanel.SetActive(true);
        infoPanel.SetActive(false);
        upgradeButtonPanel.SetActive(true);
    }

    public void onInfoPanelClick(int index)
    {
        infoPanelManager.setTextfields(AllHeros.GetFighter(index));
        shopPanel.SetActive(false);
        moneyPanel.SetActive(false);
        nextLevelButtonPanel.SetActive(false);
        infoPanel.SetActive(true);
        upgradeButtonPanel.SetActive(false);
    }

    public void loadUpgradePrices()
    {
        ShopData data = SaveSystem.LoadShop();

        if (data == null)
        {
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            items[i].health.price = data.upgradePrices[i, 0];
            items[i].attack.price = data.upgradePrices[i, 1];
            items[i].defense.price = data.upgradePrices[i, 2];
            items[i].setUpgradePrices();
        }
    }

    public void saveUpgradePrices()
    {
        SaveSystem.SaveShop(items, 3);
    }

    public static void Reset()
    {
        SaveSystem.ResetShop();
    }

}

