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
        maxCapacity = 5;
        fighters = new Fighter[maxCapacity];
        count = 0;
        /*Fighter f = new Fighter(AllHeros.GetFighter(0));
        Fighter f2 = new Fighter(AllHeros.GetFighter(0));
        Fighter f3 = new Fighter(AllHeros.GetFighter(0));
        Fighter f4 = new Fighter(AllHeros.GetFighter(0));
        AddHero(f);
        AddHero(f2);
        AddHero(f3);*/
        // AddHero(AllHeros.GetFighter(0));
        // AddHero(AllHeros.GetFighter(0));
    }

    void Update()
    {
    }

    public static void AddHero(Fighter hero)
    {
        fighters[count++] = hero;
    }

    public static void RemoveHero(int index)
    {
        for (int i = index; i < count - 1; i++)
        {
            fighters[i] = fighters[i + 1];
        }
        count--;
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

    public static int getCountOfHeroByID(int id)
    {
        int sum = 0;
        for (int i = 0; i < count; i++)
        {
            if (fighters[i].ID == id)
                sum++;
        }
        return sum;
    }

    public static void Reset()
    {
        fighters = new Fighter[maxCapacity];
        count = 0;
    }

}
