using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public bool isLooped;
    private bool used = false;
    public string name;

    [TextArea(3,10)]
    public string[] sentences;

    public bool isUsed()
    {
        return used;
    }

    public void setUsed(bool isUsed)
    {
        used = isUsed;
    }
}
