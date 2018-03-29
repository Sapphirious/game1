using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldControls : MonoBehaviour
{
    public float movementSpeed = 7f;
    public float sprintSpeed = 13f;
    private bool sprinting = false;

    private CharacterController character;
    private Transform camFollowPoint;

    private bool[] axis = new bool[] { false, false };

    private void Awake()
    {
        character = this.GetComponent<CharacterController>();
        camFollowPoint = this.transform.Find("CamFollowPoint");
    }

    //This should remain as a call from fixedupate
    private void checkKeys()
    {
        Vector3 rotation = Vector3.zero;
        sbyte keyPressed = 0;

        //Check for sprinting
        sprinting = (Input.GetKey(Bindings.Sprint[0]) || Input.GetKey(Bindings.Sprint[1]) || Input.GetKey(Bindings.Sprint[2]) ? true : false);

        //If the player is pressing backwards when the last key was forward
        if ((axis[0] == true) ? ((Input.GetKey(Bindings.Backward[0]) || Input.GetKey(Bindings.Backward[1]) || Input.GetKey(Bindings.Backward[2]))
            && !(Input.GetKey(Bindings.Forward[0]) || Input.GetKey(Bindings.Forward[1]) || Input.GetKey(Bindings.Forward[2])))
            //Check if forward is still being pressed
            : ((Input.GetKey(Bindings.Forward[0]) || Input.GetKey(Bindings.Forward[1]) || Input.GetKey(Bindings.Forward[2]))
            && !(Input.GetKey(Bindings.Backward[0]) || Input.GetKey(Bindings.Backward[1]) || Input.GetKey(Bindings.Backward[2]))))
        {
            keyPressed = (sbyte) ((axis[0] == true) ? -1 : 1);
            rotation += new Vector3(0, camFollowPoint.localEulerAngles.y - ((axis[0] == true) ? 180 : 0), 0);//180 for backwards, 0 for forward
            axis[0] = (axis[0] == true) ? false : true;
        }
        //If the player is pressing forward when the last key was backward
        else if (((axis[0] == true) ? Input.GetKey(Bindings.Forward[0]) || Input.GetKey(Bindings.Forward[1]) || Input.GetKey(Bindings.Forward[2])
            //Check if backward is still being pressed
            : (Input.GetKey(Bindings.Backward[0]) || Input.GetKey(Bindings.Backward[1]) || Input.GetKey(Bindings.Backward[2]))))
        {
            keyPressed = (sbyte)((axis[0] == true) ? 1 : -1);
            rotation += new Vector3(0, camFollowPoint.localEulerAngles.y - ((axis[0] == true) ? 0 : 180), 0);//0 for forward, 180 for backwards
        }

        //If the player is pressing right
        if ((Input.GetKey(Bindings.Right[0]) || Input.GetKey(Bindings.Right[1]) || Input.GetKey(Bindings.Right[2])) 
            && !((Input.GetKey(Bindings.Left[0]) || Input.GetKey(Bindings.Left[1]) || Input.GetKey(Bindings.Left[2]))))
        {
            rotation += new Vector3(0, ((keyPressed != 0) ? 45*keyPressed : camFollowPoint.localEulerAngles.y + 90), 0);
            axis[1] = (axis[1] == true) ? false : true;
            keyPressed = 2;
        }
        //If the player is pressing left
        if ((Input.GetKey(Bindings.Left[0]) || Input.GetKey(Bindings.Left[1]) || Input.GetKey(Bindings.Left[2])) 
            && !((Input.GetKey(Bindings.Right[0]) || Input.GetKey(Bindings.Right[1]) || Input.GetKey(Bindings.Right[2]))))
        {
            rotation += new Vector3(0, ((keyPressed != 0) ? -45*keyPressed : camFollowPoint.localEulerAngles.y - 90), 0);
            keyPressed = 2;
        }

        Vector3 movement = Vector3.zero;

        if (keyPressed != 0)
        {
            this.transform.Rotate(rotation);
            camFollowPoint.localEulerAngles -= rotation;

            movement = transform.forward;
        }

        //Gravity
        movement.y -= 100f * Time.deltaTime;
        //Movement Call
        character.Move(movement * ((sprinting == true) ? sprintSpeed : movementSpeed) * Time.deltaTime);
    }

    private void Update()
    {
        checkKeys();
    }
}