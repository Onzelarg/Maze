using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UIElements;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Linq;

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
    List<GameObject> tile_parents = new List<GameObject>();
    int seed;
    public List<Room> rooms=new List<Room>();
    List<int> corridor = new List<int>();

    public FloorGenerator(float _tile_size, int _grid_width, int _grid_height, float _cell_scale, int _index,GameObject _floor,GameObject _wall,int seed,float min_room_multiplier,float max_room_multiplier)
    {
        this.index = _index;
        this.floor_height = _grid_height;
        this.floor_width = _grid_width;
        this.tile_size = _tile_size;
        this.tile_scale = _cell_scale;
        this.size = _grid_height * _grid_width;
        this.floor = _floor;
        this.wall = _wall;
        this.min_room = (int)(this.size * min_room_multiplier);
        this.max_room = (int)(this.size * max_room_multiplier);
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
        for (int i = 0; i < tile_parents.Count; i++)
        {
            UnityEngine.Object.Destroy(tile_parents[i]);
        }
    }

    public void clearNotvisited()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (!cells[i].visited)
            {
                cells[i].clearAll();
            }
            
        }
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
        tile_parents.Add(parent_gameObject);

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
                    tile_parents.Add(parent_gameObject);
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
        int max_size_rooms = (int)(this.size * max_room_ratio);
        //int tries = 200;
        //int minimum_created_rooms = 30;

        Stopwatch operation = new Stopwatch();
        operation.Start();

        DateTime now = DateTime.Now;

        System.Random rnd = new System.Random(this.seed);
        int current_size = 0;
        int tried = 0;
        int cell_index;
        int room_size;
        int room_width=0;
        int room_height=0;
        int created_rooms = 0;
        int cant_divided = 0;
        int cant_fit = 0;
        int cant_fit_method = 0;
        int too_big = 0;
        int not_unique = 0;
        int counter = 0;
        string output = "";

        using (StreamWriter writetext = new StreamWriter("Debugs/room_generation.txt"))
        {
            writetext.WriteLine(now.ToString("F"));
            output += now.ToString("F");
            do
            {
                if (tried==153)
                {
                    Debug.Log("");
                }
                cell_index = (int)(rnd.Next(0, (this.size-1)));
                room_size = (int)(rnd.Next(this.min_room, this.max_room));
                bool can_divided = true;
                bool can_fit_byGrid = true;
                bool can_fit_byMethod = true;
                bool is_unique = true;
                int room_dim_x = 0;
                int room_dim_z = 0;
                bool room_directionX_positive = true;
                bool room_directionZ_positive = true;
                
                
                writetext.WriteLine("Tries: " + (tried+1));
                writetext.WriteLine("Current Size: " + current_size);
                writetext.WriteLine("Generated Size: " + room_size+" added: "+ (current_size+room_size));

                 
                if (current_size + room_size < max_size_rooms)
                {
                    //2 3 5	7 11 13	17 19 23 29	31 37 41 43 47 53 59 61 67 71
                    if (max_size_rooms>1000)
                    {
                        if (room_size % 17 == 0)
                        {
                            room_width = 17;
                            room_height = room_size / 17;
                        }
                        if (room_size % 29 == 0)
                        {
                            room_width = 29;
                            room_height = room_size / 29;
                        }
                        if (room_size % 37 == 0)
                        {
                            room_width = 37;
                            room_height = room_size / 37;
                        }
                        if (room_size % 43 == 0)
                        {
                            room_width = 43;
                            room_height = room_size / 43;
                        }
                        if (room_size % 59 == 0)
                        {
                            room_width = 59;
                            room_height = room_size / 59;
                        }
                    }
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
                        cant_divided++;
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
                                rooms.Add(new Room(room_size, room_width, room_height));
                                rooms[created_rooms].generate(cell_index, room_directionX_positive, room_directionZ_positive, created_rooms, cells);
                                rooms[created_rooms].setMaterial(floor_width, cells, created_rooms);
                                if (method==1 && created_rooms!=0)
                                {
                                    for (int i = 0; i < created_rooms; i++)
                                    {
                                        is_unique=rooms[created_rooms].checkUniqueness(rooms[i]);
                                        if (!is_unique)
                                        {
                                            not_unique++;
                                            break;
                                        }
                                    }
                                     
                                }

                                if (is_unique)
                                {
                                    for (int i = 0; i < created_rooms; i++)
                                    {
                                        rooms[created_rooms].mergeTiles(cells,rooms[i], counter++, created_rooms, i);
                                    }
                                    current_size += room_size;
                                    created_rooms++;
                                }
                                else
                                {
                                    rooms.RemoveAt(rooms.Count - 1);
                                }
                                

                                writetext.WriteLine("Created rooms: " + created_rooms);
                                writetext.WriteLine("Cell index: " + cell_index);
                                writetext.WriteLine("Size: " + room_size + " Width: " + room_width + " Height: " + room_height);
                                writetext.WriteLine("Room added");
                            }
                            else
                            {
                                writetext.WriteLine("Can't fit by method");
                                cant_fit_method++;
                            }

                        } 
                        else
                        {
                            writetext.WriteLine("Can't fit into grid");
                            cant_fit++;
                        }
                    }

                }
                else
                {
                    writetext.WriteLine("Room too big");
                    too_big++;
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
            writetext.WriteLine("Can't divided: " + cant_divided);
            writetext.WriteLine("Can't fit: " + cant_fit);
            writetext.WriteLine("Can't fit by method: " + cant_fit_method);
            writetext.WriteLine("Room too big: " + too_big);

            output+=("\n");
            output += ("Sizes: " + max_size_rooms + " : " + current_size+"\n");
            output += ("Created rooms: " + (created_rooms) + "\n");
            output += ("Can't divided: " + cant_divided + "\n");
            output += ("Can't fit: " + cant_fit + "\n");
            output += ("Can't fit by method: " + cant_fit_method) + "\n";
            output += ("Room too big: " + too_big + "\n");
            output += ("Not unique: " + not_unique + "\n");
            using (StreamWriter wt = new StreamWriter("Debugs/room_generation_string.txt"))
            {
                wt.Write(output);
            }

            for (int i = 0; i < created_rooms; i++)
            {
                rooms[i].setTileSides(cells);
                rooms[i].setVisited(cells);
                rooms[i].makePartofRoom(cells);
            }
            for (int i = 0; i < created_rooms; i++)
            { 
                if (rooms[i].connected_rooms.Count==0)
                {
                    int other_room = rooms[i].room_index;
                    while (other_room == rooms[i].room_index) {
                        other_room = UnityEngine.Random.Range(0, rooms.Count);
                    }
                    rooms[i].makeConnection(cells, rooms[other_room], rooms[other_room].room_index,corridor);
                }
            }
            for (int i = 0; i < corridor.Count; i++)
            {
                if (cells[corridor[i]].visited)
                {
                    cells[corridor[i]].checkNeighbor(cells);
                }
            }
            operation.Stop();
            writetext.WriteLine("Operation took: " + operation.ElapsedMilliseconds + " milliseconds");
        }
        generateMazeNoRooms(true);

    }
    public void makeWalls(bool skip_walls)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (!skip_walls)
            {
                cells[i].makeWalls();
            }else if (!cells[i].visited)
            {
                cells[i].makeWalls();
            }
            
        }
    }

    public void generateMazeNoRooms(bool skip_walls = false,int max_runs = -1)
    {
        makeWalls(skip_walls);
        int runs = 0;
        int current = UnityEngine.Random.Range(0, size);
        List<int> previous = new List<int>();
        previous.Add(current);
        bool not_finished = true;

        Dictionary<int, int> neighbors = new Dictionary<int, int>();
        for (int i = 0; i < cells.Length; i++)
        {
            neighbors[i] = 0;
            for (int j = 0; j < 4; j++)
            {
                if (cells[i].neighbors[j] != -1)
                {
                    neighbors[i]++;
                }
            }
        }

        do
        {
            int next = UnityEngine.Random.Range(0, 3);
            while (cells[current].neighbors[next]==-1 || cells[cells[current].neighbors[next]].visited)
            {
                if (neighbors[current] > 0)
                {
                    neighbors[current]--;
                }
                else
                {
                    previous.RemoveAt(previous.Count - 1);
                    if (previous.Count == 0)
                    {
                        not_finished = false;
                        break;
                    }
                    current = previous.Last();
                }
                next = UnityEngine.Random.Range(0, 4);
            }
            if (!not_finished)
            {
                break;
            }
            cells[current].clearWall(next+1);
            
            if (neighbors[current]>0)
            {
                neighbors[current]--;
            }
            
            cells[current].visited=true;
            current = cells[cells[current].neighbors[next]].index;
            previous.Add(current);
            bool not_changed = true;
            if (next == 0) { next = 2; not_changed = false; }
            if (next == 1 && not_changed) { next = 1; not_changed = false; }
            if (next == 2 && not_changed) { next = 4; not_changed = false; }
            if (next == 3 && not_changed) { next = 3; not_changed = false; }
            cells[current].clearWall(next);
            cells[current].visited = true;

            // 0 1 2 3
            // 1->2 2->1 3->4 4->3


            runs++;
            
            if (runs==max_runs)
            {
                break;
            }
        } while (not_finished);
        Debug.Log(runs);



    }

}
