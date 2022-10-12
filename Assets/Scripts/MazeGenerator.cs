using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public GameObject floor;
    public GameObject wall;
    Vector3 cell_position;
    public float cell_scale;
    public float tile_size;
    public int grid_width;
    public int grid_height;
    public Tiles[] cell;
    public FloorGenerator[] floors;
    int index;
    

    //left z-90 right z+90
    //front x+90 back x+90 


    void Start()
    {
        tile_size = 15f;
        grid_width = 5;
        grid_height = 5;
        cell_scale = 2f;
        index = 1;
        floors = new FloorGenerator[10];
    }

    public void generate()
    {
        floors[index] = new FloorGenerator(tile_size, grid_width, grid_height,cell_scale,index,floor,wall);

    }

   

}