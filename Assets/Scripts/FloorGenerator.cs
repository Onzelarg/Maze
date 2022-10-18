using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UIElements;
using System.IO;

public class FloorGenerator
{

    public int floor_width;
    public int floor_height;
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
    public List<Room> rooms=new List<Room>();

    public class Room
    {
        public int topleft;
        public int topright;
        public int bottomleft;
        public int bottomright;

        public int room_size;
        public int room_width;
        public int room_height;
        public int[] room_cells;
        public int cell_index;

        public Room(int size,int w,int h)
        {
            this.room_size = size;
            this.room_width = w;
            this.room_height = h;
            room_cells = new int[size];
        }


        public void generate(int _cell_index, bool _x_positive, bool _z_positive, int _floor_w, int _floor_h,int created_rooms)
        {
            this.cell_index = _cell_index;
            using (StreamWriter writetext = new StreamWriter("Debugs/room_cell_generation"+created_rooms+".txt"))
            {
                if (_x_positive && _z_positive)
                {
                    this.bottomleft = _cell_index;
                    this.bottomright = bottomleft + room_width-1;
                    this.topleft = bottomleft + ((room_height-1) * _floor_w);
                    this.topright = topleft + room_width-1;
                    writetext.WriteLine("Cell index is bottom left ");
                }
                else if (_x_positive && !_z_positive)
                {
                    this.topleft = _cell_index;
                    this.topright = topleft + room_width-1;
                    this.bottomleft = topleft - ((room_height-1) * _floor_w);
                    this.bottomright = bottomleft + room_width-1;
                    writetext.WriteLine("Cell index is top left ");
                }
                else if (!_x_positive && _z_positive)
                {
                    this.bottomright = _cell_index;
                    this.bottomleft = bottomright - room_width+1;
                    this.topleft = bottomleft + ((room_height-1) * _floor_w);
                    this.topright = topleft + room_width-1;
                    writetext.WriteLine("Cell index is bottom right ");
                }
                else if (!_x_positive && !_z_positive)
                {
                    this.topright = _cell_index;
                    this.topleft = topright - room_width+1;
                    this.bottomleft = topleft - ((room_height-1) * _floor_w);
                    this.bottomright = bottomleft + room_width-1;
                    writetext.WriteLine("Cell index is top right ");

                }
                writetext.WriteLine("Cell index: "+_cell_index);
                writetext.WriteLine("Top Left: "+topleft);
                writetext.WriteLine("Top Right: "+topright);
                writetext.WriteLine("Bottom Left: "+bottomleft);
                writetext.WriteLine("Bottom Right: "+bottomright);
                writetext.WriteLine("Room width: "+room_width);
                writetext.WriteLine("Room height: "+room_height);
                writetext.WriteLine("X positive: "+_x_positive);
                writetext.WriteLine("Z positive: "+_z_positive);
                writetext.WriteLine("Cells: ");

                int index = 0;
                for (int z = 0; z < room_height; z++)
                {
                    for (int x = 0; x < room_width; x++)
                    {
                        room_cells[index] = bottomleft + (z * _floor_w) + x;
                        writetext.WriteLine(room_cells[index]);
                        index++;
                    }
                }

            }
        }

        public void changeMaterial(Tiles[] cells)
        {
            using (StreamWriter writetext = new StreamWriter("Debugs/room_cell_generation_mat_change.txt"))
            {
                for (int i = 0; i < room_cells.Length; i++)
                {
                    cells[room_cells[i]].changeMaterial();
                }
                cells[this.topleft]._corner();
                cells[this.topright]._corner();
                cells[this.bottomleft]._corner();
                cells[this.bottomright]._corner();
                cells[this.cell_index].cellindex();
                writetext.WriteLine("Cell index: " + this.cell_index);
                writetext.WriteLine("Top Left: " + this.topleft);
                writetext.WriteLine("Top Right: " + this.topright);
                writetext.WriteLine("Bottom Left: " + this.bottomleft);
                writetext.WriteLine("Bottom Right: " + this.bottomright);
            }
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

        using (StreamWriter writetext = new StreamWriter("Debugs/room_generation.txt"))
        {
        
            do
            {
                cell_index = (int)(rnd.Next(0, (this.size+1)));
                room_size = (int)(rnd.Next(this.min_room, this.max_room));
                bool can_divided = true;
                int room_dim_x = 0;
                int room_dim_z = 0;
                bool can_fit = true;
                bool room_directionX_positive = true;
                bool room_directionZ_positive = true;
                
                writetext.WriteLine("");
                writetext.WriteLine("Tries: " + (tried+1));
                writetext.WriteLine("Current Size: " + current_size);
                writetext.WriteLine("Generated Size: " + room_size+" added: "+ (current_size+room_size));

                if (current_size + room_size < max_size_rooms)
                {

                    if (room_size % 11 == 0)
                    {
                        room_width = 11;
                        room_height = room_size / 11;
                    }
                    else if (room_size % 7 == 0)
                    {
                        room_width = 7;
                        room_height = room_size / 7;
                    }
                    else if (room_size % 5 == 0)
                    {
                        room_width = 5;
                        room_height = room_size / 5;
                    }
                    else if (room_size % 3 == 0)
                    {
                        room_width = 3;
                        room_height = room_size / 3;
                    }
                    else if (room_size % 2 == 0)
                    {
                        room_width = 2;
                        room_height = room_size / 2;
                    }
                    else
                    {
                        can_divided = false;
                        writetext.WriteLine("Can't be divided");  
                    }

                    if (can_divided)
                    {
                        //x*tile_size
                        //z*tile_size
                        
                        

                        if (cells[cell_index].x!=0)
                        {
                            room_dim_x = (int)((cells[cell_index].x / tile_size)+1);
                        }
                        if (cells[cell_index].z != 0)
                        {
                            room_dim_z = (int)((cells[cell_index].z / tile_size)+1);
                        }


                        if ((room_dim_x+(room_width-1)<floor_width))
                        {
                            
                        }else if((room_dim_x - (room_width - 1) > 0))
                        {
                            room_directionX_positive = false;
                        }
                        else
                        {
                            can_fit = false;
                        }


                        if (room_dim_z + (room_height - 1) < floor_height)
                        {

                        }else if (room_dim_z - (room_height - 1) > 0)
                        {
                            room_directionZ_positive = false;
                        }
                        else
                        {
                            can_fit = false;
                        }


                        if (can_fit)
                        {
                            current_size += room_size;
                            rooms.Add(new Room(room_size, room_width, room_height));
                            rooms[created_rooms].generate(cell_index,room_directionX_positive,room_directionZ_positive,floor_width,floor_height,created_rooms);
                            rooms[created_rooms].changeMaterial(cells);
                            created_rooms++;

                            writetext.WriteLine("Created rooms: " + created_rooms);
                            writetext.WriteLine("Cell index: " + cell_index);
                            writetext.WriteLine("Size: " + room_size + " Width: " + room_width + " Height: " + room_height);
                            writetext.WriteLine("Room added");
                        }
                        else
                        {
                            writetext.WriteLine("Can't fit into grid");
                        }

                    }

                }
                else
                {
                    writetext.WriteLine("Room too big");
                }




                if (max_size_rooms - current_size < this.min_room)
                {

                    writetext.WriteLine(max_size_rooms - current_size + " : " + this.min_room);
                    writetext.WriteLine("Size break");
                    break;
                }

                tried++;
                if (minimum_created_rooms == created_rooms){ break; }
            } while (tried!=tries);

            writetext.WriteLine("");
            writetext.WriteLine("Sizes: " + max_size_rooms + " : " + current_size);
            writetext.WriteLine("Created rooms: " + (created_rooms));
        }

    }

}
