using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public float[] position;
    public float currentHealth;
    public float maxHealth;
    public float currentStamina;
    public float maxStamina;

    public PlayerData(Combat player, Movement playerMovement)
    {
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        currentHealth = player.currentHealth;
        maxHealth = player.maxHealth;
        currentStamina = playerMovement.stamina;
        maxStamina = playerMovement.maxStamina;
    }
}
