﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static bool isCompleted;
    public bool setIsCompleted;

    // Start is called before the first frame update
    void Start()
    {
        isCompleted = setIsCompleted;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveTutorialState()
    {

    }

}
