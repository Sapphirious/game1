using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bindings : MonoBehaviour
{
    private static int controllerType = 0;//0 = keyboard, 1 = XBOX, 2 = DS4
    //Movement
    private static KeyCode[] forward = new KeyCode[3];
    private static KeyCode[] backward = new KeyCode[3];
    private static KeyCode[] left = new KeyCode[3];
    private static KeyCode[] right = new KeyCode[3];
    //Menu
    private static KeyCode[] select = new KeyCode[3];
    private static KeyCode[] back = new KeyCode[3];
    //Misc/debug
    private static readonly KeyCode console = KeyCode.BackQuote;
    private static readonly KeyCode toggleFPS = KeyCode.F9;

    public static void setDefaults()
    {
        PlayerPrefs.SetInt("controllerType", 0);
        PlayerPrefs.SetInt("forward_1", (int)(KeyCode.W));
        PlayerPrefs.SetInt("forward_2", (int)(KeyCode.UpArrow));
        PlayerPrefs.SetInt("backward_1", (int)(KeyCode.S));
        PlayerPrefs.SetInt("backward_2", (int)(KeyCode.DownArrow));
        PlayerPrefs.SetInt("left_1", (int)(KeyCode.A));
        PlayerPrefs.SetInt("left_2", (int)(KeyCode.LeftArrow));
        PlayerPrefs.SetInt("right_1", (int)(KeyCode.D));
        PlayerPrefs.SetInt("right_2", (int)(KeyCode.RightArrow));
        PlayerPrefs.SetInt("select_1", (int)(KeyCode.Mouse0));
        PlayerPrefs.SetInt("select_2", (int)(KeyCode.Return));
        PlayerPrefs.SetInt("back_1", (int)(KeyCode.Escape));
        PlayerPrefs.SetInt("back_2", (int)(KeyCode.Backspace));
        initializeKeys();
    }

    public static void initializeKeys()
    {
        controllerType = PlayerPrefs.GetInt("controllerType");
        forward[0] = (KeyCode)(PlayerPrefs.GetInt("forward_1"));
        forward[1] = (KeyCode)(PlayerPrefs.GetInt("forward_2"));
        forward[2] = (KeyCode)(PlayerPrefs.GetInt("forward_3"));
        backward[0] = (KeyCode)(PlayerPrefs.GetInt("backward_1"));
        backward[1] = (KeyCode)(PlayerPrefs.GetInt("backward_2"));
        backward[2] = (KeyCode)(PlayerPrefs.GetInt("backward_3"));
        left[0] = (KeyCode)(PlayerPrefs.GetInt("left_1"));
        left[1] = (KeyCode)(PlayerPrefs.GetInt("left_2"));
        left[2] = (KeyCode)(PlayerPrefs.GetInt("left_3"));
        right[0] = (KeyCode)(PlayerPrefs.GetInt("right_1"));
        right[1] = (KeyCode)(PlayerPrefs.GetInt("right_2"));
        right[2] = (KeyCode)(PlayerPrefs.GetInt("right_3"));
        select[0] = (KeyCode)(PlayerPrefs.GetInt("select_1"));
        select[1] = (KeyCode)(PlayerPrefs.GetInt("select_2"));
        select[2] = (KeyCode)(PlayerPrefs.GetInt("select_3"));
        back[0] = (KeyCode)(PlayerPrefs.GetInt("back_1"));
        back[1] = (KeyCode)(PlayerPrefs.GetInt("back_2"));
        back[2] = (KeyCode)(PlayerPrefs.GetInt("back_3"));
    }

    //Getters and setters
    public static int ControllerType
    {
        get
        {
            return controllerType;
        }

        set
        {
            controllerType = value;
        }
    }

    public static KeyCode[] Forward
    {
        get
        {
            return forward;
        }

        set
        {
            forward = value;
        }
    }

    public static KeyCode[] Backward
    {
        get
        {
            return backward;
        }

        set
        {
            backward = value;
        }
    }

    public static KeyCode[] Left
    {
        get
        {
            return left;
        }

        set
        {
            left = value;
        }
    }

    public static KeyCode[] Right
    {
        get
        {
            return right;
        }

        set
        {
            right = value;
        }
    }

    public static KeyCode[] Select
    {
        get
        {
            return select;
        }

        set
        {
            select = value;
        }
    }

    public static KeyCode[] Back
    {
        get
        {
            return back;
        }

        set
        {
            back = value;
        }
    }

    public static KeyCode Console
    {
        get
        {
            return console;
        }
    }

    public static KeyCode ToggleFPS
    {
        get
        {
            return toggleFPS;
        }
    }
}
