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
    private CmdConsoleLarge cmdConsoleLarge;
    private GameObject GO_cmdConsoleLarge;
    private CommandProcessor commandProcessor;

    public bool consoleUse = true;
    public String consoleVersion = "";

    private List<String> smlCmdOutput = new List<String>();
    private List<String> lrgCmdOutput = new List<String>();

    private List<String> inputHistory = new List<String>();
    private byte onHistory = 0;

    //Called when script is initiailzed
    private void Awake()
    {
        if (consoleUse == true)
        {
            GO_cmdConsoleSmall = GameObject.Find("CommandConsole/CmdConsole_sml");
            cmdConsoleSmall = GO_cmdConsoleSmall.GetComponent<CmdConsoleSmall>();
            GO_cmdConsoleLarge = GameObject.Find("CommandConsole/CmdConsole_lrg");
            cmdConsoleLarge = GO_cmdConsoleLarge.GetComponent<CmdConsoleLarge>();
            commandProcessor = this.GetComponent<CommandProcessor>();
            Application.logMessageReceived += HandleLog;
            Debug.Log("<i><b>Current game version: " + Version.version + "</b>\nCurrent console version: " + consoleVersion + "</i>");
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
                this.GetComponent<Canvas>().worldCamera = Camera.main;
                this.GetComponent<Canvas>().enabled = true;
                GO_cmdConsoleSmall.SetActive(false);
                GO_cmdConsoleLarge.SetActive(true);
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
            GO_cmdConsoleLarge.SetActive(false);
            GO_cmdConsoleSmall.SetActive(true);
            cmdConsoleSmall.openConsole();
        }
        else if (Input.GetKeyDown(Bindings.Console) == true && this.GetComponent<Canvas>().enabled == true)//Close the console
        {
            closeConsole();   
        }

        //Get command
        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            //If small console is up
            if (GO_cmdConsoleSmall.activeInHierarchy == true && GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true 
                && GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text != String.Empty && GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text != "\n")
            {
                commandProcessor.processCommand(GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Substring(0, GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Length-1));

                if (inputHistory.Count == 0 
                    || GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Substring(0, GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Length - 1) != inputHistory[0])
                {
                    inputHistory.Insert(0, GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Substring(0, GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text.Length - 1));
                }

                GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text = "";
            }
            //If large console is up
            else  if (GO_cmdConsoleLarge.activeInHierarchy == true && GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true
                && GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text != String.Empty && GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text != "\n")
            {
                commandProcessor.processCommand(GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text.Substring(0, GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text.Length - 1));

                if (inputHistory.Count == 0
                    || GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text.Substring(0, GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text.Length - 1) != inputHistory[0])
                {
                    inputHistory.Insert(0, GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text.Substring(0, GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text.Length - 1));
                }

                GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text = "";
            }

            if (inputHistory.Count > 32)
            {
                inputHistory.RemoveAt(inputHistory.Count-1);
            }
        }

        //Pull from the history
        if (Input.GetKeyDown(KeyCode.UpArrow) == true && onHistory < inputHistory.Count)
        {
            onHistory++;
            //If small console is up
            if (GO_cmdConsoleSmall.activeInHierarchy == true && GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true)
            {
                GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text = inputHistory[onHistory-1];
            }
            //If large console is up
            else if (GO_cmdConsoleLarge.activeInHierarchy == true && GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true)
            {
                GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text = inputHistory[onHistory - 1];
            }
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) == true && onHistory > 0)
        {
            onHistory--;
            //If small console is up
            if (GO_cmdConsoleSmall.activeInHierarchy == true && GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true)
            {
                GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text = ((onHistory > 0) ? inputHistory[onHistory - 1] : "");
            }
            //If large console is up
            else if (GO_cmdConsoleLarge.activeInHierarchy == true && GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true)
            {
                GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text = ((onHistory > 0) ? inputHistory[onHistory - 1] : "");
            }
        }
    }

    public void closeConsole()
    {
        if (GO_cmdConsoleSmall.activeInHierarchy == true)//Small console
        {
            cmdConsoleSmall.closeConsole();
        }
        else if (GameObject.Find("CommandConsole/CmdConsole_lrg").activeInHierarchy == true)//Large console
        {
            cmdConsoleLarge.closeConsole();
        }
    }
    
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Assert || type == LogType.Exception)
        {
            smlCmdOutput.Add("\n<color=red>" + logString + "</color>");
            lrgCmdOutput.Add("\n<i><color=red>" + logString + "\n" + stackTrace + "</color></i>");
        }
        else if(type == LogType.Warning)
        {
            smlCmdOutput.Add("\n<color=yellow>" + logString + "</color>");
            lrgCmdOutput.Add("\n<color=yellow>" + logString + "\n<i>" + stackTrace.Substring(stackTrace.IndexOf("\n", stackTrace.IndexOf(")\n")) + 1,
                stackTrace.IndexOf(")\n", stackTrace.IndexOf("\n", stackTrace.IndexOf("\n", stackTrace.IndexOf("\n")))) - stackTrace.IndexOf("\n", stackTrace.IndexOf("\n"))) + "</i></color>");
        }
        else
        {
            smlCmdOutput.Add("\n" + logString);
            lrgCmdOutput.Add("\n" + logString);
        }
    }

    public void trimSmallLog()
    {
        smlCmdOutput.RemoveRange(0, 8);
    }

    public void trimLargeLogs()
    {
        
        lrgCmdOutput.RemoveRange(0, 16);
    }

    public List<String> getSmlCmdOutput()
    {
        return smlCmdOutput;
    }

    public List<String> getLrgCmdOutput()
    {
        return lrgCmdOutput;
    }
}
