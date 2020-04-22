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
    [HideInInspector]
    public int battleStationIndex;
    public List<Effect> effects;
    public Ability AbilityOne;
    public Ability AbilityTwo;
    public Ability AbilityThree;
    public Ability AbilityFour;

    public Fighter(Fighter f)
    {
        prefab = f.prefab;
        animator = f.animator;
        flipSpriteOnX = f.flipSpriteOnX;
        name = f.name;
        maxHP = f.maxHP;
        currentHP = f.currentHP;
        attackDamage = f.attackDamage;
        critChance = f.critChance;
        isTargetable = f.isTargetable;
        effects = new List<Effect>();
        AbilityOne = new Ability(f.AbilityOne);
        AbilityTwo = new Ability(f.AbilityTwo);
        AbilityThree = new Ability(f.AbilityThree);
        AbilityFour = new Ability(f.AbilityFour);
    }

    public void setBattleStation(int index)
    {
        battleStationIndex = index;
    }

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
        

        try
        {
            foreach (Effect e in effects)
            {
                e.decrementDuration();
                if (e.duration <= 0)
                    effects.Remove(e);
            }
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }
        
            
    }

    public void resetCooldowns()
    {
        AbilityOne.cooldownCurrently = 0;
        AbilityTwo.cooldownCurrently = 0;
        AbilityThree.cooldownCurrently = 0;
        AbilityFour.cooldownCurrently = 0;
    }
}
