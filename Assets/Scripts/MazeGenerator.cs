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

    

    //left z-90 right z+90
    //front x+90 back x+90 


    void Start()
    {
        tile_size = 15f;
        grid_width = 5;
        grid_height = 5;
        cell_scale = 2f;
        generateGrid();
    }

    public void clearGrid()
    {
        for (int i = 0; i < cell.Length; i++)
        {
            cell[i].destroyObjects();
        }
        cell = new Tiles[0];
    }

   public void generateGrid()
    {
        if (cell!=null)
        {
            clearGrid();
        }

        cell = new Tiles[grid_height * grid_width];


        int index = 0;
        

        cell = new Tiles[grid_height * grid_width];

        if ((cell_scale*10)/tile_size>1)
        {
            cell_scale = tile_size/10;
        }
        for (int i = 0; i < grid_height; i++)
        {
            for (int j = 0; j < grid_width; j++)
            {
                cell_position = new Vector3(j * tile_size, 0, i * tile_size);

                cell[index] = new Tiles(cell_position, tile_size, floor, wall,index, cell_scale);
                cell[index].generateSide(new int[] {1,1,1,1});
                
                index++;
            }
        }

    }

}