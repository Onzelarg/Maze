using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;
using static Room;

public class Room
{
    public List<int> left_cells = new List<int>();
    public List<int> right_cells = new List<int>();
    public List<int> top_cells = new List<int>();
    public List<int> bottom_cells = new List<int>();
    public corner_cell[] corner_cells = new corner_cell[4];

    public int room_size;
    public int room_width;
    public int room_height;
    public Dictionary<int, room_cell> room_cells = new Dictionary<int, room_cell>();
    public int cell_index;
    public int connected_room_index;
    public int room_index;

    public class room_cell
    {
        public Material material;
        public string type { get; set; }
    }

    public struct corner_cell
    {
        public int x;
        public int z;
        public int index;
        public bool isIndex;
    }


    public Room(int size, int w, int h)
    {
        this.room_size = size;
        this.room_width = w;
        this.room_height = h;
    }


    public void generate(int _cell_index, bool _x_positive, bool _z_positive, int created_rooms, Tiles[] cells)
    {
        //TopLeft      0
        //TopRight     1
        //BottomLeft   2
        //BottomRight  3
        int _floor_w = Variables.floor_width;
        this.cell_index = _cell_index;
        this.room_index = created_rooms;

        using (StreamWriter writetext = new StreamWriter("Debugs/room_cell_generation" + created_rooms + ".txt"))
        {
            if (_x_positive && _z_positive)
            {
                this.corner_cells[2].index = _cell_index;
                this.corner_cells[3].index = corner_cells[2].index + room_width - 1;
                this.corner_cells[0].index = corner_cells[2].index + ((room_height - 1) * _floor_w);
                this.corner_cells[1].index = corner_cells[0].index + room_width - 1;
                writetext.WriteLine("Cell index is bottom left ");
                corner_cells[2].isIndex = true;
            }
            else if (_x_positive && !_z_positive)
            {
                this.corner_cells[0].index = _cell_index;
                this.corner_cells[1].index = corner_cells[0].index + room_width - 1;
                this.corner_cells[2].index = corner_cells[0].index - ((room_height - 1) * _floor_w);
                this.corner_cells[3].index = corner_cells[2].index + room_width - 1;
                writetext.WriteLine("Cell index is top left ");
                corner_cells[0].isIndex = true;
            }
            else if (!_x_positive && _z_positive)
            {
                this.corner_cells[3].index = _cell_index;
                this.corner_cells[2].index = corner_cells[3].index - room_width + 1;
                this.corner_cells[0].index = corner_cells[2].index + ((room_height - 1) * _floor_w);
                this.corner_cells[1].index = corner_cells[0].index + room_width - 1;
                writetext.WriteLine("Cell index is bottom right ");
                corner_cells[3].isIndex = true;
            }
            else if (!_x_positive && !_z_positive)
            {
                this.corner_cells[1].index = _cell_index;
                this.corner_cells[0].index = corner_cells[1].index - room_width + 1;
                this.corner_cells[2].index = corner_cells[0].index - ((room_height - 1) * _floor_w);
                this.corner_cells[3].index = corner_cells[2].index + room_width - 1;
                writetext.WriteLine("Cell index is top right ");
                corner_cells[1].isIndex = true;
            }
            //TopLeft      0
            //TopRight     1
            //BottomLeft   2
            //BottomRight  3
            //if (_x_positive && _z_positive)
            //{
            //    this.bottomleft[0] = _cell_index;                                             2
            //    this.bottomright[0] = bottomleft[0] + room_width - 1;                         3=2
            //    this.topleft[0] = bottomleft[0] + ((room_height - 1) * _floor_w);             0=2
            //    this.topright[0] = topleft[0] + room_width - 1;                               1=0
            //    writetext.WriteLine("Cell index is bottom left ");
            //}
            //else if (_x_positive && !_z_positive)
            //{
            //    this.topleft[0] = _cell_index;                                                0
            //    this.topright[0] = topleft[0] + room_width - 1;                               1=0
            //    this.bottomleft[0] = topleft[0] - ((room_height - 1) * _floor_w);             2=0
            //    this.bottomright[0] = bottomleft[0] + room_width - 1;                         3=2
            //    writetext.WriteLine("Cell index is top left ");
            //}
            //else if (!_x_positive && _z_positive)
            //{
            //    this.bottomright[0] = _cell_index;                                            3
            //    this.bottomleft[0] = bottomright[0] - room_width + 1;                         2=3
            //    this.topleft[0] = bottomleft[0] + ((room_height - 1) * _floor_w);             0=2
            //    this.topright[0] = topleft[0] + room_width - 1;                               1=0
            //    writetext.WriteLine("Cell index is bottom right ");
            //}
            //else if (!_x_positive && !_z_positive)
            //{
            //    this.topright[0] = _cell_index;                                               1
            //    this.topleft[0] = topright[0] - room_width + 1;                               0=1
            //    this.bottomleft[0] = topleft[0] - ((room_height - 1) * _floor_w);             2=0
            //    this.bottomright[0] = bottomleft[0] + room_width - 1;                         3=2
            //    writetext.WriteLine("Cell index is top right ");

            //}

            corner_cells[0].x = (int)(cells[corner_cells[0].index].x);
            corner_cells[0].z = (int)(cells[corner_cells[0].index].z);
            corner_cells[1].x = (int)(cells[corner_cells[1].index].x);
            corner_cells[1].z = (int)(cells[corner_cells[1].index].z);
            corner_cells[2].x = (int)(cells[corner_cells[2].index].x);
            corner_cells[2].z = (int)(cells[corner_cells[2].index].z);
            corner_cells[3].x = (int)(cells[corner_cells[3].index].x);
            corner_cells[3].z = (int)(cells[corner_cells[3].index].z);

            writetext.WriteLine("Cell index: " + _cell_index);
            writetext.WriteLine("Top Left: " + corner_cells[0].index + " X: " + corner_cells[0].x + " Z: " + corner_cells[0].z);
            writetext.WriteLine("Top Right: " + corner_cells[1].index + " X: " + corner_cells[1].x + " Z: " + corner_cells[1].z);
            writetext.WriteLine("Bottom Left: " + corner_cells[2].index + " X: " + corner_cells[2].x + " Z: " + corner_cells[2].z);
            writetext.WriteLine("Bottom Right: " + corner_cells[3].index + " X: " + corner_cells[3].x + " Z: " + corner_cells[3].z);
            writetext.WriteLine("Room width: " + room_width);
            writetext.WriteLine("Room height: " + room_height);
            writetext.WriteLine("X positive: " + _x_positive);
            writetext.WriteLine("Z positive: " + _z_positive);
            writetext.WriteLine("Cells: ");
        }
    }

    public void setMaterial(int _floor_w, Tiles[] cells, int created_rooms)
    {
        using (StreamWriter wt = File.AppendText("Debugs/room_cell_generation" + created_rooms + ".txt"))
        {

            wt.WriteLine(" ");
            int index = 0;
            for (int z = 0; z < room_height; z++)
            {
                for (int x = 0; x < room_width; x++)
                {
                    room_cell cell = new room_cell();
                    if ((corner_cells[2].index + (z * _floor_w) + x) == cell_index)
                    {
                        cell.material = Resources.Load("Index") as Material;
                    }
                    else if (z == 0 || z == room_height - 1 || x == 0 || x == room_width - 1)
                    {
                        cell.material = Resources.Load("Corner") as Material;
                        cell.type = "corner";
                    }
                    else
                    {
                        cell.material = Resources.Load("Room") as Material;
                        cell.type = "inside";
                    }
                    // Sides
                    if (z == 0 && (x != 0 && x != room_width - 1))
                    {
                        this.bottom_cells.Add((corner_cells[2].index + (z * _floor_w) + x));
                        cell.type = "side";
                    }

                    if (z == room_height - 1 && (x != 0 && x != room_width - 1))
                    {
                        this.top_cells.Add((corner_cells[2].index + (z * _floor_w) + x));
                        cell.type = "side";
                    }

                    if (x == 0 && (z != 0 && z != room_height - 1))
                    {
                        this.left_cells.Add((corner_cells[2].index + (z * _floor_w) + x));
                        cell.type = "side";
                    }

                    if (x == room_width - 1 && (z != 0 && z != room_height - 1))
                    {
                        this.right_cells.Add((corner_cells[2].index + (z * _floor_w) + x));
                        cell.type = "side";
                    }

                    room_cells[(corner_cells[2].index + (z * _floor_w) + x)] = cell;
                    cells[(corner_cells[2].index + (z * _floor_w) + x)].visited = true;
                    wt.Write(room_cells.ElementAt(index).Key);
                    wt.WriteLine(" : " + room_cells.ElementAt(index).Value.material);
                    index++;
                }
            }
            wt.WriteLine("Corner Cells: (" + corner_cells.Length + ")");
            for (int i = 0; i < corner_cells.Length; i++)
            {
                wt.WriteLine(corner_cells[i].index);
            }
            wt.WriteLine("Left Cells: (" + left_cells.Count + ")");
            for (int i = 0; i < left_cells.Count; i++)
            {
                wt.WriteLine(left_cells[i]);
            }
            wt.WriteLine("Right Cells: (" + right_cells.Count + ")");
            for (int i = 0; i < right_cells.Count; i++)
            {
                wt.WriteLine(right_cells[i]);
            }
            wt.WriteLine("Top Cells: (" + top_cells.Count + ")");
            for (int i = 0; i < top_cells.Count; i++)
            {
                wt.WriteLine(top_cells[i]);
            }
            wt.WriteLine("Bottom Cells: (" + bottom_cells.Count + ")");
            for (int i = 0; i < bottom_cells.Count; i++)
            {
                wt.WriteLine(bottom_cells[i]);
            }
        }
    }

    public void setTileSides(Tiles[] cells)
    {
        int[] sides = new int[4];
        GameObject wall = Resources.Load("Wall", typeof(GameObject)) as GameObject;

        for (int i = 0; i < room_cells.Count; i++)
        {
            if (room_cells.ElementAt(i).Value.type == "inside")
            {
                sides = new int[] { 0, 0, 0, 0 };
                cells[room_cells.ElementAt(i).Key].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
            }
        }

        //left     //1

        for (int i = 0; i < left_cells.Count; i++)
        {
            if (room_cells[left_cells[i]].type != "inside")
            {
                sides = new int[] { 1, 0, 0, 0 };
                cells[left_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
            }
            else
            {
                sides = new int[] { 0, 0, 0, 0 };
                cells[left_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
            }
        }
        //right    //2

        for (int i = 0; i < right_cells.Count; i++)
        {
            if (room_cells[right_cells[i]].type != "inside")
            {
                sides = new int[] { 0, 1, 0, 0 };
                cells[right_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
            }
            else
            {
                sides = new int[] { 0, 0, 0, 0 };
                cells[right_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
            }
        }
        //front    //3

        for (int i = 0; i < top_cells.Count; i++)
        {
            if (room_cells[top_cells[i]].type != "inside")
            {
                sides = new int[] { 0, 0, 0, 1 };
                cells[top_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
            }
            else
            {
                sides = new int[] { 0, 0, 0, 0 };
                cells[top_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
            }
        }
        //back     //4

        for (int i = 0; i < bottom_cells.Count; i++)
        {
            if (room_cells[bottom_cells[i]].type != "inside")
            {
                sides = new int[] { 0, 0, 1, 0 };
                cells[bottom_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
            }
            else
            {
                sides = new int[] { 0, 0, 0, 0 };
                cells[bottom_cells[i]].generateSide(sides, wall, Variables.tile_size, Variables.tile_scale);
            }
        }
        //TL TR BL BR
        cells[corner_cells[0].index].generateSide(new int[] { 1, 0, 0, 1 }, wall, Variables.tile_size, Variables.tile_scale);
        cells[corner_cells[1].index].generateSide(new int[] { 0, 1, 0, 1 }, wall, Variables.tile_size, Variables.tile_scale);
        cells[corner_cells[2].index].generateSide(new int[] { 1, 0, 1, 0 }, wall, Variables.tile_size, Variables.tile_scale);
        cells[corner_cells[3].index].generateSide(new int[] { 0, 1, 1, 0 }, wall, Variables.tile_size, Variables.tile_scale);
    }

    public void changeMaterial(Tiles[] cells)
    {
        for (int i = 0; i < (room_cells.Count); i++)
        {

            cells[room_cells.ElementAt(i).Key].changeMaterial(room_cells.ElementAt(i).Value.material);
        }
    }

    public bool methodCheck(int method, Room other, float tilesize)
    {
        using (StreamWriter writetext = File.AppendText("Debugs/method/room_cell_method.txt"))
        {
            //room_cells[corner_cells[3]]
            writetext.WriteLine("");
            writetext.Write("This index: " + this.cell_index + " Other index:" + other.cell_index + " TTL X: " + this.corner_cells[0].x + " TTL Y: " + this.corner_cells[0].z);
            writetext.Write(" OTL X: " + other.corner_cells[0].x + " OTL Y: " + other.corner_cells[0].z);
            writetext.Write(" TBR X: " + this.corner_cells[3].x + " TBR Y: " + this.corner_cells[3].z);
            writetext.Write(" OBR X: " + other.corner_cells[3].x + " OBR Y: " + other.corner_cells[3].z);


            int offset = (int)(tilesize);
            if (method == 2)
            {
                if (this.corner_cells[0].x >= (other.corner_cells[3].x + offset) || this.corner_cells[3].x <= (other.corner_cells[0].x - offset) || this.corner_cells[0].z <= (other.corner_cells[3].z - offset) || this.corner_cells[3].z >= (other.corner_cells[0].z + offset))
                {
                    writetext.Write(" true");
                    return true;
                }
            }

            if (method == 3)
            {
                offset = offset * 2;
                if (this.corner_cells[0].x > (other.corner_cells[3].x + offset) || this.corner_cells[3].x < (other.corner_cells[0].x - offset) || this.corner_cells[0].z < (other.corner_cells[3].z - offset) || this.corner_cells[3].z > (other.corner_cells[0].z + offset))
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
        DateTime now = DateTime.Now;

        using (StreamWriter wt = new StreamWriter("Debugs/merged_mat/before " + cell_index +" : "+now.ToString("F")+ ".txt"))
        {
            wt.WriteLine("This room: ");
            wt.WriteLine(" ");
            for (int i = 0; i < room_cells.Count; i++)
            {
                wt.WriteLine("Cell index: " + room_cells.ElementAt(i).Key + " type: " + room_cells.ElementAt(i).Value.type);
            }
            wt.WriteLine("Other room: ");
            wt.WriteLine(" ");
            for (int i = 0; i < other.room_cells.Count; i++)
            {
                wt.WriteLine("Cell index: " + other.room_cells.ElementAt(i).Key + " type: " + other.room_cells.ElementAt(i).Value.type);
            }
        }
            for (int i = 0; i < room_cells.Count; i++)
            {
                for (int j = 0; j < other.room_cells.Count; j++)
                {
                    if (room_cells.ElementAt(i).Key == other.room_cells.ElementAt(j).Key)
                    {
                        if (room_cells.ElementAt(i).Value.type == "side" || room_cells.ElementAt(i).Value.type == "corner")
                        {
                            room_cells.ElementAt(i).Value.type = "inside";
                            room_cells.ElementAt(i).Value.material = Resources.Load("Room") as Material;
                        }
                        if (other.room_cells.ElementAt(j).Value.type == "side" || other.room_cells.ElementAt(j).Value.type == "corner")
                        {
                            other.room_cells.ElementAt(j).Value.type = "inside";
                            other.room_cells.ElementAt(j).Value.material = Resources.Load("Room") as Material;
                        }
                    }
                }
            }
            using (StreamWriter wt = new StreamWriter("Debugs/merged_mat/after " + cell_index + " : " + now.ToString("F") + ".txt"))
            {
                wt.WriteLine("This room: ");
                wt.WriteLine(" ");
                for (int i = 0; i < room_cells.Count; i++)
                {
                    wt.WriteLine("Cell index: " + room_cells.ElementAt(i).Key + " type: " + room_cells.ElementAt(i).Value.type);
                }
                wt.WriteLine("Other room: ");
                wt.WriteLine(" ");
                for (int i = 0; i < other.room_cells.Count; i++)
                {
                    wt.WriteLine("Cell index: " + other.room_cells.ElementAt(i).Key + " type: " + other.room_cells.ElementAt(i).Value.type);
                }
            }
        }
}