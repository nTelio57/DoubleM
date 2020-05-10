using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public MainStats player;
    public Combat playerCombat;
    public Animator playerAnimator;
    public Button attackButton;
    public Text hpText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setHpText(playerCombat.currentHealth);
    }

    public void setHpText(int h)
    {
        hpText.text = h+"";
    }

    public void onAttackClick()
    {
        playerAnimator.SetTrigger("Attack");
        playerCombat.Attack(player.damage);
    }

}
