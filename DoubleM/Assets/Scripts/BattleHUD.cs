using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text hpText;
    private Unit unit;

    public void setHUD(Unit u)
    {
        this.unit = u;
        nameText.text = unit.unitName;
        hpText.text = unit.currentHP + "/" + unit.maxHP;
    }

    public void SetHP(int hp)
    {
        hpText.text = unit.currentHP + "/" + unit.maxHP;
    }
}
