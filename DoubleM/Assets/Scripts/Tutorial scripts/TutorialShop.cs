using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShop : MonoBehaviour
{
    public GameObject shopObject;
    public GameObject upgradeObject;

    [HideInInspector]
    public bool isUpgradesCompleted;

    // Start is called before the first frame update
    void Start()
    {
        if (!Tutorial.isCompleted)
        {
            loadTutorialShop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Tut 1 shop
    public void onTutorialShopClick()
    {
        Time.timeScale = 1f;
        shopObject.SetActive(false);
    }

    public void loadTutorialShop()
    {
        Time.timeScale = 0f;
        shopObject.SetActive(true);
    }
    #endregion


    public void onTutorialUpgradeClick()
    {
        Time.timeScale = 1f;
        upgradeObject.SetActive(false);
    }

    public void loadTutorialUpgrade()
    {
        Time.timeScale = 0f;
        upgradeObject.SetActive(true);
    }


}
