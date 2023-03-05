using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        floors = new FloorGenerator[100];
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

    // ********************************************************************************************************************
    //                                                      Debug only
    // ********************************************************************************************************************

    public void test(int start,int w,int h)
    {
        Stopwatch operation;

        string output = "<html>\n<head>\n<meta charset=\"UTF-8\">\n<title>Maze generator time</title>\n</head>\n<body>\n" +
            "<table border=\"1\" style=\"text-align: center\">\n    " +
            "<tr>\n    " +
            "<td>Width </td>\n    " +
            "<td>Height </td>\n    " +
            "<td width=\"80\">Grid size</td>\n    " +
            "<td width=\"110\">Generate grid </td>\n    " +
            "<td width=\"110\">Make rooms</td>\n    " +
            "<td width=\"150\">Check connectivity</td>\n    " +
            "<td width=\"120\">Gen no room </td>\n    " +
            "<td width=\"130\">Clear unvisited</td>\n    " +
            "<td width=\"100\">Fix corridor</td>\n    " +
            "<td width=\"130\">Time to generate</td>\n  " +
            "</tr>";

        int seed = 12345678;
        float min = 0.05f;
        float max = 0.2f;
        long elapsed = 0;
        long elapsedPerRow = 0;
        long grid = 0;
        long rooms = 0;
        long connect = 0;
        long gennoroom = 0;
        long unv = 0;
        long fix = 0;

        DateTime now = DateTime.Now;
        string filename = "Debugs/Test/test" + now.ToString("F") + ".html";
        using (StreamWriter wt = new StreamWriter(filename))
        {
            wt.Write(output);
        }
        output = "";
        int total = 0;
        for (int i = start; i < w; i++)
        {
            for (int j = start; j < h; j++)
            {
                operation = new Stopwatch();
                
                output += "<tr>" +
                    "<td>" + i + "</td>" +
                    "<td>" + j + "</td>" +
                    "<td>" + i * j + "</td>";

                clear();
                grid_width = i;
                grid_height = j;

                // generate roomgen ccon gennoroom clearnv fixcorridor
                // Width / Height / Grid size / Generate grid / Make rooms / Check connectivity / Gen no room / Clear unvisited / Fix corridor /  Time to generate

                operation.Start();
                generate(seed, min, max);
                operation.Stop();
                elapsedPerRow = operation.ElapsedMilliseconds;
                grid += operation.ElapsedMilliseconds;
                output += "<td>" + operation.ElapsedMilliseconds + " ms</td>";

                operation.Start();
                generateRooms(0);
                operation.Stop();
                elapsedPerRow += operation.ElapsedMilliseconds;
                rooms += operation.ElapsedMilliseconds;
                output += "<td>" + operation.ElapsedMilliseconds + " ms</td>";

                operation.Start();
                ccon();
                operation.Stop();
                elapsedPerRow += operation.ElapsedMilliseconds;
                connect += operation.ElapsedMilliseconds;
                output += "<td>" + operation.ElapsedMilliseconds + " ms</td>";

                operation.Start();
                generateMazeNoRooms();
                operation.Stop();
                elapsedPerRow += operation.ElapsedMilliseconds;
                gennoroom += operation.ElapsedMilliseconds;
                output += "<td>" + operation.ElapsedMilliseconds + " ms</td>";

                operation.Start();
                clearNotvisited();
                operation.Stop();
                elapsedPerRow += operation.ElapsedMilliseconds;
                unv += operation.ElapsedMilliseconds;
                output += "<td>" + operation.ElapsedMilliseconds + " ms</td>";

                operation.Start();
                fixc();
                operation.Stop();
                elapsedPerRow += operation.ElapsedMilliseconds;
                fix += operation.ElapsedMilliseconds;
                output += "<td>" + operation.ElapsedMilliseconds + " ms</td>";

                output += "<td>" + elapsedPerRow + " ms</td></tr>";
                elapsed += elapsedPerRow;
                using (StreamWriter writetext = File.AppendText(filename))
                {
                    writetext.WriteLine(output);
                }
                output = "";
                total++;
            }
        }

        float tgrid = grid / total;
        float troom = rooms / total;
        float tconn = connect / total;
        float tgnr = gennoroom / total;
        float tunv = unv / total;
        float tfix = fix / total;
        float avarageTime = elapsed / total;
        

        output = "<tr>" +
            "<td colspan=\"3\">Stats:</td>" +
            "<td>Avg grid</td>" +
            "<td>Avg room</td>" +
            "<td>Avg conn</td>" +
            "<td>Avg GNR</td>" +
            "<td>Avg UNV</td>" +
            "<td>Avg Fix</td>" +
            "<td>Avarage time</td>" +
            "</tr>";

        output += "<tr>" +
            "<td colspan=\"3\"></td>" +
            "<td>"+tgrid+"</td>" +
            "<td>"+troom+"</td>" +
            "<td>"+tconn+"</td>" +
            "<td>"+tgnr+"</td>" +
            "<td>"+tunv+"</td>" +
            "<td>"+tfix+"</td>" +
            "<td>" + avarageTime + " ms</td>" +
            "</tr>";

        output += "<tr>" +
            "<td colspan=\"5\">Total</td>" +
            "<td colspan=\"5\">Total time</td>" +
            "</tr>";

        int mins = 0;
        int secs = (int)elapsed / 1000;
        elapsed -= (secs * 1000);
        if (secs>59)
        {
            mins = secs / 60;
            secs -= (mins * 60);
        } 

        output += "<tr>" +
            "<td colspan=\"5\">" + total + "</td>" +
            "<td colspan=\"5\">"+ mins +" minutes "+secs+" secs "+elapsed + " milisecs</td>" +
            "</tr>";

        output += "</table>  \n</body>\n</html>";
        using (StreamWriter writetext = File.AppendText(filename))
        {
            writetext.WriteLine(output);
        }
    }

    public void testNoRoom(int start, int w, int h)
    {
        Stopwatch operation;

        string output = "<html>\n<head>\n<meta charset=\"UTF-8\">\n<title>Maze generator time</title>\n</head>\n<body>\n" +
            "<table border=\"1\" style=\"text-align: center\">\n    " +
            "<tr>\n    " +
            "<td>Width </td>\n    " +
            "<td>Height </td>\n    " +
            "<td width=\"80\">Grid size</td>\n    " +
            "<td width=\"110\">Generate grid </td>\n    " +
            "<td width=\"120\">Gen no room </td>\n    " +
            "<td width=\"130\">Time to generate</td>\n  " +
            "</tr>";

        int seed = 12345678;
        float min = 0.05f;
        float max = 0.2f;
        long elapsed = 0;
        long elapsedPerRow = 0;
        long grid = 0;
        long gennoroom = 0;

        DateTime now = DateTime.Now;
        string filename = "Debugs/Test/test no Room " + now.ToString("F") + ".html";
        using (StreamWriter wt = new StreamWriter(filename))
        {
            wt.Write(output);
        }
        output = "";
        int total = 0;
        for (int i = start; i < w; i++)
        {
            for (int j = start; j < h; j++)
            {
                operation = new Stopwatch();

                output += "<tr>" +
                    "<td>" + i + "</td>" +
                    "<td>" + j + "</td>" +
                    "<td>" + i * j + "</td>";

                clear();
                grid_width = i;
                grid_height = j;

                // generate roomgen ccon gennoroom clearnv fixcorridor
                // Width / Height / Grid size / Generate grid / Make rooms / Check connectivity / Gen no room / Clear unvisited / Fix corridor /  Time to generate

                operation.Start();
                generate(seed, min, max);
                operation.Stop();
                elapsedPerRow = operation.ElapsedMilliseconds;
                grid += operation.ElapsedMilliseconds;
                output += "<td>" + operation.ElapsedMilliseconds + " ms</td>";

                operation.Start();
                generateMazeNoRooms();
                operation.Stop();
                elapsedPerRow += operation.ElapsedMilliseconds;
                gennoroom += operation.ElapsedMilliseconds;
                output += "<td>" + operation.ElapsedMilliseconds + " ms</td>";


                output += "<td>" + elapsedPerRow + " ms</td></tr>";
                elapsed += elapsedPerRow;
                using (StreamWriter writetext = File.AppendText(filename))
                {
                    writetext.WriteLine(output);
                }
                output = "";
                total++;
            }
        }

        float tgrid = grid / total;
        float tgnr = gennoroom / total;
        float avarageTime = elapsed / total;


        output = "<tr>" +
            "<td colspan=\"3\">Stats:</td>" +
            "<td>Avg grid</td>" +
            "<td>Avg GNR</td>" +
            "<td>Avarage time</td>" +
            "</tr>";

        output += "<tr>" +
            "<td colspan=\"3\"></td>" +
            "<td>" + tgrid + "</td>" +
            "<td>" + tgnr + "</td>" +
            "<td>" + avarageTime + " ms</td>" +
            "</tr>";

        output += "<tr>" +
            "<td colspan=\"5\">Total</td>" +
            "<td colspan=\"5\">Total time</td>" +
            "</tr>";

        int mins = 0;
        int secs = (int)elapsed / 1000;
        elapsed -= (secs * 1000);
        if (secs > 59)
        {
            mins = secs / 60;
            secs -= (mins * 60);
        }

        output += "<tr>" +
            "<td colspan=\"5\">" + total + "</td>" +
            "<td colspan=\"5\">" + mins + " minutes " + secs + " secs " + elapsed + " milisecs</td>" +
            "</tr>";

        output += "</table>  \n</body>\n</html>";
        using (StreamWriter writetext = File.AppendText(filename))
        {
            writetext.WriteLine(output);
        }
    }

}