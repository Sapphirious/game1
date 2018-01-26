using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CmdConsoleManagement : MonoBehaviour
{
    private CmdConsoleSmall smallConsole;

    public bool consoleUse = true;
    private bool consoleUp = false;

    public StringBuilder output = new StringBuilder("");
    public StringBuilder stack = new StringBuilder("");
    public ArrayList typeStack = new ArrayList();

    //Called when script is initiailzed
    private void Awake()
    {
        if (consoleUse == true)
        {
            Application.logMessageReceived += HandleLog;
        }
        else
        {
            GameObject.Find("Scripts").GetComponent<CmdConsoleManagement>().enabled = false;//Disable this component if not using the console
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) == true)
        {
            if (Input.GetKeyDown(Bindings.Console) == true && consoleUp == false)//Open small the console
            {
                GameObject.Find("CommandConsole").GetComponent<Canvas>().worldCamera = Camera.current;
                
            }
            else if (Input.GetKeyDown(Bindings.Console) == true && consoleUp == true)//Close the console
            {

            }
        }
        else if (Input.GetKeyDown(Bindings.Console) == true && consoleUp == false)//Open the big console
        {
            GameObject.Find("CommandConsole").GetComponent<Canvas>().worldCamera = Camera.current;
            Debug.Log("tilded");
        }
        else if (Input.GetKeyDown(Bindings.Console) == true && consoleUp == true)//Close the console
        {

        }
    }
    
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output.Append(logString);
        stack.Append(stackTrace);
        typeStack.Add(type);
    }
}
