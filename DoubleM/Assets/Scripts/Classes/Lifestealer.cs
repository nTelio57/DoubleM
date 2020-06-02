using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifestealer : MonoBehaviour
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
        Fighter f = BattleSystem.friendlyCurrentFighter;
        float critChanceRoll = Random.value * 100;
        float damage = BattleSystem.friendlyCurrentFighter.attackDamage;
        if (critChanceRoll <= BattleSystem.friendlyCurrentFighter.critChance)
            damage *= 1.5f;

        bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage((int)damage);
        setBattleText("<color=blue>Lifestealer</color> hits for <color=red>" + (int)damage + "</color> damage", 1);

        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
        FindObjectOfType<AudioManager>().Play("Attack1");
        yield return new WaitForSeconds(1);

        int amount = (int)(0.15 * damage);

        setBattleText("<color=blue>Lifestealer</color> restores <color=green>" + amount + "</color> HP", 1);
        f.Heal(amount);

        yield return new WaitForSeconds(1);

    }

    public void AbilityTwo()
    {
        StartCoroutine(AbilityTwoEnum());
    }

    IEnumerator AbilityTwoEnum()
    {
        Fighter f = BattleSystem.friendlyCurrentFighter;
        float critChanceRoll = Random.value * 100;
        float damage = BattleSystem.friendlyCurrentFighter.attackDamage;
        int lowestHpFriendly = getLowestHpFriendlyIndex();
        if (critChanceRoll <= BattleSystem.friendlyCurrentFighter.critChance)
            damage *= 1.5f;

        bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage((int)damage);
        setBattleText("<color=blue>Lifestealer</color> hits for <color=red>" + (int)damage + "</color> damage", 1);

        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
        FindObjectOfType<AudioManager>().Play("Attack1");
        yield return new WaitForSeconds(1);

        int amount = (int)(0.03 * Heroes.getHero(lowestHpFriendly).maxHP);
        Heroes.getHero(lowestHpFriendly).Heal(amount);
        setBattleText("<color=blue>Lifestealer</color> restores <color=green>" + amount + "</color> HP", 1);

        yield return new WaitForSeconds(1);

    }

    public void AbilityThree()
    {
        StartCoroutine(AbilityThreeEnum());
    }

    IEnumerator AbilityThreeEnum()
    {
        Fighter f = BattleSystem.friendlyCurrentFighter;
        float critChanceRoll = Random.value * 100;
        Fighter[] eArray = FindObjectOfType<BattleSystem>().getAllFightersArray();
        float damage = BattleSystem.friendlyCurrentFighter.attackDamage * f.maxHP / f.currentHP;
        if (critChanceRoll <= BattleSystem.friendlyCurrentFighter.critChance)
            damage *= 1.5f;
        bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage((int)damage);
        setBattleText("<color=blue>Lifestealer</color> hits for <color=red>" + (int)damage + "</color> damage", 1);

        BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
        FindObjectOfType<AudioManager>().Play("Attack1");
        yield return new WaitForSeconds(1);
        int amount = 0;
        if (isDead)
            amount = (int)(0.3 * f.maxHP);
        else
            amount = (int)(0.15 * f.maxHP);
        f.Heal(amount);
        setBattleText("<color=blue>Lifestealer</color> restores <color=green>" + amount + "</color> HP", 1);
        yield return new WaitForSeconds(1);
    }

    public void AbilityFour()
    {
        StartCoroutine(AbilityFourEnum());
    }

    IEnumerator AbilityFourEnum()
    {
        Fighter f = BattleSystem.friendlyCurrentFighter;
        float critChanceRoll = Random.value * 100;
        Fighter[] eArray = FindObjectOfType<BattleSystem>().getAllFightersArray();
        float damage = BattleSystem.friendlyCurrentFighter.attackDamage * f.maxHP / f.currentHP / 2;
        if (critChanceRoll <= BattleSystem.friendlyCurrentFighter.critChance)
            damage *= 1.5f;
        for (int i = 0; i < eArray.Length; i++)
        {
            bool isDead = eArray[i].TakeDamage((int)damage);
            setBattleText("<color=blue>Lifestealer</color> hits for <color=red>" + (int)damage + "</color> damage", 1);

            BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
            

        }
        FindObjectOfType<AudioManager>().Play("Attack2");
        yield return new WaitForSeconds(1);
        for (int i=0; i<Heroes.count; i++)
        {
            int amount = (int)(0.02 * Heroes.getHero(i).maxHP);
            Heroes.getHero(i).Heal(amount);
            setBattleText("<color=blue>All alies</color> restores <color=green>" + amount + "</color> HP", 1);
        }
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

    public int[] getLowestHpEnemyIndex(Fighter[] fighters)
    {
        int[] temp = new int[2];
        int low1 = int.MaxValue;
        int low2 = int.MaxValue;
        int id1 = 0, id2 = 0;


        for (int i = 0; i < fighters.Length; i++)
        {
            if (fighters[i].currentHP <= low1)
            {
                low1 = fighters[i].currentHP;
                id1 = i;
            }
        }

        for (int i = 0; i < fighters.Length; i++)
        {
            if (fighters[i].currentHP <= low2 && i != id1)
            {
                low2 = fighters[i].currentHP;
                id2 = i;
            }
        }

        temp[0] = id1;
        temp[1] = id2;
        return temp;
    }
    public int getLowestHpFriendlyIndex()
    {
        int index = 0;
        int lowestHp = 900000;
        for(int i=0; i<Heroes.count; i++)
        {
            if (lowestHp > Heroes.getHero(i).currentHP)
            {
                lowestHp = Heroes.getHero(i).currentHP;
                index = i;
            }
        }
        return index;
    }


}

