using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

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
    public bool visited = false;
    public Material visited_mat;
    public Material corner;
    public Material index_mat;
    public int[] neighbors = new int[4];
    public bool is_partofRoom = false;

    public Tiles(Vector3 position,int _index)
    {
        this.x = position.x;
        this.z = position.z;
        this.index = _index;
        visited_mat = Resources.Load("Room") as Material;
        corner = Resources.Load("Corner") as Material;
        index_mat = Resources.Load("Index") as Material;
        side = new int[] { 0, 0, 0, 0 };
        generateCell();
        getNeighbors();
    }

    void generateCell()
    {
        GameObject floor = Resources.Load("floor") as GameObject;
        tile = new GameObject[5];
        tile[0] = UnityEngine.Object.Instantiate(floor, new Vector3(this.x,0,this.z), new Quaternion());
        tile[0].name = "Tile " + (int)(index);
        //tile[0].transform.localScale = new Vector3(2, 1, 2);
        tile[0].transform.localScale = new Vector3(Variables.tile_scale, 1, Variables.tile_scale);
    }

    void getNeighbors()
    {
        int fw = Variables.floor_width;
        int fh = Variables.floor_height;
        // left     //1
        if (this.index % fw==0)
        {
            neighbors[0] = -1;
        }
        else
        {
            neighbors[0] = this.index-1;
        }
        //right    //2
        if (this.index % fw == (fw-1))
        {
            neighbors[1] = -1;
        }
        else
        {
            neighbors[1] = this.index + 1;
        }
        //top    //3
        if ((fw*fh)-this.index<=fw)
        {
            neighbors[3] = -1;
        }
        else
        {
            neighbors[3] = this.index + fw;
        }
        //bottom     //4
        if (this.index / fh == 0)
        {
            neighbors[2] = -1;
        }
        else
        {
            neighbors[2] = this.index - fw;
        }
    }

    public void generateSide(int[] _side,GameObject _wall)
    {
        //left     //1
        //right    //2
        //front    //3
        //back     //4
        side = new int[4];
        side = _side;
        //tilesize 15 0.6 15/2 + 0.6/2
        float xz_offset = Variables.tile_size / 2  - wall_size / 2 ;
        float y = Variables.tile_size / 2;
        if (side[0] == 1)
        {
            tile[1] = UnityEngine.Object.Instantiate(_wall, new Vector3(this.x - xz_offset, y, this.z), new Quaternion());
            tile[1].transform.Rotate(left_right);
            tile[1].name = "Wall " + (int)(index) + " left";
            tile[1].transform.localScale = new Vector3(Variables.tile_scale * 10, wall_size, Variables.tile_scale * 10);
            tile[1].transform.parent = tile[0].transform;
        }
        if (side[1] == 1)
        {
            tile[2] = UnityEngine.Object.Instantiate(_wall, new Vector3(this.x + xz_offset, y, this.z), new Quaternion());
            tile[2].transform.Rotate(left_right);
            tile[2].name = "Wall " + (int)(index) + " right";
            tile[2].transform.localScale = new Vector3(Variables.tile_scale * 10, wall_size, Variables.tile_scale * 10);
            tile[2].transform.parent = tile[0].transform;
        }
        if (side[2] == 1)
        {
            tile[3] = UnityEngine.Object.Instantiate(_wall, new Vector3(this.x, y, this.z - xz_offset), new Quaternion());
            tile[3].transform.Rotate(front_back);
            tile[3].name = "Wall " + (int)(index) + " front";
            tile[3].transform.localScale = new Vector3(Variables.tile_scale * 10, wall_size, Variables.tile_scale * 10);
            tile[3].transform.parent = tile[0].transform;
        }
        if (side[3] == 1)
        {
            tile[4] = UnityEngine.Object.Instantiate(_wall, new Vector3(this.x, y, this.z + xz_offset), new Quaternion());
            tile[4].transform.Rotate(front_back);
            tile[4].name = "Wall " + (int)(index) + " back";
            tile[4].transform.localScale = new Vector3(Variables.tile_scale * 10, wall_size, Variables.tile_scale * 10);
            tile[4].transform.parent = tile[0].transform;
        }
    }

    public void changeMaterial(Material mat)
    {
        tile[0].GetComponent<Renderer>().material = mat;
    }

    public void clearAll()
    {
        for (int i = 0; i < tile.Length; i++)
        {
            UnityEngine.Object.Destroy(tile[i]);
        }

    }
     
    public void clearWalls()
    {
        for (int i = 1; i < tile.Length; i++)
        {
            UnityEngine.Object.Destroy(tile[i]);
        }
    }

    public void clearWall(int i)
    {
        UnityEngine.Object.Destroy(tile[i]);
    }
    public void makeWalls()
    {
        side = new int[] { 1, 1, 1, 1 };
        generateSide(side, Resources.Load("Wall") as GameObject);
    }

    public void checkNeighbor(Tiles[] cells)
    {
        clearWalls();
        side = new int[] { 0, 0, 0, 0 };
        for (int i = 0; i < 4; i++)
        {
            if (neighbors[i] == -1)
            {
                side[i] = 1;
            }else if (!cells[neighbors[i]].visited)
            {
                side[i] = 1;
            }
        }
        generateSide(side, Resources.Load("Wall") as GameObject);



    }

}
