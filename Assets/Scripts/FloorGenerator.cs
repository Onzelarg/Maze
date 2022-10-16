using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UIElements;
using System.IO;

public class FloorGenerator
{

    int floor_width;
    int floor_height;
    public int size;
    public int min_room;
    public int max_room;
    int index;
    public Tiles[] cells;
    public float tile_size;
    float tile_scale;
    GameObject floor;
    GameObject wall;
    int seed;
    Room[] rooms;

    protected class Room
    {
        public int top;
        public int bottom;
        public int left;
        public int right;
        public int room_size;
        public int room_width;
        public int room_height;

        public Room(int size,int w,int h)
        {
            this.room_size = size;
            this.room_width = w;
            this.room_height = h;
        }
    }

    public FloorGenerator(float _tile_size, int _grid_width, int _grid_height, float _cell_scale, int _index,GameObject _floor,GameObject _wall,int seed)
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
        this.max_room = (int)(this.size * 0.5);

        generateGrid(seed);

    }

    public void clearGrid()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].clear();
        }
        cells = new Tiles[0];
    }

    public void generateGrid(int _seed)
    {
        if (cells != null)
        {
            clearGrid();
        }
        this.seed = _seed;
        cells = new Tiles[size];


        int cell_index = 0;

        if ((tile_scale * 10) / tile_size > 1)
        {
            tile_scale = tile_size / 10;
        }
        System.Random rnd = new System.Random(this.seed);
        for (int i = 0; i < floor_height; i++)
        {
            for (int j = 0; j < floor_width; j++)
            {
                Vector3 cell_position = new Vector3(j * tile_size, 0, i * tile_size);

                cells[cell_index] = new Tiles(cell_position,  floor, cell_index, tile_scale);
                int[] sides = new int[4];
                for (int k = 0; k < 4; k++)
                {
                    sides[k]= (int)(rnd.Next(0, 2));
                }
                //cells[cell_index].generateSide(sides,wall,tile_size,tile_scale);

                cell_index++;
            }
        }
    }

    public void generateRoom()
    {
        Debug.ClearDeveloperConsole();
        // size 400
        // min 20
        // max 200
        System.Random rnd = new System.Random(this.seed);
        int max_size_rooms=(int)(this.size/0.6);
        int current_size = 0;
        int tries = 200;
        int tried = 0;
        int cell_index;
        int room_size;
        int room_width=0;
        int room_height=0;
        int created_rooms = 0;
        int minimum_created_rooms = 30;
        rooms = new Room[10];

        using (StreamWriter writetext = new StreamWriter("Debugs/room_generation.txt"))
        {
        
            do
            {
                cell_index = (int)(rnd.Next(0, this.size));
                room_size = (int)(rnd.Next(this.min_room, this.max_room));

                Debug.Log("Tries: " + tried);
                Debug.Log("Current Size: " + current_size);
                Debug.Log("<color=green>Created rooms: " + created_rooms + "</color>");

                writetext.WriteLine("");
                writetext.WriteLine("Tries: " + (tried+1));
                writetext.WriteLine("Current Size: " + current_size);
                writetext.WriteLine("Generated Size: " + room_size+" added: "+ (current_size+room_size));
                if (current_size + room_size < max_size_rooms)
                {
                    current_size += room_size;
                    if (room_size % 11 == 0)
                    {
                        room_width = 11;
                        room_height = room_size / 11;
                        rooms[created_rooms] = new Room(room_size, room_width, room_height);
                        created_rooms++;
                        Debug.Log("<color=yellow>" + 11 + "</color>");
                        Debug.Log("Cell index: " + cell_index);
                        Debug.Log("<color=red>Size: " + room_size + " Width: " + room_width + " Height: " + room_height + "</color>");

                        writetext.WriteLine("Created rooms: " + created_rooms);
                        writetext.WriteLine("Cell index: " + cell_index);
                        writetext.WriteLine("Size: " + room_size + " Width: " + room_width + " Height: " + room_height);
                        writetext.WriteLine("Room added");
                    }
                    else if (room_size % 7 == 0)
                    {
                        room_width = 7;
                        room_height = room_size / 7;
                        rooms[created_rooms] = new Room(room_size, room_width, room_height);
                        created_rooms++;
                        Debug.Log("Cell index: " + cell_index);
                        Debug.Log("<color=red>Size: " + room_size + " Width: " + room_width + " Height: " + room_height + "</color>");

                        writetext.WriteLine("Created rooms: " + created_rooms);
                        writetext.WriteLine("Cell index: " + cell_index);
                        writetext.WriteLine("Size: " + room_size + " Width: " + room_width + " Height: " + room_height);
                        writetext.WriteLine("Room added");
                    }
                    else if (room_size % 5 == 0)
                    {
                        room_width = 5;
                        room_height = room_size / 5;
                        rooms[created_rooms] = new Room(room_size, room_width, room_height);
                        created_rooms++;
                        Debug.Log("<color=yellow>" + 5 + "</color>");
                        Debug.Log("Cell index: " + cell_index);
                        Debug.Log("<color=red>Size: " + room_size + " Width: " + room_width + " Height: " + room_height + "</color>");

                        writetext.WriteLine("Created rooms: " + created_rooms);
                        writetext.WriteLine("Cell index: " + cell_index);
                        writetext.WriteLine("Size: " + room_size + " Width: " + room_width + " Height: " + room_height);
                        writetext.WriteLine("Room added");
                    }
                    else if (room_size % 3 == 0)
                    {
                        room_width = 3;
                        room_height = room_size / 3;
                        rooms[created_rooms] = new Room(room_size, room_width, room_height);
                        created_rooms++;
                        Debug.Log("<color=yellow>" + 3 + "</color>");
                        Debug.Log("Cell index: " + cell_index);
                        Debug.Log("<color=red>Size: " + room_size + " Width: " + room_width + " Height: " + room_height + "</color>");

                        writetext.WriteLine("Created rooms: " + created_rooms);
                        writetext.WriteLine("Cell index: " + cell_index);
                        writetext.WriteLine("Size: " + room_size + " Width: " + room_width + " Height: " + room_height);
                        writetext.WriteLine("Room added");
                    }
                    else if (room_size % 2 == 0)
                    {
                        room_width = 2;
                        room_height = room_size / 2;
                        rooms[created_rooms] = new Room(room_size, room_width, room_height);
                        created_rooms++;
                        Debug.Log("<color=yellow>" + 2 + "</color>");
                        Debug.Log("Cell index: " + cell_index);
                        Debug.Log("<color=red>Size: " + room_size + " Width: " + room_width + " Height: " + room_height + "</color>");

                        writetext.WriteLine("Created rooms: " + created_rooms);
                        writetext.WriteLine("Cell index: " + cell_index);
                        writetext.WriteLine("Size: " + room_size + " Width: " + room_width + " Height: " + room_height);
                        writetext.WriteLine("Room added");
                    }
                    else
                    {
                        current_size -= room_size;
                        Debug.Log("<color=blue>Landed here</color>");

                        writetext.WriteLine("Can't be divided");
                    }


                }
                else
                {
                    writetext.WriteLine("Room too big");
                }

                if (max_size_rooms - current_size < this.min_room)
                {
                    Debug.Log(max_size_rooms - current_size + " : " + this.min_room);
                    Debug.Log("Size break");

                    writetext.WriteLine(max_size_rooms - current_size + " : " + this.min_room);
                    writetext.WriteLine("Size break");
                    break;
                }

                tried++;
                if (minimum_created_rooms == created_rooms){ break; }
            } while (tried!=tries);
            Debug.Log("Sizes: " + max_size_rooms + " : " + current_size);
            Debug.Log("Created rooms: "+(created_rooms+1));

            writetext.WriteLine("");
            writetext.WriteLine("Sizes: " + max_size_rooms + " : " + current_size);
            writetext.WriteLine("Created rooms: " + (created_rooms));
        }

    }

}
