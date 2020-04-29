using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRewards : MonoBehaviour
{
    public GameObject rendererObject;
    public Sprite Health;
    public Sprite Armor;
    public Sprite Attack;
    public Sprite Money;
    public Sprite Shield;

    // Start is called before the first frame update
    void Start()
    {
        
        if (rendererObject != null)
        {
            SpriteRenderer renderer = rendererObject.GetComponent<SpriteRenderer>();
            BattleReward reward = GetComponent<CapturePoint>().reward;
            if (reward == BattleReward.Attack)
                renderer.sprite = Attack;
            if (reward == BattleReward.DodgeChance)
                renderer.sprite = Shield;
            if (reward == BattleReward.Health)
                renderer.sprite = Health;
            if (reward == BattleReward.Armor)
                renderer.sprite = Armor;
            if (reward == BattleReward.Money)
                renderer.sprite = Money;
            if (reward == BattleReward.None)
                renderer.sprite = null;
        }
        
    }
    
}
