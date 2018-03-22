using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // Use this for initialization
    public Transform followObject;
    public float mouseSpeedX;
    private Vector3 offset;
   
    void Start()
    {
        offset = followObject.transform.position - transform.position;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeedX;
        followObject.transform.Rotate(0, mouseX, 0);

        float desiredAngle = followObject.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = followObject.transform.position - (rotation * offset);

        transform.LookAt(followObject.transform);
    }

}
