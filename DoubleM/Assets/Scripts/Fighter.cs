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
    public UnityEvent passive;

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
}
