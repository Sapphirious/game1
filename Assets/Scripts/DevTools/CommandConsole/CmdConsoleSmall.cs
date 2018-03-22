using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CmdConsoleSmall : MonoBehaviour
{
    private RectTransform scrollView;
    private GameObject GO_Content;

    private List<GameObject> textSegments = new List<GameObject>();

    private bool moveConsole = false;
    public float YMin = 0.66f;
    public float YMax = 1.0f;
    public float speed = 4f;

    private void Awake()
    {
        GO_Content = GameObject.Find("CommandConsole/CmdConsole_sml/Scroll View/ViewPort/Content");
        scrollView = GameObject.Find("CommandConsole/CmdConsole_sml/Scroll View").GetComponent<RectTransform>();
    }

    public void openConsole()
    {
        GetComponent<RectTransform>().anchorMax = new Vector2(GetComponent<RectTransform>().anchorMax.x, 1 + YMax - YMin);
        GetComponent<RectTransform>().anchorMin = new Vector2(GetComponent<RectTransform>().anchorMin.x, 1);
        speed = Mathf.Abs(speed);
        moveConsole = true;
    }

    public void closeConsole()
    {
        speed = Mathf.Abs(speed) * -1;
        moveConsole = true;
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
        textSeg.transform.SetParent(GO_Content.transform);

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
        textRect.anchorMin = new Vector2(0f, 0f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.localScale = new Vector3(1f, 1f, 1f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        textRect.anchoredPosition3D = Vector3.zero;

        //Set up the 2D box collider component
        textSeg.AddComponent<BoxCollider2D>();
        textSeg.GetComponent<BoxCollider2D>().isTrigger = true;

        textSegments.Add(textSeg);
    }

    public void recieveNewLogs(String text)
    {
        createTextSegment();
        //Remove any unwanted new lines or returns (THIS IS NEEDED TO MAKE SURE THERE ISN'T AN EXTRA NEW LINE WHEN A NEW TEXT SEGMENT IS GENERATED)
        text = Regex.Replace(text.Substring(0, 1), @"\n|\r|\r\n", String.Empty) + text.Substring(1, text.Length - 1);
        //Display the current text
        textSegments[textSegments.Count - 1].GetComponent<Text>().text = text;

        //While there are more than Screen.height/8 text segments (culling dependent), remove 1
        while (textSegments.Count > Screen.height/8)
        {
            Destroy(textSegments[0]);
            textSegments.RemoveRange(0, 1);
        }

        if (this.gameObject.activeInHierarchy == true)
        {
            StartCoroutine("initializeSegments");
        }
    }

    IEnumerator initializeSegments()
    {
        for (int i = 0; i < textSegments.Count; i++)
        {
            textSegments[i].GetComponent<BoxCollider2D>().offset = new Vector2(1, Math.Abs(textSegments[i].GetComponent<RectTransform>().rect.height) / 2);
            textSegments[i].GetComponent<BoxCollider2D>().size = new Vector2(2, Math.Abs(textSegments[i].GetComponent<RectTransform>().rect.height));
        }

        yield return null;

        //Get the corner positions from the scroll view gameobject
        Vector3[] corners = new Vector3[4];
        scrollView.GetWorldCorners(corners);

        //Cast a ray on the leftmost side of the scroll view to collide with the text segments
        RaycastHit2D[] hits = Physics2D.RaycastAll(corners[0], Vector2.up, corners[1].y - corners[0].y);

        short iterateAmount = (short)textSegments.Count;

        //Iterate through the text segments
        for (short i = (short)(iterateAmount - 1); i >= 0; i--)
        {
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j].collider != null && hits[j].collider.gameObject == textSegments[i])
                {
                    break;
                }
                else if (j + 1 == hits.Length)
                {
                    Destroy(textSegments[i]);
                    textSegments.RemoveRange(i, 1);
                }
            }
        }
    }

    private void tidyUp()
    {
        this.GetComponentInChildren<InputField>().text = "";
        GameObject.Find("CommandConsole").GetComponent<Canvas>().enabled = false;
        this.gameObject.SetActive(false);//Must be the last thing ran
    }

    private void Update()
    {
        //------ [START] Movement ------
        if (moveConsole == true && ((speed > 0 && GetComponent<RectTransform>().anchorMax.y > YMax) || (speed < 0 && GetComponent<RectTransform>().anchorMax.y < 1 + YMax - YMin)))
        {
            GetComponent<RectTransform>().anchorMax = new Vector2(GetComponent<RectTransform>().anchorMax.x, GetComponent<RectTransform>().anchorMax.y - Time.deltaTime * speed);
            GetComponent<RectTransform>().anchorMin = new Vector2(GetComponent<RectTransform>().anchorMin.x, GetComponent<RectTransform>().anchorMin.y - Time.deltaTime * speed);
        }

        if (moveConsole == true && ((speed > 0 && GetComponent<RectTransform>().anchorMax.y <= YMax) || (speed < 0 && GetComponent<RectTransform>().anchorMax.y >= 1 + YMax - YMin)))
        {
            moveConsole = false;

            //Finished opening
            if (speed > 0)
            {
                GetComponent<RectTransform>().anchorMin = new Vector2(GetComponent<RectTransform>().anchorMin.x, YMin);
                GetComponent<RectTransform>().anchorMax = new Vector2(GetComponent<RectTransform>().anchorMax.x, YMax);
                StartCoroutine("initializeSegments");
            }
            //Finished closing
            else if (speed < 0)
            {
                tidyUp();
            }
        }
        //------ [END] Movement ------
    }
}
