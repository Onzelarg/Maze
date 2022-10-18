using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.UIElements;

public class Tiles
{
    public GameObject[] tile;  
    public float x;   
    public float z;    
    float wall_size = 0.6f; 
    public int[] side;     
    Vector3 left_right = new Vector3(0, 0, 90);    
    Vector3 front_back = new Vector3(90, 0, 0);    
    public int index;      
    bool visited = false;
    public Material visited_mat;
    public Material corner;
    public Material index_mat;

    public Tiles(Vector3 position,GameObject _floor,int _index,float _cell_scale)
    {
        this.x = position.x;
        this.z = position.z;
        this.index = _index;
        visited_mat = Resources.Load("Room") as Material;
        corner = Resources.Load("Corner") as Material;
        index_mat = Resources.Load("Index") as Material;
        generateCell(_floor,_cell_scale);
    }

    void generateCell(GameObject _floor,float _cell_scale)
    {
        tile = new GameObject[5];
        tile[0] = UnityEngine.Object.Instantiate(_floor, new Vector3(this.x,0,this.z), new Quaternion());
        tile[0].name = "Tile " + (int)(index);
        tile[0].transform.localScale = new Vector3(_cell_scale, 1, _cell_scale);
    }

    public void generateSide(int[] _side,GameObject _wall,float _tile_size, float _cell_scale)
    {
        //left     //1
        //right    //2
        //front    //3
        //back     //4

        side = _side;
        float xz_offset = _tile_size / 2 + wall_size / 2;
        float y = _tile_size / 2;
        if (side[0] == 1)
        {
            tile[1] = UnityEngine.Object.Instantiate(_wall, new Vector3(this.x - xz_offset, y, this.z), new Quaternion());
            tile[1].transform.Rotate(left_right);
            tile[1].name = "Wall " + (int)(index + 1) + " left";
            tile[1].transform.localScale = new Vector3(_cell_scale * 10, wall_size, _cell_scale * 10);
        }
        if (side[1] == 1)
        {
            tile[2] = UnityEngine.Object.Instantiate(_wall, new Vector3(this.x + xz_offset, y, this.z), new Quaternion());
            tile[2].transform.Rotate(left_right);
            tile[2].name = "Wall " + (int)(index + 1) + " right";
            tile[2].transform.localScale = new Vector3(_cell_scale * 10, wall_size, _cell_scale * 10);
        }
        if (side[2] == 1)
        {
            tile[3] = UnityEngine.Object.Instantiate(_wall, new Vector3(this.x, y, this.z - xz_offset), new Quaternion());
            tile[3].transform.Rotate(front_back);
            tile[3].name = "Wall " + (int)(index + 1) + " front";
            tile[3].transform.localScale = new Vector3(_cell_scale * 10, wall_size, _cell_scale * 10);
        }
        if (side[3] == 1)
        {
            tile[4] = UnityEngine.Object.Instantiate(_wall, new Vector3(this.x, y, this.z + xz_offset), new Quaternion());
            tile[4].transform.Rotate(front_back);
            tile[4].name = "Wall " + (int)(index + 1) + " back";
            tile[4].transform.localScale = new Vector3(_cell_scale * 10, wall_size, _cell_scale * 10);
        }

    }

    public void changeMaterial()
    {
        tile[0].GetComponent<Renderer>().material = visited_mat;
    }
    public void _corner()
    {
        tile[0].GetComponent<Renderer>().material = corner;
    }
    public void cellindex()
    {
        tile[0].GetComponent<Renderer>().material = index_mat;
    }
    public void clear()
    {
        for (int i = 0; i < tile.Length; i++)
        {
            UnityEngine.Object.Destroy(tile[i]);
        }

    }

}
