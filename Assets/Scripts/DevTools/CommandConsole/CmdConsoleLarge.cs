﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class CmdConsoleLarge : MonoBehaviour
{
    private GameObject GO_content;
    private RectTransform scrollView;
    private List<Pair<GameObject, StringBuilder>> textSegments = new List<Pair<GameObject, StringBuilder>>();
    private List<short> linesInSegment = new List<short>();

    private void Awake()
    {
        GO_content = GameObject.Find("CommandConsole/CmdConsole_lrg/Scroll View/ViewPort/Content");
        scrollView = GameObject.Find("CommandConsole/CmdConsole_lrg/Scroll View").GetComponent<RectTransform>();
    }

    public void openConsole()
    {
        //While there are more than 64 text segments (culling dependent), remove 1
        while (textSegments.Count > 64)
        {
            Destroy(textSegments[0].First);
            textSegments.RemoveRange(0, 1);
        }

        StartCoroutine("updateCollider", getArrayOfGameObjects());
        checkForCulling();
    }

    public void closeConsole()
    {
        this.GetComponentInChildren<UnityEngine.UI.InputField>().text = "";
        GameObject.Find("CommandConsole").GetComponent<Canvas>().enabled = false;
        this.gameObject.SetActive(false);//Must be the last thing ran
    }

    private GameObject[] getArrayOfGameObjects()
    {
        List<GameObject> returnValue = new List<GameObject>();

        for (int i = 0; i < textSegments.Count; i++)
        {
            returnValue.Add(textSegments[i].First);
        }

        return returnValue.ToArray();
    }

    /// <summary>
    /// Factory method to make text segments
    /// </summary>
    private void createTextSegment()
    {
        //Create the game object and save the RectTransform for now
        GameObject textSeg = new GameObject("TextSeg", typeof(RectTransform)) as GameObject;
        RectTransform textRect = textSeg.transform.GetComponent<RectTransform>();

        //Set up its layer
        textSeg.layer = LayerMask.NameToLayer("UI");

        //Parent it
        textSeg.transform.SetParent(GO_content.transform);

        //Set up the text component
        textSeg.AddComponent<UnityEngine.UI.Text>();
        textSeg.GetComponent<UnityEngine.UI.Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        textSeg.GetComponent<UnityEngine.UI.Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
        textSeg.GetComponent<UnityEngine.UI.Text>().verticalOverflow = VerticalWrapMode.Truncate;
        textSeg.GetComponent<UnityEngine.UI.Text>().resizeTextForBestFit = true;
        textSeg.GetComponent<UnityEngine.UI.Text>().resizeTextMaxSize = 128;
        textSeg.GetComponent<UnityEngine.UI.Text>().resizeTextMinSize = 1;
        textSeg.GetComponent<UnityEngine.UI.Text>().lineSpacing = 1.05f;

        //Anchors/position for the text
        textRect.pivot = new Vector2(0.5f, 0f);
        textRect.localScale = new Vector3(1f, 1f, 1f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        textRect.anchoredPosition3D = Vector3.zero;

        //Set up the 2D box collider component
        textSeg.AddComponent<BoxCollider2D>();
        textSeg.GetComponent<BoxCollider2D>().isTrigger = true; 

        textSegments.Add(new Pair<GameObject, StringBuilder>(textSeg, new StringBuilder()));
        linesInSegment.Add(0);
    }

    public void recieveNewLogs(List<String> newLog)
    {
        //A segment is soft-capped at 20 lines (meaning that if a segment has 19 lines and a command comes in with 4, it will allow that and then end the segment)
        if (textSegments.Count == 0 || linesInSegment[linesInSegment.Count - 1] >= 20)
        {
            createTextSegment();
            //Remove any unwanted new lines or returns (THIS IS NEEDED TO MAKE SURE THERE ISN'T AN EXTRA NEW LINE WHEN A NEW TEXT SEGMENT IS GENERATED)
            newLog[0]= Regex.Replace(newLog[0].Substring(0, 1), @"\n|\r|\r\n", String.Empty) + newLog[0].Substring(1, newLog[0].Length-1);
        }

        //Display the current text
        textSegments[textSegments.Count - 1].First.GetComponent<UnityEngine.UI.Text>().text += String.Join(String.Empty, newLog.ToArray());
        //Cache the current text
        textSegments[textSegments.Count - 1].Second.Append(String.Join(String.Empty, newLog.ToArray()));
        //Count how many lines there are in the segment
        linesInSegment[linesInSegment.Count - 1] += (short)(newLog.Count);

        //While there are more than 64 text segments (culling dependent), remove 1
        while (textSegments.Count > 64)
        {
            Destroy(textSegments[0].First);
            textSegments.RemoveRange(0, 1);
        }

        if (this.gameObject.activeInHierarchy == true)
        {
            StartCoroutine("updateCollider", new GameObject[] { textSegments[textSegments.Count - 1].First });
        }
    }

    /// <summary>
    /// Update the 2D box collider
    /// </summary>
    /// <param name="boxCollider"></param>
    IEnumerator updateCollider(GameObject[] objs)
    {
        yield return null;

        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].GetComponent<BoxCollider2D>().offset = new Vector2(1, Math.Abs(objs[i].GetComponent<RectTransform>().rect.height) / 2);
            objs[i].GetComponent<BoxCollider2D>().size = new Vector2(2, Math.Abs(objs[i].GetComponent<RectTransform>().rect.height));
        }
    }

    /// <summary>
    /// Generate new lines given the amount of new lines to create
    /// </summary>
    /// <param name="newLines"></param>
    /// <returns></returns>
    private StringBuilder generateNewLines(int newLines)
    {
        StringBuilder returnString = new StringBuilder();

        for (int i = 0; i < newLines-1; i++)
        {
            returnString.Append("\n");
        }

        return returnString;
    }

    ///<summary>Gets called whenever the scrollbar updates</summary>
    public void checkForCulling()
    {
        StartCoroutine("checkCulling");
    }

    IEnumerator checkCulling()
    {
        yield return null;

        //Get the corner positions from the scroll view gameobject
        Vector3[] corners = new Vector3[4];
        scrollView.GetWorldCorners(corners);

        //Cast a ray on the leftmost side of the scroll view to collide with the text segments
        RaycastHit2D[] hits = Physics2D.RaycastAll(corners[0], Vector2.up, corners[1].y - corners[0].y);

        //Iterate through the text segments
        for (short i = 0; i < textSegments.Count; i++)
        {
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j].collider != null && hits[j].collider.gameObject == textSegments[i].First)
                {
                    textSegments[i].First.name = "TextSeg (Active)";//TODO remove after debugging
                    textSegments[i].First.GetComponent<UnityEngine.UI.Text>().text = textSegments[i].Second.ToString();//Get cached text that was saved
                    break;
                }
                else if (j + 1 == hits.Length)
                {
                    textSegments[i].First.name = "TextSeg (Deactive)";//TODO remove after debugging
                    textSegments[i].First.GetComponent<UnityEngine.UI.Text>().text = generateNewLines(linesInSegment[i]).ToString();//Get empty lines
                }
            }
        }
    }
}
