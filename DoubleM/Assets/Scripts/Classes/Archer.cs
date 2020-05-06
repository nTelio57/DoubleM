using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Archer : MonoBehaviour
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
            float damage = BattleSystem.friendlyCurrentFighter.attackDamage;
            if (critChanceRoll <= BattleSystem.friendlyCurrentFighter.critChance)
                damage *= 1.5f;

            bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage((int)damage);
            setBattleText("<color=blue>Archer</color> hits for <color=red>" + (int)damage + "</color> damage", 1);

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
            int amount = (int)(0.3 * f.maxHP);
            if (f.maxHP - f.currentHP < amount)
                amount = f.maxHP - f.currentHP;
            
            setBattleText("<color=blue>Archer</color> restores <color=green>" + amount + "</color> HP", 1);
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
            Fighter[] eArray = FindObjectOfType<BattleSystem>().getAllFightersArray();
            float damage = BattleSystem.friendlyCurrentFighter.attackDamage / 2;
            for (int i = 0; i<eArray.Length; i++)
            {
                bool isDead = eArray[i].TakeDamage((int)damage);
                setBattleText("<color=blue>Archer</color> hits for <color=red>" + (int)damage + "</color> damage", 1);

                BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetBool("IsAttacking", true);
                yield return new WaitForSeconds(1);
                BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetBool("IsAttacking", false);
                yield return new WaitForSeconds(1);
            }
        }

        public void AbilityFour()
        {
            StartCoroutine(AbilityFourEnum());
        }

        IEnumerator AbilityFourEnum()
        {
            Fighter f = BattleSystem.friendlyCurrentFighter;
            f.attackDamage = f.attackDamage + 2;
            f.critChance = f.critChance + 2;

            setBattleText("<color=blue>Archer</color> enraged <color=red>+Crit +Dmg</color>", 1);

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

