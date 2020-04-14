using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum RequiredSelection { friendly, enemies, self, otherAllies}

[System.Serializable]
public class Ability
{
    public string name;
    public int cooldown;//statinis
    [HideInInspector]
    public int cooldownCurrently = 0;//naudojamas scripte
    public RequiredSelection requiredSelection;
    public UnityEvent ability;

}
