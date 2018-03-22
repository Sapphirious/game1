using System;
using System.Collections.Generic;
using UnityEngine;

public class Profiler : MonoBehaviour
{
    private bool profilerOn = false;
    private byte mode = 0;

    private Dictionary<String, GameObject> objectsCreated = new Dictionary<String, GameObject>();

    public void changeProfiler(int mode)
    {
        this.mode = (byte) mode;

        switch (mode)
        {
            case 1://all
                allOn();
                mode = 1;
                break;
            case 2://min
                minOn();
                mode = 2;
                break;
            case 0://off
                Debug.Log("Turning off the display of all profiler stats.");
                allOff();
                mode = 0;
                break;
            case 3://just FPS
                Debug.Log("Toggling FPS display to: " + ((mode == 3 && profilerOn == true) ? "<b>OFF</b>." : "<b>ON</b>."));
                fpsOn(true);
                mode = 3;
                break;
            default:
                Debug.LogError("Made it to the default case when changing the profiler.");
                break;
        }
    }

    private void allOn()
    {
        if (mode == 3 && profilerOn == true)
        {
            allOff();
            return;
        }

        allOff();
        profilerOn = true;

        //Turn on the fps counter
        fpsOn(false);

        InvokeRepeating("updateStats", 0.2f, 1f);
    }

    private void minOn()
    {
        if (mode == 3 && profilerOn == true)
        {
            allOff();
            return;
        }

        allOff();
        profilerOn = true;

        //Turn on the fps counter
        fpsOn(false);

        InvokeRepeating("updateStats", 0.2f, 1f);
    }

    private void fpsOn(bool doExtras)
    {
        if (mode == 3 && profilerOn == true)
        {
            allOff();
            return;
        }

        if (doExtras == true)
        {
            allOff();
            profilerOn = true;
        }

        objectsCreated.Add("fps", new GameObject("FPS", typeof(RectTransform)) as GameObject);
        objectsCreated.Add("fps_background", new GameObject("FPS_background", typeof(RectTransform)) as GameObject);
        setTextProperties(objectsCreated["fps"], objectsCreated["fps_background"], "FPS: #");

        if (doExtras == true)
        {
            InvokeRepeating("updateStats", 0.2f, 1f);
        }
    }

    private void allOff()
    {
        profilerOn = false;

        CancelInvoke("updateStats");

        foreach (KeyValuePair<string, GameObject> entry in objectsCreated)
        {
            Destroy(entry.Value);
        }

        objectsCreated.Clear();
    }

    private void setTextProperties(GameObject textObj, GameObject backgroundObj, string text)
    {
        //Get the RectTransforms
        RectTransform textRect = textObj.transform.GetComponent<RectTransform>();
        RectTransform backgroundRect = backgroundObj.transform.GetComponent<RectTransform>();

        //Set the heiarchy
        textObj.transform.SetParent(backgroundObj.transform);
        backgroundObj.transform.SetParent(this.gameObject.transform);

        //Anchors/position for the text
        textRect.anchorMin = new Vector2(0.01f, 0.01f);
        textRect.anchorMax = new Vector2(0.99f, 0.99f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        textRect.anchoredPosition3D = Vector3.zero;
        textRect.localScale = new Vector3(1f, 1f, 1f);

        //Set up the text component
        textObj.AddComponent<UnityEngine.UI.Text>();
        textObj.GetComponent<UnityEngine.UI.Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        textObj.GetComponent<UnityEngine.UI.Text>().text = text;
        textObj.GetComponent<UnityEngine.UI.Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
        textObj.GetComponent<UnityEngine.UI.Text>().resizeTextForBestFit = true;
        textObj.GetComponent<UnityEngine.UI.Text>().resizeTextMaxSize = 128;
        textObj.GetComponent<UnityEngine.UI.Text>().resizeTextMinSize = 1;

        //Anchors/position for the background
        backgroundRect.anchorMin = new Vector2(0f, 0.975f - ((objectsCreated.Count / 2 - 1) * 0.025f));
        backgroundRect.anchorMax = new Vector2(0.2f, 1f - ((objectsCreated.Count / 2 - 1) * 0.025f));
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;
        backgroundRect.anchoredPosition3D = Vector3.zero;
        backgroundRect.localScale = new Vector3(1f, 1f, 1f);

        //Set up the image component
        backgroundObj.AddComponent<UnityEngine.UI.Image>();
        Color color;
        ColorUtility.TryParseHtmlString("#38383860", out color);
        backgroundObj.GetComponent<UnityEngine.UI.Image>().color = color;
    }

    private void crampBackground(GameObject backgroundObject, GameObject textObject)
    {
        UnityEngine.UI.Text text = textObject.GetComponent<UnityEngine.UI.Text>();

        int totalLength = 0;

        CharacterInfo characterInfo = new CharacterInfo();

        char[] arr = text.text.ToCharArray();

        foreach (char c in arr)
        {
            text.font.GetCharacterInfo(c, out characterInfo, text.cachedTextGenerator.fontSizeUsedForBestFit);

            totalLength += characterInfo.advance;
        }

        backgroundObject.transform.GetComponent<RectTransform>().anchorMax = new Vector2(totalLength * 1.0f / Screen.width + 0.005f, backgroundObject.transform.GetComponent<RectTransform>().anchorMax.y);
    }

    private void updateStats()
    {
        if (objectsCreated.ContainsKey("fps_background") == true)
        {
            objectsCreated["fps"].GetComponent<UnityEngine.UI.Text>().text = "FPS: " + (float)((int) ((1.0 / Time.deltaTime) * 10))/10;
            crampBackground(objectsCreated["fps_background"], objectsCreated["fps"]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(Bindings.ToggleFPS) == true)
        {
            if (profilerOn == true)
            {
                allOff();
            }
            else
            {
                fpsOn(true);
            }
        }
    }
}
