using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

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
        }
        else
        {
            this.GetComponent<CmdConsoleManagement>().enabled = false;//Disable this component if not using the console
        }
    }

    private void Start()
    {
        Debug.Log("<i><b>Current game version: " + Version.version + "</b>\nCurrent console version: " + consoleVersion + "</i>");
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

        //Pull from the history
        if (Input.GetKeyDown(KeyCode.UpArrow) == true && onHistory < inputHistory.Count)
        {
            //If small console is up
            if (GO_cmdConsoleSmall.activeInHierarchy == true && GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true)
            {
                onHistory++;
                GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text = inputHistory[onHistory-1];
            }
            //If large console is up
            else if (GO_cmdConsoleLarge.activeInHierarchy == true && GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true)
            {
                onHistory++;
                GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text = inputHistory[onHistory - 1];
            }
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) == true && onHistory > 0)
        {
            //If small console is up
            if (GO_cmdConsoleSmall.activeInHierarchy == true && GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true)
            {
                onHistory--;
                GO_cmdConsoleSmall.GetComponentInChildren<UnityEngine.UI.InputField>().text = ((onHistory > 0) ? inputHistory[onHistory - 1] : "");
            }
            //If large console is up
            else if (GO_cmdConsoleLarge.activeInHierarchy == true && GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().isFocused == true)
            {
                onHistory--;
                GO_cmdConsoleLarge.GetComponentInChildren<UnityEngine.UI.InputField>().text = ((onHistory > 0) ? inputHistory[onHistory - 1] : "");
            }
        }
    }

    public void getInput(InputField input)
    {
        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            if ((GO_cmdConsoleSmall.activeInHierarchy == true || GO_cmdConsoleLarge.activeInHierarchy == true) && input.text != String.Empty && input.text.Equals("\n") == false)
            {
                commandProcessor.processCommand(input.text);

                if (inputHistory.Count == 0 || input.text != inputHistory[0])
                {
                    inputHistory.Insert(0, input.text);
                }

                input.text = "";
                onHistory = 0;
            }

            if (inputHistory.Count > 32)
            {
                inputHistory.RemoveAt(inputHistory.Count - 1);
            }
        }
    }

    private void closeConsole()
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
        List<String> listToSend = new List<String>();

        if (type == LogType.Error || type == LogType.Assert || type == LogType.Exception)
        {
            smlCmdOutput.Add("\n<color=red>" + logString + "</color>");

            listToSend.Add("\n<color=red>" + logString + "\n");

            if (stackTrace.Equals(String.Empty) == false)
            {
                listToSend[0] += "<i>";
                listToSend.AddRange(Regex.Split(stackTrace, @"([\r\n|\r|\n])"));
                listToSend.RemoveRange(listToSend.Count - 2, 1);
                listToSend[listToSend.Count - 1] = Regex.Replace(listToSend[listToSend.Count - 1], @"\t|\n|\r|`", String.Empty);
                listToSend[listToSend.Count - 1] += "</i>";
            }

            listToSend[listToSend.Count - 1] += "</color>";
        }
        else if(type == LogType.Warning)
        {
            smlCmdOutput.Add("\n<color=yellow>" + logString + "</color>");

            listToSend.Add("\n<color=yellow>" + logString);

            if (stackTrace.Equals(String.Empty) == false)
            {
                listToSend.Add("\n<i>" + stackTrace.Substring(stackTrace.IndexOf("\n", stackTrace.IndexOf(")\n")) + 1,
                    stackTrace.IndexOf(")\n", stackTrace.IndexOf("\n", stackTrace.IndexOf("\n", stackTrace.IndexOf("\n")))) - stackTrace.IndexOf("\n", stackTrace.IndexOf("\n"))) + "</i>");
            }

            listToSend[listToSend.Count - 1] += "</color>";
        }
        else
        {
            smlCmdOutput.Add("\n" + logString);
            listToSend.Add("\n" + logString);
        }

        cmdConsoleLarge.recieveNewLogs(listToSend);
    }

    public void trimSmallLog()
    {
        smlCmdOutput.RemoveRange(0, 8);
    }

    public List<String> getSmlCmdOutput()
    {
        return smlCmdOutput;
    }
}
