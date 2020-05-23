using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveOptions : MonoBehaviour
{
    public Text playText;

    public static bool isGameSaved;
    public static bool isShopActive;
    public static int currentStage;
    // Start is called before the first frame update
    void Start()
    {
        LoadData();

        if (isGameSaved)
            playText.text = "Continue";
        else
            playText.text = "Start game";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadData()
    {
        OptionsData data = SaveSystem.LoadOptions();

        if (data == null)
            return;

        isGameSaved = data.isGameSaved;
        isShopActive = data.isShopActive;
        currentStage = data.currentStage;
        Tutorial.isCompleted = data.isTutorialCompleted;
        if (data.joystickType == 1)
            JoystickManager.type = Type.Fixed;
        else if (data.joystickType == 2)
            JoystickManager.type = Type.Floating;
        JoystickManager.isJoystickChanged = true;
    }
}
