using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Threading;

public class movement : MonoBehaviour
{
    Rigidbody player;
    float speed = 5f;
    bool going;
    bool rotating;
    Vector3 inputVector;
    [SerializeField] Camera top;
    [SerializeField] Camera map_cam;
    [SerializeField] Camera player_cam;
    [SerializeField] int top_view;
    [SerializeField] float mouse_sensitivity_x;
    [SerializeField] float mouse_sensitivity_y;
    [SerializeField] float x_clamp = 85f;
    [SerializeField] bool rotate_on;
    float mouse_x, mouse_y, x_rotation;
    Vector3 offset;


    private void Awake()
    {
        player = GetComponent<Rigidbody>();
        top.enabled = false;
        map_cam.enabled = false;
        top_view = 50;
        mouse_sensitivity_x = 8f;
        mouse_sensitivity_y = 0.5f;
        Cursor.visible = false;
        rotate_on = false;
        offset = new Vector3(0f, 3f, -10f);
    }

    private void Update()
    {
        cameraFollow();
        if (going)
        {
            move();
            updateMapCam();
            updatePlayerCam();
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

    public void map(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (map_cam.enabled)
            {
                map_cam.enabled = false;
            }
            else
            {
                map_cam.enabled = true;
                updateMapCam();
            }
            
        }
    }
    void updateMapCam()
    {
        Vector3 player_pos = player.transform.position;
        map_cam.transform.position = new Vector3(player_pos.x, player_pos.y + top_view, player_pos.z);
    }
    void updatePlayerCam()
    {
        transform.Rotate(Vector3.up, mouse_x * Time.deltaTime);
        player_cam.transform.Rotate(Vector3.up, mouse_x * Time.deltaTime);
        //x_rotation -= mouse_y;
        //x_rotation = Mathf.Clamp(x_rotation, -x_clamp, x_clamp);
        //Vector3 targetRotation = transform.eulerAngles;
        //targetRotation.x = x_rotation;
        //player_cam.transform.eulerAngles = targetRotation;




    }
    public void mouseMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rotating = true;
        }
        if (rotate_on && rotating)
        {
            if (context.control.name == "x")
            {
                mouse_x = context.ReadValue<float>() * mouse_sensitivity_x;
            }
            else
            {
                mouse_y = context.ReadValue<float>() * mouse_sensitivity_y;
            }
            updatePlayerCam();
        }
        if (context.canceled)
        {
            rotating = false;
        }
    }
    void cameraFollow()
    {
        player_cam.transform.position = player.transform.position + offset;
    }
}
