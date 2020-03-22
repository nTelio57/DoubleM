using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    static float JoystickLeftRightValue, JoystickFloatFixValue;
    public Scrollbar JoystickPosition, JoystickType;
    // Start is called before the first frame update
    void Start()
    {
        JoystickPosition.value = JoystickLeftRightValue;
        JoystickType.value = JoystickFloatFixValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onLeaveClick()
    {
        SceneManager.UnloadSceneAsync("Options");
    }

    public void onJoystickPositionChange(Scrollbar s)
    {
        if (s.value < 0.5)
        {
            JoystickManager.side = Side.Left;

        }
        else
        {
            JoystickManager.side = Side.Right;
        }

        JoystickLeftRightValue = s.value;

        JoystickManager.setJoystickActive(false);
        JoystickManager.reloadJoystick();
        JoystickManager.setJoystickActive(true);
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

        JoystickManager.setJoystickActive(false);
        JoystickManager.reloadJoystick();
        JoystickManager.setJoystickActive(true);
    }
}
