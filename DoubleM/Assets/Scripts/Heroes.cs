using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Heroes : MonoBehaviour
{
    static int maxCapacity;
    static Fighter[] fighters;
    public static int count;


    // Start is called before the first frame update
    void Start()
    {
        maxCapacity = 4;
        fighters = new Fighter[maxCapacity];
        count = 0;

        AddHero(AllHeros.GetFighter(0));
    }

    public static void AddHero(Fighter hero)
    {
        fighters[count++] = hero;
    }

    public static Fighter getHero(int index)
    {
        return fighters[index];
    }

    public static Fighter[] getHeroArray()
    {
        return fighters;
    }

    public static void ResetCooldowns()
    {
        for (int i = 0; i < count; i++)
            fighters[i].resetCooldowns();
    }
}
