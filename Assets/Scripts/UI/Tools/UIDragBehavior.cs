using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDragBehavior : MonoBehaviour
{
    public bool affectParent = true;
    private Vector3 mousePosOnClick = new Vector3();
    private bool wasClickedFirst = false;

    public void storeMousePos()
    {
        mousePosOnClick = Input.mousePosition;
        wasClickedFirst = true;
    }

    public void releaseClick()
    {
        wasClickedFirst = false;
    }

    public void drag()
    {
        if (wasClickedFirst == false)
        {
            return;
        }

        Vector3 offset = mousePosOnClick - Input.mousePosition;
        mousePosOnClick = Input.mousePosition;

        if (affectParent == true)
        {
            this.transform.parent.GetComponent<RectTransform>().localPosition
                = new Vector3(this.transform.parent.GetComponent<RectTransform>().localPosition.x - offset.x, this.transform.parent.GetComponent<RectTransform>().localPosition.y - offset.y, 0);
        }
        else
        {
            this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x - offset.x, this.GetComponent<RectTransform>().localPosition.y - offset.y, 0);
        }
    }
}
