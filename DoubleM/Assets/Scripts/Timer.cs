﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float TimeToWaitInSeconds;
    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TimeToWaitInSeconds)
            gameObject.SetActive(false);
    }

    public void setTimer(float seconds)
    {
        TimeToWaitInSeconds = seconds;
        gameObject.SetActive(true);
        timer = 0;
    }
}
