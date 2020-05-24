using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VaultData 
{
    public int money;
    public int chances;

    public VaultData()
    {
        money = Vault.money;
        chances = Vault.chances;
    }
}
