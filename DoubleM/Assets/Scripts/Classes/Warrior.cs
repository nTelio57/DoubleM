using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public void AbilityPassive()
    {

    }

    public void AbilityOne()
    {
        StartCoroutine(AbilityOneEnum());
    }


    IEnumerator AbilityOneEnum()
    {
        float critChanceRoll = Random.value * 100;
        float damage = BattleSystem.friendlyCurrentFighter.attackDamage / 2;
        if (critChanceRoll <= BattleSystem.friendlyCurrentFighter.critChance)
            damage *= 1.5f;

        bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage((int)damage);
        setBattleText("<color=blue>Warrior</color> hits for <color=red>" + (int)damage + "</color> damage", 1);
        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(1);

        BattleSystem.enemyCurrentFighter.addEffect(Effect.Weakness);
        setBattleText("<color=blue>Warrior</color> weakens <color=red>Enemy</color>", 1);
        yield return new WaitForSeconds(1);
    }

    public void AbilityTwo()
    {
        StartCoroutine(AbilityTwoEnum());
    }

    IEnumerator AbilityTwoEnum()
    {
        float critChanceRoll = Random.value * 100;
        float damage = BattleSystem.friendlyCurrentFighter.attackDamage;
        if (critChanceRoll <= BattleSystem.friendlyCurrentFighter.critChance)
            damage *= 1.5f;

        bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage((int)damage);
        setBattleText("<color=blue>Warrior</color> hits for <color=red>" + (int)damage + "</color> damage", 1);
        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(1);

        BattleSystem.enemyCurrentFighter.addEffect(Effect.Bleeding);
        setBattleText("<color=blue>Warrior</color> bleeds <color=red>Enemy</color>", 1);
        yield return new WaitForSeconds(1);
    }

    public void AbilityThree()
    {
        StartCoroutine(AbilityThreeEnum());
    }

    IEnumerator AbilityThreeEnum()
    {
        Fighter f = BattleSystem.friendlyCurrentFighter;
        float damage = BattleSystem.enemyCurrentFighter.currentHP;
        bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage((int)damage);
        setBattleText("<color=blue>Warrior</color> hits for <color=red>" + (int)damage + "</color> damage", 1);
        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(1);
        f.TakeDamage((int)damage / 2);
        setBattleText("<color=blue>Warrior</color> takes <color=red>" + (int)damage / 2 + "</color> damage", 1);
        yield return new WaitForSeconds(1);
    }

    public void AbilityFour()
    {
        StartCoroutine(AbilityFourEnum());
    }

    IEnumerator AbilityFourEnum()
    {
        float coinChance = Random.value * 100;
        int coinAmount = (int)Random.value * 10;
        if(coinChance >= 81)
        {
            Vault.addMoney(coinAmount);
            setBattleText("Coin flip <color=yellow>succeed</color>", 1);
        }
        else
            setBattleText("Coin flip <color=red>failed</color>", 1);
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
