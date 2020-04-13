using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : MonoBehaviour
{
    public void AbilityPassive()
    {

    }

    public void AbilityOne()
    {
        StartCoroutine(AbilityOneEnum());
    }


    IEnumerator AbilityOneEnum() {
        
        float critChanceRoll = Random.value * 100;
        float damage = BattleSystem.friendlyCurrentFighter.attackDamage;
        if (critChanceRoll <= BattleSystem.friendlyCurrentFighter.critChance)
            damage *= 2;

        bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage((int)damage);

        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetBool("IsAttacking", true);
        yield return new WaitForSeconds(1);
        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetBool("IsAttacking", false);
        yield return new WaitForSeconds(1);
    }

    public void AbilityTwo()
    {
        StartCoroutine(AbilityTwoEnum());
    }

    IEnumerator AbilityTwoEnum()
    {
        Fighter f = BattleSystem.friendlyCurrentFighter;
        f.AbilityTwo.cooldownCurrently = f.AbilityTwo.cooldown;
        int amount = (int)(0.3 * f.maxHP);
        f.Heal(amount);

        yield return new WaitForSeconds(1);
    }

    public void AbilityThree()
    {
        StartCoroutine(AbilityThreeEnum());
    }

    IEnumerator AbilityThreeEnum()
    {

        yield return new WaitForSeconds(1);
    }
}
