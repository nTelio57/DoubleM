using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum RequiredSelection { friendly, enemies, self, otherAllies}

[System.Serializable]
public class Ability
{
    public string name;
    [TextArea(2,5)]
    public string describtion;
    public int cooldown;//statinis
    [HideInInspector]
    public int cooldownCurrently = 0;//naudojamas scripte
    public RequiredSelection requiredSelection;
    public UnityEvent ability;

    public Ability(Ability a)
    {
        name = a.name;
        describtion = a.describtion;
        cooldown = a.cooldown;
        cooldownCurrently = a.cooldownCurrently;
        requiredSelection = a.requiredSelection;
        ability = a.ability;
    }

}
