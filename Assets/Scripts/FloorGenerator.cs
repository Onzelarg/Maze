using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UIElements;


public class FloorGenerator
{
    int floor_width;
    int floor_height;
    int size;
    int min_room;
    int max_room;
    int index;
    Tiles[] cells;
    float tile_scale;
    float tile_size;
    GameObject floor;
    GameObject wall;

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

    public void clearGrid()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].destroyObjects();
        }
        cells = new Tiles[0];
    }

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

                cells[cell_index] = new Tiles(cell_position, tile_size, floor, wall, cell_index, tile_scale);
                cells[cell_index].generateSide(new int[] { 1, 1, 1, 1 });

                cell_index++;
            }
        }

    }

}
