using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentJoystick { FixedLeft, FixedRight, FloatingLeft, FloatingRight }
public enum Side { Left, Right}
public enum Type { Fixed, Floating}

public class JoystickManager : MonoBehaviour
{
    public static Joystick fixedLeft, fixedRight, floatingLeft, floatingRight, currentJoystick;
    public Joystick fixedLeftJoystick, fixedRightJoystick, floatingLeftJoystick, floatingRightJoystick;
    public static Side side;
    public static Type type;
    static bool loaded = false;

    // Start is called before the first frame update
    void Start()
    {
        loaded = true;
        fixedLeft = fixedLeftJoystick;
        fixedRight = fixedRightJoystick;
        floatingLeft = floatingLeftJoystick;
        floatingRight = floatingRightJoystick;
        reloadJoystick();
        setJoystickActive(true);
    }

    public static void init()
    {
        side = Side.Left;
        type = Type.Fixed;
    }

    public static Side getSide()
    {
        return side;
    }

    public static Type getType()
    {
        return type;
    }
    

    public static void reloadJoystick()
    {

        if (side == Side.Left && type == Type.Fixed)
            currentJoystick = fixedLeft;
        if (side == Side.Left && type == Type.Floating)
            currentJoystick = floatingLeft;
        if (side == Side.Right && type == Type.Fixed)
            currentJoystick = fixedRight;
        if (side == Side.Right && type == Type.Floating)
            currentJoystick = floatingRight;
        
    }

    public static void setJoystickActive(bool value)
    {
        if(loaded)
            currentJoystick.gameObject.SetActive(value);
    }
    
}
