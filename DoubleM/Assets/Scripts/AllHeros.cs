using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllHeros : MonoBehaviour
{
    public Fighter[] heroesList;
    static Fighter[] heroes;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        heroes = heroesList;
    }

    public static Fighter GetFighter(string name)
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i].name == name)
                return heroes[i];
        }
        return null;
    }

    public static Fighter GetFighter(int id)
    {
        return heroes[id];
    }

}
