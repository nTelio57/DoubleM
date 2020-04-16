using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public static Effect Untargetable = new Effect("Untargetable", 1, true);

    public string name;
    public int duration;
    int totalDurationInTurns;
    public bool isActive;

    public Effect(string name, int duration, bool isActive)
    {
        this.name = name;
        totalDurationInTurns = duration;
        this.isActive = isActive;
        duration = totalDurationInTurns;
    }

    public Effect(string name)
    {
        this.name = name;
        totalDurationInTurns = 1;
        isActive = true;
        duration = totalDurationInTurns;
    }

    public void setDuration(int duration)
    {
        totalDurationInTurns = duration;
    }

    public void decrementDuration()
    {
        duration--;
    }

    public bool Equals(Effect e)
    {
        return name.Equals(e.name);
    }
}
