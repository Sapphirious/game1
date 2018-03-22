using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{
    private List<String> commandComponents = new List<String>();
    private readonly string[] validCommands = { "echo", "help", "prof" };

    public void processCommand(string command)
    {
        commandComponents.Clear();
        int point = 0;

        for (int i = 0; i < command.Length; i++)
        {
            if (command.Substring(i, 1).Equals(" "))
            {
                commandComponents.Add(command.Substring(point, i - point));

                for (; i < command.Length; i++)
                {
                    if (command.Substring(i, 1).Equals(" ") == false)
                    {
                        break;
                    }
                }

                point = i;
            }
        }

        if (command.Substring(point, command.Length - point).Replace(" ", String.Empty).Length > 0)
        {
            commandComponents.Add(command.Substring(point, command.Length - point));
        }

        //Make the root command lowercase
        commandComponents[0] = commandComponents[0].ToLower();

        bool validCommand = false;

        foreach(string s in validCommands)
        {
            if (commandComponents[0].Equals(s))
            {
                validCommand = true;
                break;
            }
        }

        if (validCommand == false)
        {
            invalidCommand();
            return;
        }

        //Check the root word
        switch (commandComponents[0].ToString())
        {
            case "echo":
                echo(command);
                break;
            case "help":
                help();
                break;
            case "prof":
                profiler();
                break;
            default:
                Debug.LogError("Made it to the default case when processing the last command.");
                break;
        }
    }

    private void invalidCommand()
    {
        Debug.LogWarning("The base command <i>" + commandComponents[0] + "</i> is not a valid command. Type \"help\" for a list of commands.");
    }

    private void echo(String command)
    {
        int repeats = 0;

        if (commandComponents.Count == 1)
        {
            Debug.LogWarning("Missing second argument in structure for echo: echo {number of echos|\"err\"|message} [message]");
        }
        else if (commandComponents.Count > 2 && Int32.TryParse(commandComponents[1].ToString(), out repeats) == true)
        {
            if (repeats <= 256)
            {
                Debug.Log("Echoing <i>" + command.Substring(6 + (int)(Math.Floor(Math.Log10(repeats) + 1)), command.Length - 6 - (int)(Math.Floor(Math.Log10(repeats) + 1))) + "</i> " + repeats
                    + ((repeats == 1) ? " time..." : " times..."));

                System.Object[] args = new System.Object[2];
                args[0] = command;
                args[1] = repeats;

                StartCoroutine("multiEcho", args);
            }
            else
            {
                Debug.LogWarning("You may not echo a message more than 256 times per message.");
            }
        }
        else if (commandComponents.Count > 2 && commandComponents[1].ToLower() == "err")
        {
            Debug.LogError("@echo: <i>" + command.Substring(command.IndexOf("err") + 3, command.Length - command.IndexOf("err") - 3) + "</i>");//TODO fix this
        }
        else
        {
            Debug.Log("@echo: <i>" + command.Substring(5, command.Length - 5) + "</i>");
        }
    }

    IEnumerator multiEcho(System.Object[] args)
    {
        String command = (String) args[0];
        int repeats = (int) args[1];

        for (int i = 0; i < repeats; i++)
        {
            Debug.Log("@echo: <i>" + command.Substring(6 + (int)(Math.Floor(Math.Log10(repeats) + 1)), command.Length - 6 - (int)(Math.Floor(Math.Log10(repeats) + 1))) + "</i>");

            if (i % 6 == 0 || i+1 == repeats)
            { 
                yield return null;
            }
        }
    }

    private void profiler()
    {
        String profStruct = "prof {\"off\"/0|\"fps\"/1}";

        //Make the subroot lowercase
        if (commandComponents.Count > 1)
        {
            commandComponents[1] = commandComponents[1].ToLower();
        }

        if (commandComponents.Count == 1)
        {
            //Debug.LogWarning("Missing second argument in structure for profiler: prof {off/0|all/1|min/2|fps/3}");
            Debug.LogWarning("Missing second argument in structure for profiler: " + profStruct);
        }
        //else if (commandComponents.Count > 1 && (commandComponents[1].Equals("all") || commandComponents[1].Equals("1") || commandComponents[1].Equals("min") || commandComponents[1].Equals("2") 
        //    || commandComponents[1].Equals("off") || commandComponents[1].Equals("0") || commandComponents[1].Equals("fps") || commandComponents[1].Equals("3")))
        else if (commandComponents.Count > 1 && (commandComponents[1].Equals("1") || commandComponents[1].Equals("off") || commandComponents[1].Equals("0") || commandComponents[1].Equals("fps")))
        {
            switch (commandComponents[1].ToString())
            {
                case "all":
                //case "1":
                    GameObject.Find("Profiler").GetComponent<Profiler>().changeProfiler(1);
                    break;
                case "min":
                case "2":
                    GameObject.Find("Profiler").GetComponent<Profiler>().changeProfiler(2);
                    break;
                case "off":
                case "0":
                    GameObject.Find("Profiler").GetComponent<Profiler>().changeProfiler(0);
                    break;
                case "fps":
                case "1":
                    GameObject.Find("Profiler").GetComponent<Profiler>().changeProfiler(3);
                    break;
            }
        }
        else
        {
            //Debug.LogWarning("Invalid second argument in structure for profiler: prof {off/0|all/1|min/2|fps/3}");
            Debug.LogWarning("Invalid second argument in structure for profiler: " + profStruct);
        }
    }

    private void help()
    {
        Debug.Log("Valid commands: ");

        for (int i = 0; i < validCommands.Length; i += 4)
        {
            if (i + 4 <= validCommands.Length)
            {
                Debug.Log("\t" + validCommands[i] + "\t\t\t" + validCommands[i + 1] + "\t\t\t" + validCommands[i + 2] + "\t\t\t" + validCommands[i + 3]);
            }
            else if (i + 3 <= validCommands.Length)
            {
                Debug.Log("\t" + validCommands[i] + "\t\t\t" + validCommands[i + 1] + "\t\t\t" + validCommands[i + 2]);
            }
            else if (i + 2 <= validCommands.Length)
            {
                Debug.Log("\t" + validCommands[i] + "\t\t\t" + validCommands[i + 1]);
            }
            else
            {
                Debug.Log("\t" + validCommands[i]);
            }
        }
    }
}
