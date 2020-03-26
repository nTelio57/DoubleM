using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter
{
    public GameObject enemyPrefab;
    //public Sprite sprite;
    public Animator animator;
    public bool flipSpriteOnX = false;
    public string name;
    public int maxHP;
    public int attackDamage;
}
