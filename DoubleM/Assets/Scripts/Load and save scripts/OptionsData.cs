using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsData
{
    public bool isTutorialCompleted;
    public int joystickType; // 1 - fixed; 2 - floating;
    public bool isGameSaved;
    public bool isShopActive;
    public int currentStage;

    public OptionsData()
    {
        isGameSaved = SaveOptions.isGameSaved;
        isTutorialCompleted = Tutorial.isCompleted;
        if (JoystickManager.type == Type.Fixed)
            joystickType = 1;
        else if (JoystickManager.type == Type.Floating)
            joystickType = 2;
        isShopActive = SaveOptions.isShopActive;
        currentStage = SaveOptions.currentStage;
    }

}
