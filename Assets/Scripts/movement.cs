using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class movement : MonoBehaviour
{
    Rigidbody player;
    float speed = 5f;
    bool going;
    Vector3 inputVector;
    Vector3 rotation;
    public Camera camera;
    public Camera playerCam;

    private void Awake()
    {
        player = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (going)
        {
            move();
        }
    }

    public void jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.AddForce(Vector3.up * speed, ForceMode.Impulse);
        }
    }

    public void playerMovement(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            //Vector2 inputVector = context.ReadValue<Vector2>();
            going = true;
            inputVector = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
            Debug.Log(inputVector.x + " : " + inputVector.y + " : " + inputVector.z);
            player.AddForce(inputVector*10,ForceMode.Force);
        }
        if (context.canceled)
        {
            going = false;
        }
    }
    void move()
    {
        player.AddForce(inputVector * 10, ForceMode.Force);
    }
    void rot()
    {
        //transform.localRotation = Quaternion.Euler(rotation);
        playerCam.transform.Rotate(rotation);
        player.transform.Rotate(rotation);
    }

    public void map(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (camera.enabled)
            {
                camera.enabled = false;
            }
            else
            {
                camera.enabled = true;
            }
            
        }
    }

    public void rotate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.control.name=="q")
            {
                rotation = new Vector3(0, -30, 0);
            }
            if (context.control.name == "e")
            {
                rotation = new Vector3(0, 30, 0);
            }
            rot();
        }



        }



}
