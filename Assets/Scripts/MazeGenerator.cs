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
    static public FloorGenerator[] floors;
    int index;
    static public Material visited;
    public int floorSize;
    public int floorMin;
    public int floorMax;
    [SerializeField] public int tries;
    [SerializeField] public float max_room_ratio;
    [SerializeField] public int minimum_created_room;
    Player player;
    public Map map;

    //left z-90 right z+90
    //front x+90 back x+90

    void Start()
    {
        tile_size = 15f;
        grid_width = 20;
        grid_height = 20;
        cell_scale = 1.5f;
        index = 0;
        floors = new FloorGenerator[10];
        visited = Resources.Load("Room") as Material;
        tries = 200;
        max_room_ratio = 0.6f;
        minimum_created_room = 30;
        generate(12345678,0.05f,0.2f);
        //floors[0].generateRoom();
    }

    public void generate(int seed,float min,float max)
    {
        floors[index] = new FloorGenerator(tile_size, grid_width, grid_height,cell_scale,index,floor,wall,seed,min,max);
        floorSize = floors[index].size;
        floorMin = floors[index].min_room;
        floorMax = floors[index].max_room;
        map.drawEmpty();
    }

    public void clear()
    {
        floors[0].clearGrid();
    }
    public void clearNotvisited()
    {
        floors[0].clearNotvisited();
    }
    public void generateRooms(int method)
    {
        floors[0].generateRoom(method,max_room_ratio,tries,minimum_created_room);
    }

    public int update_data(int data,int index)
    {
        if (data == 0) { return floors[index].min_room; }
        if (data == 1) { return floors[index].max_room; }
        if (data == 2) { return floors[index].cells.Length; }
        return -1;
    }

    public int[] update_data_arr(int data, int index,int cell_index)
    {
        int[] _data = new int[8];
        if (data == 0)
        { 
            for (int i = 0; i < 4; i++)
            {
                _data[i]=floors[index].cells[cell_index].side[i];
                _data[i + 4] = floors[index].cells[cell_index].neighbors[i];
            }
        }
        return _data;
        
    }

    public bool[] isVisited(int index)
    {
        bool[] returns = new bool[2];
        if (floors[0].cells[index].visited)
        {
            returns[0] = true;
        }
        else
        {
            returns[0] = false;
        }
        if (floors[0].cells[index].is_partofRoom)
        {
            returns[1] = true;
        }
        else
        {
            returns[1] = false;
        }
        return returns;
    }
    
    public static void update_material(string _cell_index)
    {
        if (_cell_index.Split(" ")[0]=="Tile")
        {
            int cell_index = Convert.ToInt32(_cell_index.Split(" ")[1]);
            floors[0].cells[cell_index].tile[0].GetComponent<Renderer>().material = visited;
        }
    }

    public void room()
    {
        for (int i = 0; i < floors[0].rooms.Count; i++)
        {
            floors[0].rooms[i].changeMaterial(floors[0].cells);
        }
        
    }

    public void room(int room)
    {
        floors[0].rooms[room].changeMaterial(floors[0].cells);
    }
     
    public void cleartile()
    {
        for (int i = 0; i < 50; i++)
        {
            floors[0].cells[i].clearWalls();
        }
        
    }

    public void roomCek(int index)
    {
        floors[0].rooms[index].cekMAt(floors[0].cells);
    }
    public void makeWalls()
    {
        floors[0].makeWalls();
    }
    public void generateMazeNoRooms()
    {
        floors[0].generateMazeNoRooms();
    }
    public void clearWall(int index)
    {
        floors[0].cells[index].clearWalls();
    }
    public void ccon()
    {
        floors[0].checkConnectivity();
    }

    public void spawnPlayer()
    {
        player = new Player();
        player.spawnPlayer(floors[0].cells);
    }
    public void createPlayer()
    {
        player = new Player();
    }
    public void fixc()
    {
        floors[0].fixCorridor();
    }
}