using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldControls : MonoBehaviour
{
    public float movementSpeed = 16f;
    public float sprintSpeed = 26f;
    private bool sprinting = false;

    private CharacterController character;

    private void Awake()
    {
        character = this.GetComponent<CharacterController>();
    }

    //This should remain as a call from fixedupate
    private void checkKeys()
    {
        Vector3 moveDirection = Vector3.zero;

        //Check for sprinting
        sprinting = (Input.GetKey(Bindings.Sprint[0]) || Input.GetKey(Bindings.Sprint[1]) || Input.GetKey(Bindings.Sprint[2]) ? true : false);

        //If the player is pressing forward
        if (Input.GetKey(Bindings.Forward[0]) || Input.GetKey(Bindings.Forward[1]) || Input.GetKey(Bindings.Forward[2]))
        {
            moveDirection += this.transform.forward * Time.deltaTime * ((sprinting == true) ? sprintSpeed : movementSpeed);
        }
        //If the player is pressing backwards
        if (Input.GetKey(Bindings.Backward[0]) || Input.GetKey(Bindings.Backward[1]) || Input.GetKey(Bindings.Backward[2]))
        {
            moveDirection -= this.transform.forward * Time.deltaTime * ((sprinting == true) ? sprintSpeed : movementSpeed);
        }
        //If the player is pressing right
        if (Input.GetKey(Bindings.Right[0]) || Input.GetKey(Bindings.Right[1]) || Input.GetKey(Bindings.Right[2]))
        {
            moveDirection += this.transform.right * Time.deltaTime * ((sprinting == true) ? sprintSpeed : movementSpeed);
        }
        //If the player is pressing left
        if (Input.GetKey(Bindings.Left[0]) || Input.GetKey(Bindings.Left[1]) || Input.GetKey(Bindings.Left[2]))
        {
            moveDirection -= this.transform.right * Time.deltaTime * ((sprinting == true) ? sprintSpeed : movementSpeed);
        }

        //Gravity
        moveDirection.y -= 100f * Time.deltaTime;
        character.Move(moveDirection * ((sprinting == true) ? sprintSpeed : movementSpeed) * Time.deltaTime);
    }

    private void Update()
    {
        checkKeys();
    }
}