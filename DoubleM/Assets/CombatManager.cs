using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public Combat playerCombat;
    public Animator playerAnimator;
    public Button attackButton;
    public Healthbar playerHealthbar;
    public Text hpText;

    string healthText;
    // Start is called before the first frame update
    void Start()
    {
        playerHealthbar.SetMaxHealth(playerCombat.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        healthText = playerCombat.currentHealth + "/" + playerCombat.maxHealth;
        setHpText(healthText);
        playerHealthbar.SetHealth(playerCombat.currentHealth);
    }

    public void setHpText(string h)
    {
        hpText.text = h+"";
    }

    public void onAttackClick()
    {
        playerAnimator.SetTrigger("Attack");
        playerCombat.Attack(playerCombat.damage);
    }

}
