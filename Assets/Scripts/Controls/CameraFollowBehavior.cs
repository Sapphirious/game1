using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBehavior : MonoBehaviour
{
    private Transform followPoint;
    private Transform castPoint;

    public float mouseSpeedX = 1f;
    public float mouseSpeedY = 8f;
    public float followDistanceNear = 8f;
    public float followDistanceFar = 14f;
    public float restingY = 2.8f;
    public Boolean near = true;

    private float distanceMod = 1.0f;

    void Awake()
    {
        followPoint = this.transform.parent;
        castPoint = followPoint.Find("CamCastPoint");
    }

    private void Start()
    {
        setDistance();
    }

    public void setDistance()
    {
        castPoint.localPosition = new Vector3(0, castPoint.localPosition.y, ((near == true) ? -followDistanceNear : -followDistanceFar) * distanceMod);

        RaycastHit[] hits = Physics.RaycastAll(followPoint.position, castPoint.position-followPoint.position, Vector3.Distance(followPoint.position, castPoint.position));
        //Debug.DrawRay(followPoint.position, castPoint.position - followPoint.position, Color.red, 2.5f);
        /*Debug.Log("Ray hits: " + hits.Length);

        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log(hits[i].collider.name);
        }*/

        if (hits.Length == 0 || hits.Length == 1)
        {
            this.transform.localPosition = castPoint.localPosition;
            return;
        }
        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.name == this.transform.name)
            {
                continue;
            }

            float distance = Vector3.Distance(this.transform.InverseTransformPoint(hits[i].point), this.transform.InverseTransformPoint(this.transform.position));
            Debug.Log("Distance: " + distance);
            this.transform.localPosition = new Vector3(0, this.transform.localPosition.y
                , castPoint.transform.localPosition.z+distance);
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeedX * (Time.deltaTime * ((near == true) ? followDistanceNear : followDistanceFar));
        followPoint.Rotate(new Vector3(0, mouseX, 0));

        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeedY;
        this.transform.localPosition = new Vector3(0, this.transform.localPosition.y + mouseY * Time.deltaTime, this.transform.localPosition.z);
        castPoint.localPosition = new Vector3(0, castPoint.localPosition.y + mouseY * Time.deltaTime, castPoint.localPosition.z);

        //Upper cap for the Y
        if (this.transform.localPosition.y > 4)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 4, this.transform.localPosition.z);
            castPoint.localPosition = new Vector3(castPoint.localPosition.x, 4, castPoint.localPosition.z);
        }
        //Lower limit for the Y
        else if (this.transform.localPosition.y < -4)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, -4, this.transform.localPosition.z);
            castPoint.localPosition = new Vector3(castPoint.localPosition.x, -4, castPoint.localPosition.z);
        }

        //Formula for distance mod
        distanceMod = 1.0f - Math.Abs((castPoint.localPosition.y)*0.05f);

        //Upper cap for the distance mod
        if (distanceMod < 0.8f)
        {
            distanceMod = 0.8f;
        }

        Vector3 relativePos = followPoint.position - this.transform.position;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(relativePos.x, relativePos.y, relativePos.z));
        this.transform.rotation = rotation;

        setDistance();
    }
}