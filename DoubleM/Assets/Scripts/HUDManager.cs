using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Text moneyText;
    public CapturePointManager capturePointManager;
    public Text capturePointText;
    public string level;
    public Text levelText;
    public Text chancesText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = Vault.getMoney()+"";
        capturePointText.text = capturePointManager.getCapturedPointsAmount()+" / " + capturePointManager.getCapturePointsTotalAmount();
        levelText.text = level;
        chancesText.text = Vault.getChances() + "";
    }
}
