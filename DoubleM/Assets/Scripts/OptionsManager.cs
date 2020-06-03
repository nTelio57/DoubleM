using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    static float JoystickFloatFixValue;
    public Scrollbar  JoystickType;
    public Toggle TutorialBool;
    
    // Start is called before the first frame update
    void Start()
    {
        //JoystickType.value = JoystickFloatFixValue;
        loadData();
        //TutorialBool.isOn = !Tutorial.isCompleted;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onLeaveClick()
    {
        SceneManager.UnloadSceneAsync("Options");
    }

    void loadData()
    {
        OptionsData data = SaveSystem.LoadOptions();

        TutorialBool.isOn = !data.isTutorialCompleted;
        JoystickType.value = data.joystickType - 1;
    }

    public void onJoystickTypeChange(Scrollbar s)
    {
        if (s.value < 0.5)
        {
            JoystickManager.type = Type.Fixed;
        }
        else
        {
            JoystickManager.type = Type.Floating;

        }

        JoystickFloatFixValue = s.value;
        JoystickManager.isJoystickChanged = true;

        SaveSystem.SaveOptions();
    }

    public void onTutorialToggleChange(Toggle t)
    {
        Tutorial.isCompleted = !t.isOn;
        SaveSystem.SaveOptions();
    }

    public void onResetClick()
    {
        SaveOptions.isGameSaved = false;
        SaveSystem.SaveOptions();
        GameTracking.Reset();
        Shop.Reset();
        MainMenu.DestroyStaticObjects();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
