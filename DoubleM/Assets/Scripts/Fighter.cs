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
    [HideInInspector]
    public int ID;
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

    [HideInInspector]
    public Healthbar healthbar;

    public Fighter(Fighter f)
    {
        prefab = f.prefab;
        animator = f.animator;
        flipSpriteOnX = f.flipSpriteOnX;
        ID = f.ID;
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

        //Effects
        addEffect(Effect.Untargetable);

        disableAllEffects();

        prefab.GetComponentInChildren<Healthbar>().SetMaxHealth(maxHP);
    }

    public void disableAllEffects()
    {
        foreach (Effect e in effects)
        {
            e.isActive = false;
            e.duration = 0;
        }
    }

    public void setID(int id)
    {
        ID = id;
    }

    public void setBattleStation(int index)
    {
        battleStationIndex = index;
    }

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        healthbar.SetHealth(currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;
            return true;
        }
        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
        healthbar.SetHealth(currentHP);//meta klaida kai upgradinama hp shope
    }

    public void Upgrade(string type, int amount)
    {
        if (type == "attack")
        {
            attackDamage += amount;
        }
        if(type == "health")
        {
            maxHP += amount;
            Heal(amount);
        }
    }

    public void addEffect(Effect e)
    {
        bool found = false;
        foreach (Effect effect in effects)
        {
            if (effect.name == e.name) {
                effect.duration = e.duration;
                effect.isActive = e.isActive;
                found = true;
            }
        }
        if (!found)
            effects.Add(new Effect(e.name, e.duration, e.amount, e.isActive));
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
                {
                    e.isActive = false;
                }

            }
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }
        
            
    }

    void activateEffects()
    {
        foreach (Effect e in effects)
        {
            if (e.duration > 0)
                e.isActive = true;
            else
                e.isActive = false;
        }
    }

    public Effect GetEffect(string name)
    {
        foreach (Effect e in effects)
        {
            if (e.name == name)
                return e;
        }
        return null;
    }

    public bool HasEffect(string name)
    {
        foreach (Effect e in effects)
        {
            if (e.name == name)
                return true;
        }
        return false;
    }

    public void resetCooldowns()
    {
        AbilityOne.cooldownCurrently = 0;
        AbilityTwo.cooldownCurrently = 0;
        AbilityThree.cooldownCurrently = 0;
        AbilityFour.cooldownCurrently = 0;
    }

    float tempWeaknessAd, tempWeaknessCrit;
    float tempRageAd, tempRageCrit;
    public void StartOfTurn()
    {
        DecrementCooldowns();
        activateEffects();

        if (HasEffect(Effect.Weakness.name) && GetEffect(Effect.Weakness.name).isActive)
        {
            tempWeaknessAd = attackDamage;
            tempWeaknessCrit = critChance;
            attackDamage = (int)(attackDamage * 0.4);
            critChance = 0;
        }

        if (HasEffect(Effect.Rage.name) && GetEffect(Effect.Rage.name).isActive)
        {
            tempRageAd = attackDamage;
            tempRageCrit = critChance;
            attackDamage = (int)(attackDamage * 1.2);
            critChance = 100;
        }
    }

    //Vykdomas bet kokio veiksmo pabaigoj, nesvarbu ar tai sis fighteris atlikinejo veiksmus ar kazkas kitas. BattleSystem klaseje po kiekvieno fighterio ejimo for ciklas praeis pro visus fighterius ir isauks si metoda
    public void EndOfTurn()
    {
        if (HasEffect(Effect.Bleeding.name) && GetEffect(Effect.Bleeding.name).isActive)
        {
            TakeDamage((int)GetEffect(Effect.Bleeding.name).amount);
        }

        if (HasEffect(Effect.Weakness.name) && GetEffect(Effect.Weakness.name).isActive)
        {
            attackDamage = (int)tempWeaknessAd;
            critChance = tempWeaknessCrit;
        }

        if (HasEffect(Effect.Rage.name) && GetEffect(Effect.Rage.name).isActive)
        {
            attackDamage = (int)tempRageAd;
            critChance = tempRageCrit;
        }
        
    }
}
