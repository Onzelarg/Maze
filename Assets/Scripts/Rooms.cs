using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;

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
    float uniqueness = 1f;
    public List<int> connected_rooms = new List<int>();

    public class room_cell
    {
        public Material material;
        public bool inside = true;
        public int[] sides = new int[] { 0, 0, 0, 0 };
    }

    public struct corner_cell
    {
        public Vector2 position;
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


            corner_cells[0].position.x = (int)(cells[corner_cells[0].index].x);
            corner_cells[0].position.y = (int)(cells[corner_cells[0].index].z);
            corner_cells[1].position.x = (int)(cells[corner_cells[1].index].x);
            corner_cells[1].position.y = (int)(cells[corner_cells[1].index].z);
            corner_cells[2].position.x = (int)(cells[corner_cells[2].index].x);
            corner_cells[2].position.y = (int)(cells[corner_cells[2].index].z);
            corner_cells[3].position.x = (int)(cells[corner_cells[3].index].x);
            corner_cells[3].position.y = (int)(cells[corner_cells[3].index].z);

            writetext.WriteLine("Cell index: " + _cell_index);
            writetext.WriteLine("Top Left: " + corner_cells[0].index + " X: " + corner_cells[0].position.x + " Z: " + corner_cells[0].position.y);
            writetext.WriteLine("Top Right: " + corner_cells[1].index + " X: " + corner_cells[1].position.x + " Z: " + corner_cells[1].position.y);
            writetext.WriteLine("Bottom Left: " + corner_cells[2].index + " X: " + corner_cells[2].position.x + " Z: " + corner_cells[2].position.y);
            writetext.WriteLine("Bottom Right: " + corner_cells[3].index + " X: " + corner_cells[3].position.x + " Z: " + corner_cells[3].position.y);
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
                        cell.inside = false;
                    }
                    else if (z == 0 || z == room_height - 1 || x == 0 || x == room_width - 1)
                    {
                        cell.material = Resources.Load("Corner") as Material;
                        cell.inside = false;
                    }
                    else
                    {
                        cell.material = Resources.Load("Room") as Material;
                    }
                    // Sides
                    if (z == 0 && (x != 0 && x != room_width - 1))
                    {
                        this.bottom_cells.Add((corner_cells[2].index + (z * _floor_w) + x));
                        cell.inside = false;
                        cell.sides = new int[] { 0, 0, 1, 0 };
                    }

                    if (z == room_height - 1 && (x != 0 && x != room_width - 1))
                    {
                        this.top_cells.Add((corner_cells[2].index + (z * _floor_w) + x));
                        cell.inside = false;
                        cell.sides = new int[] { 0, 0, 0, 1 };
                    }

                    if (x == 0 && (z != 0 && z != room_height - 1))
                    {
                        this.left_cells.Add((corner_cells[2].index + (z * _floor_w) + x));
                        cell.inside = false;
                        cell.sides = new int[] { 1, 0, 0, 0 };
                    }

                    if (x == room_width - 1 && (z != 0 && z != room_height - 1))
                    {
                        this.right_cells.Add((corner_cells[2].index + (z * _floor_w) + x));
                        cell.inside = false;
                        cell.sides = new int[] { 0, 1, 0, 0 };
                    }

                    room_cells[(corner_cells[2].index + (z * _floor_w) + x)] = cell;
                    wt.Write(room_cells.ElementAt(index).Key);
                    wt.WriteLine(" : " + room_cells.ElementAt(index).Value.material);
                    index++;
                }
            }

            room_cells[corner_cells[0].index].sides = new int[] { 1, 0, 0, 1 };
            room_cells[corner_cells[1].index].sides = new int[] { 0, 1, 0, 1 };
            room_cells[corner_cells[2].index].sides = new int[] { 1, 0, 1, 0 };
            room_cells[corner_cells[3].index].sides = new int[] { 0, 1, 1, 0 };

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
        GameObject wall = Resources.Load("Wall", typeof(GameObject)) as GameObject;
        for (int i = 0; i < room_cells.Count; i++)
        {
            if (!room_cells.ElementAt(i).Value.inside)
            {
                cells[room_cells.ElementAt(i).Key].generateSide(room_cells.ElementAt(i).Value.sides, wall);
            }
        }
    }

    public void changeMaterial(Tiles[] cells)
    {
        for (int i = 0; i < (room_cells.Count); i++)
        {

            cells[room_cells.ElementAt(i).Key].changeMaterial(room_cells.ElementAt(i).Value.material);
        }
    }

    public void cekMAt(Tiles[] cells)
    {
        Material material= Resources.Load("cek") as Material;
        Color color = Color.white;

        int unixTime = (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        System.Random random = new System.Random(unixTime);
        
        color.r = (float)random.NextDouble();
        color.g = (float)random.NextDouble();
        color.b = (float)random.NextDouble();
        material.color = color;
        for (int i = 0; i < (room_cells.Count); i++)
        {

            cells[room_cells.ElementAt(i).Key].changeMaterial(material);
        }

    }

    public bool methodCheck(int method, Room other, float tilesize)
    {
        using (StreamWriter writetext = File.AppendText("Debugs/method/room_cell_method.txt"))
        {
            writetext.WriteLine("");
            writetext.Write("This index: " + this.cell_index + " Other index:" + other.cell_index + " TTL X: " + this.corner_cells[0].position.x + " TTL Y: " + this.corner_cells[0].position.y);
            writetext.Write(" OTL X: " + other.corner_cells[0].position.x + " OTL Y: " + other.corner_cells[0].position.y);
            writetext.Write(" TBR X: " + this.corner_cells[3].position.x + " TBR Y: " + this.corner_cells[3].position.y);
            writetext.Write(" OBR X: " + other.corner_cells[3].position.x + " OBR Y: " + other.corner_cells[3].position.y);


            int offset = (int)(tilesize);
            if (method == 2)
            {
                if (this.corner_cells[0].position.x >= (other.corner_cells[3].position.x + offset) || this.corner_cells[3].position.x <= (other.corner_cells[0].position.x - offset) || this.corner_cells[0].position.y <= (other.corner_cells[3].position.y - offset) || this.corner_cells[3].position.y >= (other.corner_cells[0].position.y + offset))
                {
                    writetext.Write(" true");
                    return true;
                }
            }

            if (method == 3)
            {
                offset = offset * 2;
                if (this.corner_cells[0].position.x > (other.corner_cells[3].position.x + offset) || this.corner_cells[3].position.x < (other.corner_cells[0].position.x - offset) || this.corner_cells[0].position.y < (other.corner_cells[3].position.y - offset) || this.corner_cells[3].position.y > (other.corner_cells[0].position.y + offset))
                {
                    writetext.Write(" true");
                    return true;
                }
            }

            writetext.Write(" false");
            return false;
        }
    }

    public bool checkUniqueness(Room other)
    {
            for (int i = 0; i < room_cells.Count; i++)
            {
                for (int j = 0; j < other.room_cells.Count; j++)
                {
                    if (room_cells.ElementAt(i).Key == other.room_cells.ElementAt(j).Key)
                    {
                        uniqueness -= (1f / this.room_cells.Count);
                        if (uniqueness < 0.7f)
                        {
                            return false;
                        }
                    }
                }
            }
        return true;
    }

    public void makePartofRoom(Tiles[] cells)
    {
        for (int i = 0; i < room_cells.Count; i++)
        {
            cells[room_cells.ElementAt(i).Key].is_partofRoom = true;
        }
    }

    public void mergeTiles(Tiles[] cells, Room other,int counter,int room_id,int oroom_id)
    {
        DateTime now = DateTime.Now;
        bool connected = false;

        using (StreamWriter wt = new StreamWriter("Debugs/merged_mat/before " + counter +" .txt"))
        {
            wt.WriteLine("This room: "+cell_index+" room id: "+room_id);
            wt.WriteLine(" ");
            for (int i = 0; i < room_cells.Count; i++)
            {
                wt.WriteLine("Cell index: " + room_cells.ElementAt(i).Key + " type: ");
                if (room_cells.ElementAt(i).Value.inside)
                {
                    wt.Write("inside ");
                }
                else
                {
                    wt.Write("not inside ");
                }
            }
            wt.WriteLine("Other room: "+other.cell_index + " room id: " + oroom_id);
            wt.WriteLine(" ");
            for (int i = 0; i < other.room_cells.Count; i++)
            {
                wt.WriteLine("Cell index: " + other.room_cells.ElementAt(i).Key + " type: ");
                if (other.room_cells.ElementAt(i).Value.inside)
                {
                    wt.Write("inside ");
                }
                else
                {
                    wt.Write("not inside ");
                }
            }
            wt.WriteLine("Changing: ");
            for (int i = 0; i < room_cells.Count; i++)
            {
                for (int j = 0; j < other.room_cells.Count; j++)
                {
                    if (room_cells.ElementAt(i).Key == other.room_cells.ElementAt(j).Key)
                    {
                        connected = true;
                        wt.WriteLine(uniqueness);
                        if (!room_cells.ElementAt(i).Value.inside)
                        {
                            room_cells.ElementAt(i).Value.inside = true;
                            room_cells.ElementAt(i).Value.material = Resources.Load("Room") as Material;
                            wt.WriteLine("This: id: " + room_cells.ElementAt(i).Key + " type: inside");
                        }
                        if (!other.room_cells.ElementAt(j).Value.inside)
                        {
                            other.room_cells.ElementAt(j).Value.inside = true;
                            other.room_cells.ElementAt(j).Value.material = Resources.Load("Room") as Material;
                            wt.WriteLine("Other: id: " + other.room_cells.ElementAt(j).Key + " type: inside");
                        }
                    }
                }
            }
            if (connected)
            {
                connected_rooms.Add(oroom_id);
                other.connected_rooms.Add(room_id);
            }
        }
    }

    public void setVisited(Tiles[] cell)
    {
        for (int i = 0; i < room_cells.Count; i++)
        {
            cell[room_cells.ElementAt(i).Key].visited = true;
        }

    }

    public void makeConnection(Tiles[] cells,Room other,int oroom_id,List<int> corridor)
    {
        using (StreamWriter wt = new StreamWriter("Debugs/connection/connection" + cell_index + ".txt"))
        {
            connected_rooms.Add(oroom_id);
            int[] start = new int[3];
            int[] target = new int[3];
            start[0] = room_cells.ElementAt((UnityEngine.Random.Range(0, room_cells.Count))).Key;
            target[0] = other.room_cells.ElementAt((UnityEngine.Random.Range(0, other.room_cells.Count))).Key;
            start[1] = (int)cells[start[0]].x;
            start[2] = (int)cells[start[0]].z;
            target[1] = (int)cells[target[0]].x;
            target[2] = (int)cells[target[0]].z;
            corridor.Add(start[0]);
            wt.WriteLine("Start index: " + start[0]+" target :" + target[0]);
            while (start[1] != target[1])
            {
                if (start[1] > target[1])
                {
                    start[0]--;
                }
                else
                {
                    start[0]++;
                }
                corridor.Add(start[0]);
                start[1] = (int)cells[start[0]].x;
                wt.WriteLine("Index: " + start[0] + " part of room: " + cells[start[0]].is_partofRoom);
                if (!cells[start[0]].is_partofRoom)
                {
                    cells[start[0]].generateSide(new int[] { 0, 0, 1, 1 }, Resources.Load("Wall") as GameObject);
                    cells[start[0]].changeMaterial(Resources.Load("Connection") as Material);
                    cells[start[0]].visited = true;
                }              
            }
            while (start[2] != target[2])
            {
                if (start[2] > target[2])
                {
                    start[0] -= Variables.floor_width;
                }
                else
                {
                    start[0] += Variables.floor_width;
                }
                corridor.Add(start[0]);
                start[2] = (int)cells[start[0]].z;
                wt.WriteLine("Index: " + start[0] + " part of room: " + cells[start[0]].is_partofRoom);
                if (!cells[start[0]].is_partofRoom)
                {
                    cells[start[0]].generateSide(new int[] { 1, 1, 0, 0 }, Resources.Load("Wall") as GameObject);
                    cells[start[0]].changeMaterial(Resources.Load("Connection") as Material);
                    cells[start[0]].visited = true;
                }
            }
        }
    }
}