using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    public GameObject attackButton;
    public GameObject healthBar;
    public GameObject staminaBar;
    

    // Start is called before the first frame update
    void Start()
    {
        if (!Tutorial.isCompleted)
        {
            attackButton.SetActive(false);
            healthBar.SetActive(false);
            staminaBar.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onMainCombatTriger()
    {
        attackButton.SetActive(true);
        healthBar.SetActive(true);
        staminaBar.SetActive(true);
    }
}
