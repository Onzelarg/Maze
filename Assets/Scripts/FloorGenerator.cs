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
        Variables.updateVar(_tile_size,_cell_scale,_grid_height,_grid_width);

        generateGrid(seed);

    }

    public void clearGrid()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].clearAll();
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
        int parent = 0;
        GameObject parent_gameObject = new GameObject("Tiles: " + parent + " - " + (parent + 99));

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
                if (cell_index - parent > 99)
                {
                    parent += 100;
                    parent_gameObject = new GameObject("Tiles: " + parent + " - " + (parent + 99));
                }
                Vector3 cell_position = new Vector3(j * tile_size, 0, i * tile_size);

                cells[cell_index] = new Tiles(cell_position,cell_index);
                cells[cell_index].tile[0].transform.parent = parent_gameObject.transform;

                //int[] sides = new int[4];
                //for (int k = 0; k < 4; k++)
                //{
                //    sides[k]= (int)(rnd.Next(0, 2));
                //}
                ////cells[cell_index].generateSide(sides,wall,tile_size,tile_scale);

                cell_index++;
            }
        }
    }
     
    public void generateRoom(int method,float max_room_ratio,int tries, int minimum_created_rooms)
    {
        // size 400
        // min 20
        // max 200
        int max_size_rooms = (int)(this.size / max_room_ratio);
        //int tries = 200;
        //int minimum_created_rooms = 30;
        DateTime now = DateTime.Now;
        using (StreamWriter w = new StreamWriter("Debugs/method/room_cell_method.txt"))
        {
            
            w.WriteLine(now.ToString("F"));
        }

        System.Random rnd = new System.Random(this.seed);
        int current_size = 0;
        int tried = 0;
        int cell_index;
        int room_size;
        int room_width=0;
        int room_height=0;
        int created_rooms = 0;

        using (StreamWriter writetext = new StreamWriter("Debugs/room_generation.txt"))
        {
            writetext.WriteLine(now.ToString("F"));
            do
            {
                cell_index = (int)(rnd.Next(0, (this.size-1)));
                room_size = (int)(rnd.Next(this.min_room, this.max_room));
                bool can_divided = true;
                bool can_fit_byGrid = true;
                bool can_fit_byMethod = true;
                int room_dim_x = 0;
                int room_dim_z = 0;
                bool room_directionX_positive = true;
                bool room_directionZ_positive = true;
                
                
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
                            can_fit_byGrid = false;
                        }


                        if (room_dim_z + (room_height - 1) < floor_height)
                        {

                        }else if (room_dim_z - (room_height - 1) > 0)
                        {
                            room_directionZ_positive = false;
                        }
                        else
                        {
                            can_fit_byGrid = false;
                        }


                        if (can_fit_byGrid)
                        {
                            if ((method==2 || method==3) && created_rooms != 0)
                            {
                                rooms.Add(new Room(room_size, room_width, room_height));
                                rooms[created_rooms].generate(cell_index, room_directionX_positive, room_directionZ_positive, created_rooms, cells);
                                using (StreamWriter wt = File.AppendText("Debugs/method/room_cell_method.txt")) { wt.WriteLine(""); wt.Write("New Check for: "+rooms.Count); }
                                
                                int method_check = 0;
                                do
                                {
                                    if (method == 2)
                                    {
                                        can_fit_byMethod = rooms[method_check].methodCheck(2, rooms[created_rooms],tile_size);
                                    }
                                    if (method == 3)
                                    {
                                        can_fit_byMethod = rooms[method_check].methodCheck(3, rooms[created_rooms], tile_size);
                                    }
                                } while (++method_check!=(created_rooms) && can_fit_byMethod);
                                rooms.RemoveAt(rooms.Count - 1);

                            }
                            else if (method == 1)
                            {
                                can_fit_byMethod = true;
                            }

                            if (can_fit_byMethod || created_rooms==0)
                            {
                                current_size += room_size;
                                rooms.Add(new Room(room_size, room_width, room_height));
                                rooms[created_rooms].generate(cell_index, room_directionX_positive, room_directionZ_positive, created_rooms, cells);
                                rooms[created_rooms].setMaterial(floor_width,cells,created_rooms);
                                if (method==1 && created_rooms!=0)
                                {
                                    for (int i = 0; i < created_rooms-1; i++)
                                    {
                                        rooms[created_rooms].mergeTiles(rooms[i]);
                                    }
                                    
                                }
                                rooms[created_rooms].setTileSides(cells);
                                //rooms[created_rooms].changeMaterial(cells);
                                created_rooms++;

                                writetext.WriteLine("Created rooms: " + created_rooms);
                                writetext.WriteLine("Cell index: " + cell_index);
                                writetext.WriteLine("Size: " + room_size + " Width: " + room_width + " Height: " + room_height);
                                writetext.WriteLine("Room added");
                            }
                            else
                            {
                                writetext.WriteLine("Can't fit by method");
                            }

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
                if (minimum_created_rooms == created_rooms){ writetext.WriteLine("Limit break"); break; }
            } while (tried!=tries);

            writetext.WriteLine("");
            writetext.WriteLine("Sizes: " + max_size_rooms + " : " + current_size);
            writetext.WriteLine("Created rooms: " + (created_rooms));
        }

    }

}
