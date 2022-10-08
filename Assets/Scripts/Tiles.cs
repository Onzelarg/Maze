using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.UIElements;

public class Tiles
{
    GameObject wall;
    GameObject floor;
    GameObject[] walls;

    float x;
    float z;
    float tile_size;
    float wall_size = 0.6f;
    float cell_scale;

    Vector3 left_right = new Vector3(0, 0, 90);
    Vector3 front_back = new Vector3(90, 0, 0);
    
    public int index;


    public Tiles(Vector3 position,float tilesize,GameObject _floor,GameObject _wall,int _index,float _cell_scale)
    {
        this.x = position.x;
        this.z = position.z;
        this.tile_size = tilesize;
        this.floor = _floor;
        this.wall = _wall;
        this.index = _index;
        this.cell_scale = _cell_scale;
        generateCell();
    }

    void generateCell()
    {
        floor=UnityEngine.Object.Instantiate(floor, new Vector3(this.x,0,this.z), new Quaternion());
        floor.name = "Tile " + (int)(index + 1);
        floor.transform.localScale = new Vector3(cell_scale, 1, cell_scale);
    }

    public void generateSide(int[] side)
    {
        //left     //1
        //right    //2
        //front    //3
        //back     //4
        walls = new GameObject[4];
        float xz_offset = tile_size / 2 + wall_size / 2;
        float y = tile_size / 2;
        if (side[0] == 1)
        {
            
            walls[0] = UnityEngine.Object.Instantiate(wall, new Vector3(this.x - xz_offset, y, this.z), new Quaternion());
            walls[0].transform.Rotate(left_right);
            walls[0].name = "Wall " + (int)(index + 1) + " left";
            walls[0].transform.localScale = new Vector3(cell_scale*10, wall_size, cell_scale*10);
        }
        if (side[1] == 1)
        {
            walls[1] = UnityEngine.Object.Instantiate(wall, new Vector3(this.x + xz_offset, y, this.z), new Quaternion());
            walls[1].transform.Rotate(left_right);
            walls[1].name = "Wall " + (int)(index + 1) + " right";
            walls[1].transform.localScale = new Vector3(cell_scale * 10, wall_size, cell_scale * 10);
        }
        if (side[2] == 1)
        {
            walls[2] = UnityEngine.Object.Instantiate(wall, new Vector3(this.x, y, this.z - xz_offset), new Quaternion());
            walls[2].transform.Rotate(front_back);
            walls[2].name = "Wall " + (int)(index + 1) + " front";
            walls[2].transform.localScale = new Vector3(cell_scale * 10, wall_size, cell_scale * 10);
        }
        if (side[3] == 1)
        {
            walls[3] = UnityEngine.Object.Instantiate(wall, new Vector3(this.x, y, this.z + xz_offset), new Quaternion());
            walls[3].transform.Rotate(front_back);
            walls[3].name = "Wall " + (int)(index + 1) + " back";
            walls[3].transform.localScale = new Vector3(cell_scale * 10, wall_size, cell_scale * 10);
        }

    }


    public void destroyObjects()
    {
        UnityEngine.Object.Destroy(floor);
        for (int i = 0; i < walls.Length; i++)
        {
            UnityEngine.Object.Destroy(walls[i]);
        }
    }

}
