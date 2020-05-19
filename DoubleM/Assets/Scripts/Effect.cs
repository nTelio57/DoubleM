using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public static Effect Untargetable = new Effect("Untargetable", 1, 0, true);
    public static Effect Bleeding = new Effect("Bleeding", 4, 3, false);
    public static Effect Rage = new Effect("Rage", 2, 0, false);
    public static Effect Weakness = new Effect("Weakness", 1, 0, false);

    public string name;
    public int duration;
    int totalDurationInTurns;
    public bool isActive;
    public float amount;

    public Effect(string name, int duration, float amount, bool isActive)
    {
        this.name = name;
        totalDurationInTurns = duration;
        this.isActive = isActive;
        this.amount = amount;
        this.duration = totalDurationInTurns;
    }

    public Effect(string name)
    {
        this.name = name;
        totalDurationInTurns = 1;
        isActive = true;
        amount = 1;
        duration = totalDurationInTurns;
    }

    public void setDuration(int duration)
    {
        this.duration = duration;
        if (this.duration > 0)
            isActive = true;
    }

    public void decrementDuration()
    {
        if (isActive)
            duration--;
    }

    public bool Equals(Effect e)
    {
        return name.Equals(e.name);
    }
}
