using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public Combat playerCombat;
    public Animator playerAnimator;
    public Button attackButton;
    [Header("Health bar")]
    public Healthbar playerHealthbar;
    public Text hpText;
    [Header("Attack speed bar")]
    public GameObject attackSpeedBarObject;
    public Healthbar playerAttackSpeedbar;
    public Text attackSpeedBarText;

    string healthText;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthbar.SetMaxHealth(playerCombat.maxHealth);
        playerAttackSpeedbar.SetMaxHealth(playerCombat.attackSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        healthText = playerCombat.currentHealth + "/" + playerCombat.maxHealth;
        setHpText(healthText);
        playerHealthbar.SetHealth(playerCombat.currentHealth);
        attackSpeedBarUpdate();
    }

    public void setHpText(string h)
    {
        hpText.text = h+"";
    }

    void attackSpeedBarUpdate()
    {
        if(playerCombat.attackSpeed - playerCombat.timerAttackSpeed <= 0)
            attackSpeedBarObject.SetActive(false);
        float value = playerCombat.attackSpeed - playerCombat.timerAttackSpeed;
        string temp = value.ToString("F2");
        playerAttackSpeedbar.SetHealth(playerCombat.attackSpeed - playerCombat.timerAttackSpeed);
        attackSpeedBarText.text = "Attack speed " + temp;
    }

    public void onAttackClick()
    {
        if (playerCombat.isAbleToAttack)
        {
            attackSpeedBarObject.SetActive(true);
            playerAnimator.SetTrigger("Attack");
            playerCombat.Attack();
        }
        
    }

}
