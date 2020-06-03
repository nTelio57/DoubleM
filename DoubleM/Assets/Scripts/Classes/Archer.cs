using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Archer : MonoBehaviour
    {
        int critBoost = 0;
        int dmgBoost = 0;
        public void AbilityPassive()
        {

        }

        public void AbilityOne()
        {
            StartCoroutine(AbilityOneEnum());
        }


        IEnumerator AbilityOneEnum()
        {

            float critChanceRoll = Random.value * 100 + critBoost;
            float damage = BattleSystem.friendlyCurrentFighter.attackDamage + dmgBoost;
            if (critChanceRoll <= BattleSystem.friendlyCurrentFighter.critChance)
                damage *= 1.5f;

            bool isDead = BattleSystem.enemyCurrentFighter.TakeDamage((int)damage);
            setBattleText("<color=blue>Archer</color> hits for <color=red>" + (int)damage + "</color> damage", 1);

            BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
            FindObjectOfType<AudioManager>().Play("Bow1");
            yield return new WaitForSeconds(1);
            critBoost = 0;
            dmgBoost = 0;
        }

        public void AbilityTwo()
        {
            StartCoroutine(AbilityTwoEnum());
        }

        IEnumerator AbilityTwoEnum()
        {
            float critChanceRoll = Random.value * 100 + critBoost;
            float damage = BattleSystem.friendlyCurrentFighter.attackDamage + dmgBoost;
            Fighter[] eArray = FindObjectOfType<BattleSystem>().getAllFightersArray();
            int[] te = getLowestHpEnemyIndex(eArray);
            if (critChanceRoll <= BattleSystem.friendlyCurrentFighter.critChance)
                damage *= 1.5f;
            if(eArray.Length > 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
                    yield return new WaitForSeconds(1);
                    bool isDead = eArray[te[i]].TakeDamage((int)damage);
                    setBattleText("<color=blue>Archer</color> hits for <color=red>" + (int)damage + "</color> damage", 1);
                }
                FindObjectOfType<AudioManager>().Play("Bow1");
            }
            else
            {
                BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
                FindObjectOfType<AudioManager>().Play("Bow1");
                yield return new WaitForSeconds(1);
                int damage1 = (int)(damage * 0.90);
                bool isDead = eArray[0].TakeDamage(damage1);
                setBattleText("<color=blue>Archer</color> hits for <color=red>" + (int)damage1 + "</color> damage", 1);
                yield return new WaitForSeconds(1);
                BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
                FindObjectOfType<AudioManager>().Play("Bow1");
                yield return new WaitForSeconds(1);
                int damage2 = (int)(damage * 0.50);
                isDead = eArray[0].TakeDamage(damage2);
                setBattleText("<color=blue>Archer</color> hits for <color=red>" + (int)damage2 + "</color> damage", 1);
            }
            

            //BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
            //yield return new WaitForSeconds(1);

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

                
            }
            BattleSystem.friendlyCurrentPrefab.GetComponent<Animator>().SetTrigger("Attack");
            FindObjectOfType<AudioManager>().Play("Bow1");
            yield return new WaitForSeconds(1);
        }

        public void AbilityFour()
        {
            StartCoroutine(AbilityFourEnum());
        }

        IEnumerator AbilityFourEnum()
        {
            Fighter f = BattleSystem.friendlyCurrentFighter;
            dmgBoost = dmgBoost + 2;
            critBoost = critBoost + 2;

            setBattleText("<color=blue>Archer</color> enraged <color=red>+Crit +Dmg</color>", 1);
            FindObjectOfType<AudioManager>().Play("Rage1");
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
    }

