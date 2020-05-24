using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroesData 
{
    //all heroes
    public float[] allMaxHp;
    public float[] allAttackDamage;
    public float[] allCritChance;

    //heroes
    public int count;
    public int[] id;
    public float[] currentHP;

    public HeroesData()
    {
        //all heroes
        int allCount = AllHeros.heroes.Length;

        allMaxHp = new float[allCount];
        allAttackDamage = new float[allCount];
        allCritChance = new float[allCount];

        for (int i = 0; i < allCount; i++)
        {
            allMaxHp[i] = AllHeros.heroes[i].maxHP;
            allAttackDamage[i] = AllHeros.heroes[i].attackDamage;
            allCritChance[i] = AllHeros.heroes[i].critChance;
        }

        //heroes
        count = Heroes.count;
        id = new int[count];
        currentHP = new float[count];
        for (int i = 0; i < count; i++)
        {
            id[i] = Heroes.getHero(i).ID;
            currentHP[i] = Heroes.getHero(i).currentHP;
        }
    }
}
