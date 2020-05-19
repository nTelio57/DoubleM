using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentJoystick { FixedLeft, FixedRight, FloatingLeft, FloatingRight }
public enum Side { Left, Right}
public enum Type { Fixed, Floating}

public class JoystickManager : MonoBehaviour
{
    public static Joystick fixedLeft, floatingLeft, currentJoystick;
    public Joystick fixedLeftJoystick,  floatingLeftJoystick;
    public static Type type;
    static bool loaded = false;

    // Start is called before the first frame update
    void Start()
    {
        loaded = true;
        fixedLeft = fixedLeftJoystick;
        floatingLeft = floatingLeftJoystick;
        reloadJoystick();
        setJoystickActive(true);
    }

    public static void init()
    {
        type = Type.Fixed;
    }

    public static Type getType()
    {
        return type;
    }
    

    public static void reloadJoystick()
    {

        if ( type == Type.Fixed)
            currentJoystick = fixedLeft;
        if (type == Type.Floating)
            currentJoystick = floatingLeft;
        
    }

    public static void setJoystickActive(bool value)
    {
        if(loaded)
            currentJoystick.gameObject.SetActive(value);
    }
    
}
