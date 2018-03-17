using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // Use this for initialization
    public Transform followObject;
    public float distanceFromCharacter;
    public float hieghtFromCharacter;
    public float minCameraDown;
    public float maxCameraUp;
    public float mouseSpeedY;
    public float mouseSpeedX;
    private float mouseX;
    private float mouseY;
    //private Quaternion rotation;

    void Start()
    {

    }

    void LateUpdate()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSpeedX * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSpeedY * Time.deltaTime;

        setPositionOfCamera();
        setRotationValuesOfCamera();

    }

    private void setPositionOfCamera()
    {
        this.transform.position = new Vector3(followObject.position.x, followObject.position.y + hieghtFromCharacter, followObject.position.z - distanceFromCharacter);
    }

    private void setRotationValuesOfCamera()
    {
        //rotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.Rotate(new Vector3(mouseY, 0, 0));
        transform.RotateAround(followObject.position, Vector3.up, mouseX);
        this.transform.eulerAngles = new Vector3(0, mouseX, 0);
    }
}
