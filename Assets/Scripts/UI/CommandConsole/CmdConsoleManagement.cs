using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CmdConsoleManagement : MonoBehaviour
{
    private CmdConsoleSmall cmdConsoleSmall;
    private GameObject GO_cmdConsoleSmall;

    public bool consoleUse = true;

    public StringBuilder output = new StringBuilder("");
    public StringBuilder stack = new StringBuilder("");
    public ArrayList typeStack = new ArrayList();

    //Called when script is initiailzed
    private void Awake()
    {
        if (consoleUse == true)
        {
            GO_cmdConsoleSmall = GameObject.Find("CommandConsole/CmdConsole_sml");
            cmdConsoleSmall = GO_cmdConsoleSmall.GetComponent<CmdConsoleSmall>();
            Application.logMessageReceived += HandleLog;
        }
        else
        {
            this.GetComponent<CmdConsoleManagement>().enabled = false;//Disable this component if not using the console
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) == true)//Shift is pressed
        {
            if (Input.GetKeyDown(Bindings.Console) == true && this.GetComponent<Canvas>().enabled == false)//Open big the console
            {
                Debug.Log("Opening the big console...");
                GameObject.Find("CommandConsole").GetComponent<Canvas>().worldCamera = Camera.main;
                GameObject.Find("CommandConsole").GetComponent<Canvas>().enabled = true;
            }
            else if (Input.GetKeyDown(Bindings.Console) == true && this.GetComponent<Canvas>().enabled == true)//Close the console
            {
                closeConsole();
            }
        }
        //Shift is not pressed
        else if (Input.GetKeyDown(Bindings.Console) == true && this.GetComponent<Canvas>().enabled == false)//Open the small console
        {
            GameObject.Find("CommandConsole").GetComponent<Canvas>().worldCamera = Camera.main;
            GameObject.Find("CommandConsole").GetComponent<Canvas>().enabled = true;
            GO_cmdConsoleSmall.SetActive(true);
            cmdConsoleSmall.openConsole();
        }
        else if (Input.GetKeyDown(Bindings.Console) == true && this.GetComponent<Canvas>().enabled == true)//Close the console
        {
            closeConsole();   
        }
    }

    public void closeConsole()
    {
        Debug.Log("Closing the console...");

        if (GO_cmdConsoleSmall.activeInHierarchy == true)//Small console
        {
            cmdConsoleSmall.closeConsole();
        }
        else if (GameObject.Find("CommandConsole/CmdConsole_lrg").activeInHierarchy == true)//Large console
        {
            //TODO close big console
        }
    }
    
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output.Append(logString);
        stack.Append(stackTrace);
        typeStack.Add(type);
    }
}
