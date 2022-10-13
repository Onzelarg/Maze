using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UIElements;


public class FloorGenerator
{
    //  Floor details: Width,height,total size, minimum and maximum room size
    //  Its Index in the game logic
    int floor_width;
    int floor_height;
    int size;
    public int min_room;
    public int max_room;
    int index;
    // Cells array,holding all cells within the floor
    public Tiles[] cells;
    // Cell size and scale within the size
    public float tile_size;
    float tile_scale;
    // Floor and wall object which gets pushed to the cells
    GameObject floor;
    GameObject wall;

    //  ************************************************************************************************************************************* //
    //  ************************************************************************************************************************************* //
    // Constructor which generates the floor object
    public FloorGenerator(float _tile_size, int _grid_width, int _grid_height, float _cell_scale, int _index,GameObject _floor,GameObject _wall)
    {
        this.index = _index;
        this.floor_height = _grid_height;
        this.floor_width = _grid_width;
        this.tile_size = _tile_size;
        this.tile_scale = _cell_scale;
        this.size = _grid_height * _grid_width;
        this.floor = _floor;
        this.wall = _wall;
        this.min_room = (int)(this.size * 0.05);
        this.max_room = (int)(this.size * 0.8);

        generateGrid();

    }

    // Clears and destroys the whole floor
    public void clearGrid()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].clear();
        }
        cells = new Tiles[0];
    }

    // Grid generation that fills the cells array
    public void generateGrid()
    {
        if (cells != null)
        {
            clearGrid();
        }

        cells = new Tiles[size];


        int cell_index = 0;

        if ((tile_scale * 10) / tile_size > 1)
        {
            tile_scale = tile_size / 10;
        }
        for (int i = 0; i < floor_height; i++)
        {
            for (int j = 0; j < floor_width; j++)
            {
                Vector3 cell_position = new Vector3(j * tile_size, 0, i * tile_size);

                cells[cell_index] = new Tiles(cell_position,  floor, cell_index, tile_scale);
                int[] sides = new int[4];
                System.Random rnd = new System.Random();
                for (int k = 0; k < 4; k++)
                {
                    sides[k]= (int)(rnd.Next(0, 2));
                }

                //cells[cell_index].generateSide(sides,wall,tile_size,tile_scale);

                cell_index++;
            }
        }
        Debug.Log(cells.Length);
    }

    public void generateRoom()
    {






    }

}
