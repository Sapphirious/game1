using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CmdConsoleManagement : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) == true)
        {
            if (Input.GetKeyDown(Bindings.Console) == true)
            {
                Debug.Log("Shift tilded");
            }
        }
        else if (Input.GetKeyDown(Bindings.Console) == true)
        {
            Debug.Log("tilded");
        }
    }
    
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output.Append(logString);
        stack.Append(stackTrace);
        typeStack.Add(type);
    }
}
