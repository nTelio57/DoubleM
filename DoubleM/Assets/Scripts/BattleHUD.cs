using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text hpText;
    private Fighter fighter;
    

    public void setHUD(Fighter f)
    {
        fighter = f;
        nameText.text = fighter.name;
        hpText.text = fighter.currentHP + "/" + fighter.maxHP;
    }

    public void SetHP(int hp)
    {
        hpText.text = fighter.currentHP + "/" + fighter.maxHP;
    }
}
