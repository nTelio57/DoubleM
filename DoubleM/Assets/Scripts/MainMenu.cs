﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        JoystickManager.init();
    }

    public void OnPlayClick()
    {
        SceneManager.LoadScene("Stage1", LoadSceneMode.Single);
    }

    public void OnOptionsClick()
    {
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
    }
}
