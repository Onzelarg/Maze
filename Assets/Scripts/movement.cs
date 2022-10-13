using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System;

public class movement : MonoBehaviour
{
    public GameObject player;

    public int speed;
    public int acceleration;
    public int maxSpeed;
    public int jump_height;

    public Vector3 jump;


    void Start()
    {
         player = GameObject.FindGameObjectWithTag("Player");
         acceleration = 1;
         speed = 5;
         maxSpeed = 10;
         jump_height = 10;
         jump = new Vector3(0, jump_height, 0);
        player.transform.position = new Vector3(0, 10, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        if (horizontalInput!=0 || verticalInput!=0)
        {
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
            movement.Normalize();
            player.transform.Translate(movement * Time.deltaTime*speed);
            if (speed<maxSpeed)
            {
                speed += acceleration;
            }
            //Debug.Log("Player position: "+player.transform.position.x + " : " + player.transform.position.z);


        }
        if (horizontalInput==0 && verticalInput==0)
        {
            speed = 1;
        }

        
        
        if (Input.GetKey("space"))
        {
            
            player.transform.Translate(jump*Time.deltaTime);
        }

    }

    void OnCollisionEnter(Collision floor)
    {
        MazeGenerator.update_material(floor.collider.name);

    }

    
    



}

