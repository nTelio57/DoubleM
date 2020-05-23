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
            damage *= 1.5f;
        
        bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage((int)damage);
        setBattleText("<color=blue>Ninja</color> hits for <color=red>" + (int)damage + "</color> damage", 1);

        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetInteger("attackId", 0);
        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
        FindObjectOfType<AudioManager>().Play("Attack1");
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(1);
    }

    public void AbilityTwo()
    {
        StartCoroutine(AbilityTwoEnum());
    }

    IEnumerator AbilityTwoEnum()
    {
        Fighter f = BattleSystem.friendlyCurrentFighter;
        int amount = (int)(0.3 * f.maxHP);
        if (f.maxHP - f.currentHP < amount)
            amount = f.maxHP - f.currentHP;
        BattleSystem.friendlyCurrentFighter.addEffect(Effect.Rage);

        setBattleText("<color=blue>Ninja</color> restores <color=green>" + amount + "</color> HP", 1);
        f.Heal(amount);

        yield return new WaitForSeconds(1);
    }

    public void AbilityThree()
    {
        StartCoroutine(AbilityThreeEnum());
    }

    IEnumerator AbilityThreeEnum()
    {
        Fighter f = BattleSystem.friendlyCurrentFighter;
        f.addEffect(Effect.Untargetable);
        setBattleText("<color=blue>Ninja</color> is untargetable for one turn", 1);
        yield return new WaitForSeconds(1);
    }

    public void AbilityFour()
    {
        StartCoroutine(AbilityFourEnum());
    }

    IEnumerator AbilityFourEnum()
    {
        Fighter f = BattleSystem.friendlyCurrentFighter;
        Fighter e = BattleSystem.enemyCurrentFighter;
        int damage = (int)(f.attackDamage*0.15 + ((2.75 * missingHealthPercentage(e)) * f.attackDamage));

        bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage(damage);

        setBattleText("<color=blue>Ninja</color> strikes for <color=red>" + damage + "</color> damage", 1);
        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetInteger("attackId", 1);
        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
        FindObjectOfType<AudioManager>().Play("Attack2");
        yield return new WaitForSeconds(1);
        
    }

    float missingHealthPercentage(Fighter f)
    {
        float currentPercentage = f.currentHP * 100 / f.maxHP;
        return 1 - (currentPercentage / 100);
    }

    public void setBattleText(string text, int duration)
    {
        BattleSystem bs = FindObjectOfType<BattleSystem>();
        bs.setBattleText(text, duration);
    }

}
