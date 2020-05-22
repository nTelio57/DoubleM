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
        JoystickType.value = JoystickFloatFixValue;
        TutorialBool.isOn = !Tutorial.isCompleted;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onLeaveClick()
    {
        SceneManager.UnloadSceneAsync("Options");
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

        /*if (s.value < 0.5)
        {
            JoystickManager.type = Type.Fixed;
        }
        else
        {
            JoystickManager.type = Type.Floating;
        }

        JoystickFloatFixValue = s.value;

        JoystickManager.setJoystickActive(false);
        JoystickManager.reloadJoystick();
        JoystickManager.setJoystickActive(true);*/
    }

    public void onTutorialToggleChange(Toggle t)
    {
        Tutorial.isCompleted = !t.isOn;
    }
}
