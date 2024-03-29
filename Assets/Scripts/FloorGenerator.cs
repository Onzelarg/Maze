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
using TMPro;
using UnityEngine.UI;

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
    public float tile_scale;
    GameObject floor;
    GameObject wall;
    List<GameObject> tile_parents = new List<GameObject>();
    int seed;
    public List<Room> rooms=new List<Room>();
    List<int> corridor = new List<int>();
    public List<int> unvisited;
    GameObject[] texts;

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
        this.texts = new GameObject[this.size];

        Variables.updateVar(_tile_size, _cell_scale, _grid_height, _grid_width);
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
        for (int i = 0; i < unvisited.Count; i++)
        {
            if (!cells[unvisited[i]].visited)
            {
                cells[unvisited[i]].clearAll();
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
        unvisited = new List<int>();
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
                unvisited.Add(cell_index);

                cells[cell_index].text = cells[cell_index].createText();
                cell_index++;
            }
        }
    }
      
    public void generateRoom(int method,float max_room_ratio,int tries, int minimum_created_rooms)
    {
        int max_size_rooms = (int)(this.size * max_room_ratio);

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
                rooms[i].setVisited(cells,unvisited);
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
            //checkConnectivity();

            operation.Stop();
            writetext.WriteLine("Operation took: " + operation.ElapsedMilliseconds + " milliseconds");
        }
        cmat(Resources.Load("Room") as Material);
        //generateMazeNoRooms();
    }

    public void fixCorridor()
    {
        for (int i = 0; i < corridor.Count; i++)
        {
            if (cells[corridor[i]].visited)
            {
                unvisited.Remove(cells[corridor[i]].index);
                cells[corridor[i]].checkNeighbor(cells);
            }
        }
    }

    public void cmat(Material mat)
    {
        for (int i = 0; i < unvisited.Count; i++)
        {
            cells[unvisited[i]].changeMaterial(mat);
        }
    }

    public void cmatCell(Material mat,int index)
    {
        cells[index].changeMaterial(mat);
    }

    public void checkConnectivity()
    {
        //0 4
        int start_room = UnityEngine.Random.Range(0, rooms.Count);
        //start_room = 0
        string output = "Connection check:\n";
        for (int i = 0; i < rooms.Count; i++)
            //for (int i = 0; i < 1; i++)
            {
            int start_cell = rooms[start_room].corner_cells[0].index;
            List<int> tested = new List<int>();
            Dictionary<int, int> to_test = new Dictionary<int, int>();
            to_test[start_cell]=0;
            int target_cell = -1;
            if (i == start_room)
            {
                continue;
            }
             
            //left     //1
            //right    //2
            //front    //3
            //back     //4
            target_cell = rooms[i].corner_cells[0].index;
            int[] sides = new int[] { 1, 0, 3, 2 };
            int run = 0;
            while (to_test.Count!=0 && start_cell!=target_cell)
            {
                int distance = 0;
                cmatCell(Resources.Load("Connection") as Material,start_cell);
                for (int j = 0; j < 4; j++)
                {
                    int n_index = cells[start_cell].neighbors[j];
                    bool side = false;
                    bool not_tested = false;
                    if (n_index != -1 && cells[n_index].visited)
                    {
                        if (cells[start_cell].side[j] == 0 && cells[cells[start_cell].neighbors[j]].side[sides[j]] == 0) { side = true; }
                        if (tested.Contains(cells[start_cell].neighbors[j]) == false) { not_tested = true; }
                        if (side && not_tested)
                        {
                            distance = (int)(Math.Abs(cells[cells[start_cell].neighbors[j]].x - cells[target_cell].x) + Math.Abs(cells[cells[start_cell].neighbors[j]].z - cells[target_cell].z));
                            to_test[cells[start_cell].neighbors[j]] = distance;
                            cells[cells[start_cell].neighbors[j]].text.GetComponent<TextMesh>().text = "Cost: " + (distance).ToString() + "\n";
                        }
                    }
                }
                tested.Add(start_cell);
                to_test.Remove(start_cell);
                if (to_test.Count != 0)
                {
                    int best = to_test.ElementAt(0).Value;
                    int best_min = 0;
                    for (int k = 0; k < to_test.Count; k++)
                    {
                        if (to_test.ElementAt(k).Value < best)
                        {
                            best = to_test.ElementAt(k).Value;
                            best_min = k;
                        }
                    }
                    start_cell = to_test.ElementAt(best_min).Key;
                }
                cells[start_cell].text.GetComponent<TextMesh>().text += "Step: " + run++;
                output +=to_test.Count+ "\n";
                if (to_test.Count==0)
                {
                    output += "No connection !\n";
                    rooms[start_room].makeConnection(cells, rooms[i], rooms[i].room_index, corridor);
                }
                if (start_cell==target_cell)
                {
                    output += "Found it!\n";
                }
            }
            output+=tested.Count+ "\n";

        }
        fixCorridor();
        using (StreamWriter wt = new StreamWriter("Debugs/room_generation_connect.txt"))
        {
            wt.Write(output);

        }
    }





    public void makeWalls()
    {
        for (int i = 0; i < unvisited.Count; i++)
        {
            cells[unvisited[i]].makeWalls();  
        }
    }
     
    public void generateMazeNoRooms(int max_runs = -1)
    {
        Stopwatch operation = new Stopwatch();
        operation.Start();
        string output = "Generation no Room\n";
        string prev = "Previous list:\n";
        makeWalls();
        int runs = 0;
        int current = -1;
        if (rooms.Count==0)
        {
            current = UnityEngine.Random.Range(0, unvisited.Count);
        }
        else
        {
            int room = UnityEngine.Random.Range(0, rooms.Count);
            int side = UnityEngine.Random.Range(0, 4);
            switch (side)
            {
                case 0:
                    try
                    {
                        current = rooms[room].left_cells[UnityEngine.Random.Range(0, rooms[room].left_cells.Count)];
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        return;
                    }


                    break;
                case 1:
                    try
                    {
                        current = rooms[room].right_cells[UnityEngine.Random.Range(0, rooms[room].right_cells.Count)];
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        return;
                    }
                    break;
                case 2:
                    try
                    {
                        current = rooms[room].top_cells[UnityEngine.Random.Range(0, rooms[room].top_cells.Count)];
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        return;
                    }
                    break;
                case 3:
                    try
                    {
                        current = rooms[room].bottom_cells[UnityEngine.Random.Range(0, rooms[room].bottom_cells.Count)];
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        return;
                    }
                    break;
            }
        }
        
        List<int> previous = new List<int>();
        previous.Add(current);
        bool not_finished = true;

        do
        {
            int next = UnityEngine.Random.Range(0, 4);
            bool not_found_next = true;
            List<int> available = new List<int>();
            do
            {
                for (int i = 0; i < 4; i++)
                {
                    if (cells[current].neighbors[i] != -1 && !cells[cells[current].neighbors[i]].visited)
                    {
                        available.Add(i);
                    }
                }
                if (available.Count!=0)
                {
                    next = available[UnityEngine.Random.Range(0, available.Count)];
                    not_found_next = false;
                }
                else
                {
                    previous.RemoveAt(previous.Count - 1);
                    output += "Previous count: " + previous.Count + "\n";
                    if (previous.Count == 0)
                    {
                        not_finished = false;
                        not_found_next = false;
                        break;
                    }
                    current = previous.Last();
                    prev += "Index: " + previous.Count + " : " + current + "\n";
                }
            } while (not_found_next);

            if (!not_finished)
            {
                break;
            }
            cells[current].clearWall(next+1);
            
            cells[current].visited=true;
            output += "Found new: " + current + "\n";
            output += "Current: " + current + " status: "+cells[current].visited + "\n";
            current = cells[cells[current].neighbors[next]].index;
            previous.Add(current);
            prev +="Index: "+previous.Count+" : "+ current + "\n";
            bool not_changed = true;
            if (next == 0) { next = 2; not_changed = false; }
            if (next == 1 && not_changed) { next = 1; not_changed = false; }
            if (next == 2 && not_changed) { next = 4; not_changed = false; }
            if (next == 3 && not_changed) { next = 3; not_changed = false; }
            cells[current].clearWall(next);
            cells[current].visited = true;
            output += "Current: " + current + " status: " + cells[current].visited + "\n";

            runs++;
            
            if (runs==max_runs)
            {
                break;
            }
        } while (not_finished);

        
        operation.Stop();
        output += "Runs: " + runs + "\n";
        output+="Operation took: " + operation.ElapsedMilliseconds + " milliseconds";
        using (StreamWriter wt = new StreamWriter("Debugs/room_generation_no_room.txt"))
        {
            wt.Write(output);
            
        }
        using (StreamWriter wt = new StreamWriter("Debugs/room_generation_prevlist.txt"))
        {
            wt.Write(prev);

        }
    }

}
