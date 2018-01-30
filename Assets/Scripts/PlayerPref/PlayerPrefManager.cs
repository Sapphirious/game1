using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefManager : MonoBehaviour
{
    // Use this for initialization
    void Awake()
    {
        //If firstTimeCheck is 0, then it is our first run
        if (PlayerPrefs.GetInt("firstTimeCheck") == 0)
        {
            firstTimeSetup();
        }
        else
        {
            normalSetup();
        }
    }

    private void firstTimeSetup()
    {
        Bindings.setDefaults();
        PlayerPrefs.SetInt("firstTimeCheck", 0);//TODO will no longer flag the first time setup
    }

    private void normalSetup()
    {
        Bindings.initializeKeys();
    }
}
