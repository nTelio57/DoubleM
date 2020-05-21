using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTBC : MonoBehaviour
{
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        if (!Tutorial.isCompleted)
        {
            panel.SetActive(true);
        }
    }

    public void unload()
    {
        panel.SetActive(false);
        Tutorial.isCompleted = true;
    }

    
}
