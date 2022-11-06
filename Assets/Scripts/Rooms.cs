using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;

public class Room
{
    public int[] topleft = new int[3];
    public int[] topright = new int[3];
    public int[] bottomleft = new int[3];
    public int[] bottomright = new int[3];
    public List<int> left_cells = new List<int>();
    public List<int> right_cells = new List<int>();
    public List<int> top_cells = new List<int>();
    public List<int> bottom_cells = new List<int>();
    public List<int> corner_cells = new List<int>();
    public List<int> inside_cells = new List<int>();

    public int room_size;
    public int room_width;
    public int room_height;
    public Dictionary<int, Material> room_cells = new Dictionary<int, Material>();
    public int cell_index;
    public int connected_room_index;
    public int room_index;

    struct room_cell
    {
        Material material;



    }



public Room(int size, int w, int h)
    {
        this.room_size = size;
        this.room_width = w;
        this.room_height = h;
    }


    public void generate(int _cell_index, bool _x_positive, bool _z_positive, int created_rooms, Tiles[] cells)
    {
        int _floor_w = Variables.floor_width;
        this.cell_index = _cell_index;
        this.room_index = created_rooms;
        using (StreamWriter writetext = new StreamWriter("Debugs/room_cell_generation" + created_rooms + ".txt"))
        {
            if (_x_positive && _z_positive)
            {
                this.bottomleft[0] = _cell_index;
                this.bottomright[0] = bottomleft[0] + room_width - 1;
                this.topleft[0] = bottomleft[0] + ((room_height - 1) * _floor_w);
                this.topright[0] = topleft[0] + room_width - 1;
                writetext.WriteLine("Cell index is bottom left ");
            }
            else if (_x_positive && !_z_positive)
            {
                this.topleft[0] = _cell_index;
                this.topright[0] = topleft[0] + room_width - 1;
                this.bottomleft[0] = topleft[0] - ((room_height - 1) * _floor_w);
                this.bottomright[0] = bottomleft[0] + room_width - 1;
                writetext.WriteLine("Cell index is top left ");
            }
            else if (!_x_positive && _z_positive)
            {
                this.bottomright[0] = _cell_index;
                this.bottomleft[0] = bottomright[0] - room_width + 1;
                this.topleft[0] = bottomleft[0] + ((room_height - 1) * _floor_w);
                this.topright[0] = topleft[0] + room_width - 1;
                writetext.WriteLine("Cell index is bottom right ");
            }
            else if (!_x_positive && !_z_positive)
            {
                this.topright[0] = _cell_index;
                this.topleft[0] = topright[0] - room_width + 1;
                this.bottomleft[0] = topleft[0] - ((room_height - 1) * _floor_w);
                this.bottomright[0] = bottomleft[0] + room_width - 1;
                writetext.WriteLine("Cell index is top right ");

            }

            this.topleft[1] = (int)(cells[this.topleft[0]].x);
            this.topleft[2] = (int)(cells[this.topleft[0]].z);
            this.topright[1] = (int)(cells[this.topright[0]].x);
            this.topright[2] = (int)(cells[this.topright[0]].z);
            this.bottomleft[1] = (int)(cells[this.bottomleft[0]].x);
            this.bottomleft[2] = (int)(cells[this.bottomleft[0]].z);
            this.bottomright[1] = (int)(cells[this.bottomright[0]].x);
            this.bottomright[2] = (int)(cells[this.bottomright[0]].z);
            this.corner_cells.Add(topleft[0]);
            this.corner_cells.Add(topright[0]);
            this.corner_cells.Add(bottomleft[0]);
            this.corner_cells.Add(bottomright[0]);
            
            writetext.WriteLine("Cell index: " + _cell_index);
            writetext.WriteLine("Top Left: " + topleft[0]+" X: " + topleft[1] +" Z: " + topleft[2]);
            writetext.WriteLine("Top Right: " + topright[0] + " X: " + topright[1] +" Z: " + topright[2]);
            writetext.WriteLine("Bottom Left: " + bottomleft[0] + " X: " + bottomleft[1] +" Z: " + bottomleft[2]);
            writetext.WriteLine("Bottom Right: " + bottomright[0] + " X: " + bottomright[1] +" Z: " + bottomright[2]);
            writetext.WriteLine("Room width: " + room_width);
            writetext.WriteLine("Room height: " + room_height);
            writetext.WriteLine("X positive: " + _x_positive);
            writetext.WriteLine("Z positive: " + _z_positive);
            writetext.WriteLine("Cells: ");    
        }
    }

    public void setMaterial(int _floor_w, Tiles[] cells,StreamWriter wt)
    {
        
        wt.WriteLine(" ");
        int index = 0;
        for (int z = 0; z < room_height; z++)
        {
            for (int x = 0; x < room_width; x++)
            {
                if ((bottomleft[0] + (z * _floor_w) + x) == cell_index)
                {
                    room_cells[(bottomleft[0] + (z * _floor_w) + x)] = Resources.Load("Index") as Material;
                    inside_cells.Add((bottomleft[0] + (z * _floor_w) + x));
                }
                else if (z == 0 || z == room_height - 1 || x == 0 || x == room_width - 1)
                {
                    room_cells[(bottomleft[0] + (z * _floor_w) + x)] = Resources.Load("Corner") as Material;
                }
                else
                {
                    room_cells[(bottomleft[0] + (z * _floor_w) + x)] = Resources.Load("Room") as Material;
                }

                // Sides
                if (z==0 && (x!=0 && x!= room_width-1))
                {
                    this.bottom_cells.Add((bottomleft[0] + (z * _floor_w) + x));
                }

                if (z == room_height - 1 && (x != 0 && x != room_width - 1))
                {
                    this.top_cells.Add((bottomleft[0] + (z * _floor_w) + x));
                }

                if (x==0 && (z!=0 && z != room_height - 1))
                {
                    this.left_cells.Add((bottomleft[0] + (z * _floor_w) + x));
                }

                if (x == room_width - 1 && (z != 0 && z != room_height - 1))
                {
                    this.right_cells.Add((bottomleft[0] + (z * _floor_w) + x));
                }

                cells[(bottomleft[0] + (z * _floor_w) + x)].visited = true;
                wt.Write(room_cells.ElementAt(index).Key);
                wt.WriteLine(" : "+room_cells.ElementAt(index).Value);
                index++;
            }
        }
        wt.WriteLine("Corner Cells: (" + corner_cells.Count + ")");
        for (int i = 0; i < corner_cells.Count; i++)
        {
            wt.WriteLine(corner_cells[i]);
        }
        wt.WriteLine("Left Cells: ("+left_cells.Count+")");
        for (int i = 0; i < left_cells.Count; i++)
        {
            wt.WriteLine(left_cells[i]);
        }
        wt.WriteLine("Right Cells: ("+right_cells.Count+")");
        for (int i = 0; i < right_cells.Count; i++)
        {
            wt.WriteLine(right_cells[i]);
        }
        wt.WriteLine("Top Cells: ("+top_cells.Count+")");
        for (int i = 0; i < top_cells.Count; i++)
        {
            wt.WriteLine(top_cells[i]);
        }
        wt.WriteLine("Bottom Cells: ("+bottom_cells.Count+")");
        for (int i = 0; i < bottom_cells.Count; i++)
        {
            wt.WriteLine(bottom_cells[i]);
        }
    }

    void setTileSides(Tiles[] cells)
    {
        int[] sides = { 0, 0, 0, 0 };
        GameObject wall = Resources.Load("Wall",typeof(GameObject)) as GameObject;

        for (int i = 0; i < inside_cells.Count; i++)
        {
            cells[inside_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
        }
        //left     //1
        sides = new int[] { 1, 0, 0, 0 };
        for (int i = 0; i < left_cells.Count; i++)
        {
            cells[left_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
        }
        //right    //2
        sides = new int[] { 0, 1, 0, 0 };
        for (int i = 0; i < right_cells.Count; i++)
        {
            cells[right_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
        }
        //front    //3
        sides = new int[] { 0, 0, 0, 1 };
        for (int i = 0; i < top_cells.Count; i++)
        {
            cells[top_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
        }
        //back     //4
        sides = new int[] { 0, 0, 1, 0 };
        for (int i = 0; i < bottom_cells.Count; i++)
        {
            cells[bottom_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
        }
        //TL TR BL BR
        cells[corner_cells[0]].generateSide(new int[] { 1, 0, 0, 1 }, wall, Variables.tile_size, Variables.tile_scale);
        cells[corner_cells[1]].generateSide(new int[] { 0, 1, 0, 1 }, wall, Variables.tile_size, Variables.tile_scale);
        cells[corner_cells[2]].generateSide(new int[] { 1, 0, 1, 0 }, wall, Variables.tile_size, Variables.tile_scale);
        cells[corner_cells[3]].generateSide(new int[] { 0, 1, 1, 0 }, wall, Variables.tile_size, Variables.tile_scale);
    }

    public void changeMaterial(Tiles[] cells)
    {
        for (int i = 0; i < (room_cells.Count); i++)
        {
            cells[room_cells.ElementAt(i).Key].changeMaterial(room_cells.ElementAt(i).Value);
        }            
    } 

    public bool methodCheck(int method,Room other,float tilesize)
    {
        using (StreamWriter writetext = File.AppendText("Debugs/method/room_cell_method.txt"))
        {

            writetext.WriteLine("");
            writetext.Write("This index: " + this.cell_index + " Other index:" + other.cell_index + " TTL X: " + this.topleft[1] + " TTL Y: " + this.topleft[2]);
            writetext.Write(" OTL X: " + other.topleft[1] + " OTL Y: " + other.topleft[2]);
            writetext.Write(" TBR X: " + this.bottomright[1] + " TBR Y: " + this.bottomright[2]);
            writetext.Write(" OBR X: " + other.bottomright[1] + " OBR Y: " + other.bottomright[2]);
            
            
            int offset = (int)(tilesize);
            if (method == 2)
            {
                if (this.topleft[1] >= (other.bottomright[1] + offset) || this.bottomright[1] <= (other.topleft[1] - offset) || this.topleft[2] <= (other.bottomright[2] - offset) || this.bottomright[2] >= (other.topleft[2] + offset))
                {
                    writetext.Write(" true");
                    return true;
                }
            }
              
            if (method == 3)
            {
                offset = offset*2;
                if (this.topleft[1] > (other.bottomright[1] + offset) || this.bottomright[1] < (other.topleft[1] - offset) || this.topleft[2] < (other.bottomright[2] - offset) || this.bottomright[2] > (other.topleft[2] + offset))
                { 
                    writetext.Write(" true");
                    return true;
                }
            }

            writetext.Write(" false");
            return false;
        }
    }

    public void mergeTiles(Room other)
    {
        List<int> match = new List<int>();
        for (int i = 0; i < room_cells.Count; i++)
        {
            for (int j = 0; j < other.room_cells.Count; j++)
            {
                if (room_cells.ElementAt(i).Key==other.room_cells.ElementAt(j).Key)
                {
                    match.Add(i);
                }
            }
        }



    }

}