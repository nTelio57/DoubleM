using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Fighter
{
    public GameObject prefab;
    //public Sprite sprite;
    public Animator animator;
    public bool flipSpriteOnX = false;
    public string name;
    public int maxHP;
    public int currentHP;
    public int attackDamage;
    public float critChance;
    public bool isTargetable = true;
    public List<Effect> effects = new List<Effect>();
    public Ability AbilityOne;
    public Ability AbilityTwo;
    public Ability AbilityThree;
    public Ability AbilityFour;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void addEffect(Effect e)
    {
        effects.Add(e);
    }

    public void DecrementCooldowns()
    {
        if (AbilityOne.cooldownCurrently > 0)
            AbilityOne.cooldownCurrently--;

        if (AbilityTwo.cooldownCurrently > 0)
            AbilityTwo.cooldownCurrently--;

        if (AbilityThree.cooldownCurrently > 0)
            AbilityThree.cooldownCurrently--;

        if (AbilityFour.cooldownCurrently > 0)
            AbilityFour.cooldownCurrently--;

        foreach (Effect e in effects)
            e.decrementDuration();
    }

    public void resetCooldowns()
    {
        AbilityOne.cooldownCurrently = 0;
        AbilityTwo.cooldownCurrently = 0;
        AbilityThree.cooldownCurrently = 0;
        AbilityFour.cooldownCurrently = 0;
    }
}
