using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class CmdConsoleManagement : MonoBehaviour
{
    private CmdConsoleSmall cmdConsoleSmall;
    private GameObject GO_cmdConsoleSmall;
    private CommandProcessor commandProcessor;

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
            commandProcessor = this.GetComponent<CommandProcessor>();
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
            if (GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Length > 0)
            {
                GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text = GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Substring(0, GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Length - 1);
            }

            closeConsole();   
        }

        //Get command
        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            //If small console is up
            if (GO_cmdConsoleSmall.activeInHierarchy == true && GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true 
                && GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text != "")
            {
                commandProcessor.processCommand(GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Substring(0, GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Length-1));
                GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text = "";
            }
        }

        GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text = Regex.Replace(GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text, @"\t|\n|\r", "");
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
        typeStack.Add(type);

        if (type == LogType.Error || type == LogType.Assert || type == LogType.Exception)
        {
            output.Append("\n<color=red>" + logString + "</color>");
            stack.Append("\n<i><color=red>" + stackTrace + "</color></i>");
        }
        else if(type == LogType.Warning)
        {
            output.Append("\n<color=yellow>" + logString + "</color>");
            stack.Append("\n<i><color=yellow>" + stackTrace + "</color></i>");
        }
        else
        {
            output.Append("\n" + logString);
            stack.Append("\n<i>" + stackTrace + "</i>");
        }
    }
}
