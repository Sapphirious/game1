using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldControls : Bindings
{
    public new Camera camera;
    public Rigidbody controller;
    private float xAxis = 0;
    private float yAxis = 0;
    private Vector3 move;
    public float speed = 10.0f;
    public float gravity = 50.0f;

    private float maxVChange = 10.0f;

    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
        controller = GetComponent<Rigidbody>();
        controller.freezeRotation = true;
        controller.useGravity = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Sets the movement of the charavcter to a X and Y variable that is manipulated by thr predefined key bindings
        move = new Vector3(yAxis, 0, xAxis);
        move = transform.TransformDirection(move);
        move *= speed;

        //Calculates the change in the characters velocity in a given direction
        Vector3 fVelocity = calculateVelocity(getInput());

        //Gives the movement to the character that is claculated and applies it to one of Unitys predefined force modes
        controller.AddForce(fVelocity, ForceMode.VelocityChange);
        //Allows for the manipulation of gravity on the character
        controller.AddForce(new Vector3(0, -gravity * controller.mass, 0));
   }

    //Method used to calculate the speed of which the character is moving in a given direction
    private Vector3 calculateVelocity(Vector3 fVelocity)
    {
        Vector3 currVelocity = controller.velocity;
        Vector3 vChange = fVelocity - currVelocity;
        vChange.x = Mathf.Clamp(vChange.x, -maxVChange, maxVChange);
        vChange.z = Mathf.Clamp(vChange.z, -maxVChange, maxVChange);
        vChange.y = 0;

        return vChange;
    }

    /*Method used to get the inputs from the keybindings and translate them into directions for the character. 
     * Also allows for multiple inputs to change the direction in a diagonal approach*/
    public Vector3 getInput()
    {
        /*
         *  0 = No movement on this axis/direction
         *  1 = Move in the forward/right/up direction depending on the axis specified
         *  -1 = Move in the backwards/left/down direction depending on the axis specified
         *  
         */

        xAxis = 0;
        yAxis = 0;

        if (Input.GetKey(Forward[0]) || Input.GetKey(Forward[1]))
        {
            xAxis = 1;
        }

        if (Input.GetKey(Left[0]) || Input.GetKey(Left[1]))
        {
            yAxis = -1;
        }

        if (Input.GetKey(Right[0]) || Input.GetKey(Right[1]))
        {
            
            yAxis = 1;
        }

        if (Input.GetKey(Backward[0]) || Input.GetKey(Backward[1]))
        { 
            xAxis = -1;
        }

        return move;
    }
}