using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBehavior : MonoBehaviour
{
    private Transform characterTarget;
    public float mouseSpeedX = 1f;
    public float mouseSpeedY = 8f;
    public float followDistanceNear = 8f;
    public float followDistanceFar = 14f;
    public float restingY = 2.8f;
    public Boolean near = true;

    private float distanceMod = 1.0f;

    void Awake()
    {
        characterTarget = GameObject.Find("DevPlayer").transform;
        setDistance();
    }

    public void setDistance()
    {
        RaycastHit[] hits = Physics.RaycastAll(characterTarget.position, this.transform.position-characterTarget.position, ((near == true) ? followDistanceNear * distanceMod : followDistanceFar * distanceMod));

        if (hits.Length == 0 || hits.Length == 1)
        {
            this.transform.localPosition = new Vector3(0, this.transform.localPosition.y, ((near == true) ? -followDistanceNear * distanceMod : -followDistanceFar * distanceMod));
            return;
        }

        for(short i = 1; i < hits.Length; i++)
        {
            if (hits[i].collider.transform.tag.Equals("MainCamera"))
            {
                Vector3 distance = hits[i - 1].point - hits[i].transform.position;
                this.transform.localPosition = new Vector3(0, this.transform.localPosition.y
                    , -Mathf.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.z, 2)) + 0.05f);
                break;
            }
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeedX * (Time.deltaTime * (((near == true) ? followDistanceNear : followDistanceFar) + 2));
        characterTarget.Rotate(new Vector3(0, mouseX, 0));

        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeedY;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + mouseY * Time.deltaTime, this.transform.localPosition.z);

        //Upper cap for the Y
        if (this.transform.localPosition.y > 4)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 4, this.transform.localPosition.z);
        }
        //Lower limit for the Y
        else if (this.transform.localPosition.y < -4)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, -4, this.transform.localPosition.z);
        }

        //Formula for distance mod
        distanceMod = 1.0f - Math.Abs((this.transform.localPosition.y)*0.05f);

        //Upper cap for the distance mod
        if (distanceMod < 0.8f)
        {
            distanceMod = 0.8f;
        }

        Vector3 relativePos = characterTarget.position - this.transform.position;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(relativePos.x, relativePos.y+1.2f, relativePos.z));
        this.transform.rotation = rotation;

        setDistance();
    }
}