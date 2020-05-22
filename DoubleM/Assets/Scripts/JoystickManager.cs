using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Type { Fixed, Floating}

public class JoystickManager : MonoBehaviour
{
    public static Type type;
    public VariableJoystick joystick;
    public static bool isJoystickChanged;

    // Start is called before the first frame update
    void Start()
    {
        isJoystickChanged = true;
        updateJoystick();
    }

    void Update()
    {
        if (isJoystickChanged)
            updateJoystick();
    }

    void updateJoystick()
    {
        if (isJoystickChanged)
        {
            if (type == Type.Fixed)
                joystick.SetMode(JoystickType.Fixed);
            else if (type == Type.Floating)
                joystick.SetMode(JoystickType.Floating);
        }
        isJoystickChanged = false;
    }

}
