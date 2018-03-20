using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class CmdConsoleLarge : MonoBehaviour
{
    private RectTransform scrollView;
    private List<Pair<GameObject, StringBuilder>> textSegments = new List<Pair<GameObject, StringBuilder>>();
    private List<short> linesInSegment = new List<short>();

    private void Awake()
    {
        scrollView = GameObject.Find("CommandConsole/CmdConsole_lrg/Scroll View").GetComponent<RectTransform>();
    }

    public void closeConsole()
    {
        this.GetComponentInChildren<UnityEngine.UI.InputField>().text = "";
        GameObject.Find("CommandConsole").GetComponent<Canvas>().enabled = false;
        this.gameObject.SetActive(false);//Must be the last thing ran
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
        textSeg.transform.SetParent(GameObject.Find("CommandConsole/CmdConsole_lrg/Scroll View/ViewPort/Content").transform);

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
            //Remove any unwanted new lines or returns
            newLog[0]= Regex.Replace(newLog[0].Substring(0, 1), @"\n|\r|\r\n", String.Empty) + newLog[0].Substring(1, newLog[0].Length-1);
        }

        //Display the current text
        textSegments[textSegments.Count - 1].First.GetComponent<UnityEngine.UI.Text>().text += String.Join(String.Empty, newLog.ToArray());
        //Cache the current text
        textSegments[textSegments.Count - 1].Second.Append(String.Join(String.Empty, newLog.ToArray()));
        //Count how many lines there are in the segment
        linesInSegment[linesInSegment.Count - 1] += (short)(newLog.Count);

        updateCollider(textSegments[textSegments.Count - 1].First.GetComponent<BoxCollider2D>());

        //While there are more than 64 text segments (culling dependent), remove 1
        while (textSegments.Count > 64)
        {
            textSegments.RemoveRange(0, 1);
        }
    }

    /// <summary>
    /// Update the 2D box collider
    /// </summary>
    /// <param name="boxCollider"></param>
    private void updateCollider(BoxCollider2D boxCollider)
    {
        boxCollider.offset = new Vector2(1, Math.Abs(textSegments[textSegments.Count - 1].First.GetComponent<RectTransform>().rect.height) / 2);
        boxCollider.size = new Vector2(2, Math.Abs(textSegments[textSegments.Count - 1].First.GetComponent<RectTransform>().rect.height));
    }

    public void checkForInvalidCharas(UnityEngine.UI.InputField inputField)
    {
        //Remove any new lines from the input field
        inputField.text = Regex.Replace(this.GetComponentInChildren<UnityEngine.UI.InputField>().text, @"\t|\n|\r|`", String.Empty);
    }

    /// <summary>
    /// Generate new lines given the amount of new lines to create
    /// </summary>
    /// <param name="newLines"></param>
    /// <returns></returns>
    private StringBuilder generateNewLines(int newLines)
    {
        StringBuilder returnString = new StringBuilder();

        for (int i = 0; i < newLines; i++)
        {
            returnString.Append("\n");
        }

        return returnString;
    }

    ///<summary>Gets called whenever the scrollbar updates</summary>
    public void checkForCulling()
    {
        //Get the corner positions from the scroll view gameobject
        Vector3[] corners = new Vector3[4];
        scrollView.GetWorldCorners(corners);

        //Cast a ray on the leftmost side of the scroll view to collide with the text segments
        RaycastHit2D[] hits = Physics2D.RaycastAll(corners[0], Vector2.up, corners[1].y - corners[0].y);



        //Iterate through the text segments
        for (short i = 0; i < textSegments.Count; i++)
        {
            updateCollider(textSegments[i].First.GetComponent<BoxCollider2D>());

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
