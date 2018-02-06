using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdConsoleSmall : MonoBehaviour
{
    private CmdConsoleManagement cmdConsoleManagement;
    private UnityEngine.UI.Text textBox;

    private bool moveConsole = false;
    public float YMin = 0.66f;
    public float YMax = 1.0f;
    public float speed = 4f;

    private void Awake()
    {
        cmdConsoleManagement = GameObject.Find("CommandConsole").GetComponent<CmdConsoleManagement>();
        textBox = GameObject.Find("CommandConsole/CmdConsole_sml/Scroll View/Text").GetComponent<UnityEngine.UI.Text>();
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

    private void tidyUp()
    {
        this.GetComponentInChildren<UnityEngine.UI.InputField>().text = "";
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

            if (speed > 0)
            {
                GetComponent<RectTransform>().anchorMin = new Vector2(GetComponent<RectTransform>().anchorMin.x, YMin);
                GetComponent<RectTransform>().anchorMax = new Vector2(GetComponent<RectTransform>().anchorMax.x, YMax);
            }

            if (speed < 0)
            {
                tidyUp();
            }
        }
        //------ [END] Movement ------

        textBox.text = cmdConsoleManagement.output.ToString();
    }
}
