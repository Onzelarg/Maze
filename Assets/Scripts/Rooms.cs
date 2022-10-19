using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class Room
{
    public int topleft;
    public int topright;
    public int bottomleft;
    public int bottomright;

    public int room_size;
    public int room_width;
    public int room_height;
    public Dictionary<int, Material> room_cells = new Dictionary<int, Material>();
    public int cell_index;

    //visited_mat = Resources.Load("Room") as Material;
    //    corner = Resources.Load("Corner") as Material;
    //    index_mat = Resources.Load("Index") as Material;

public Room(int size, int w, int h)
    {
        this.room_size = size;
        this.room_width = w;
        this.room_height = h;
    }


    public void generate(int _cell_index, bool _x_positive, bool _z_positive, int _floor_w, int _floor_h, int created_rooms, Tiles[] cells)
    {
        this.cell_index = _cell_index;
        using (StreamWriter writetext = new StreamWriter("Debugs/room_cell_generation" + created_rooms + ".txt"))
        {
            if (_x_positive && _z_positive)
            {
                this.bottomleft = _cell_index;
                this.bottomright = bottomleft + room_width - 1;
                this.topleft = bottomleft + ((room_height - 1) * _floor_w);
                this.topright = topleft + room_width - 1;
                writetext.WriteLine("Cell index is bottom left ");
            }
            else if (_x_positive && !_z_positive)
            {
                this.topleft = _cell_index;
                this.topright = topleft + room_width - 1;
                this.bottomleft = topleft - ((room_height - 1) * _floor_w);
                this.bottomright = bottomleft + room_width - 1;
                writetext.WriteLine("Cell index is top left ");
            }
            else if (!_x_positive && _z_positive)
            {
                this.bottomright = _cell_index;
                this.bottomleft = bottomright - room_width + 1;
                this.topleft = bottomleft + ((room_height - 1) * _floor_w);
                this.topright = topleft + room_width - 1;
                writetext.WriteLine("Cell index is bottom right ");
            }
            else if (!_x_positive && !_z_positive)
            {
                this.topright = _cell_index;
                this.topleft = topright - room_width + 1;
                this.bottomleft = topleft - ((room_height - 1) * _floor_w);
                this.bottomright = bottomleft + room_width - 1;
                writetext.WriteLine("Cell index is top right ");

            }
            writetext.WriteLine("Cell index: " + _cell_index);
            writetext.WriteLine("Top Left: " + topleft);
            writetext.WriteLine("Top Right: " + topright);
            writetext.WriteLine("Bottom Left: " + bottomleft);
            writetext.WriteLine("Bottom Right: " + bottomright);
            writetext.WriteLine("Room width: " + room_width);
            writetext.WriteLine("Room height: " + room_height);
            writetext.WriteLine("X positive: " + _x_positive);
            writetext.WriteLine("Z positive: " + _z_positive);
            writetext.WriteLine("Cells: ");

            int index = 0;
            for (int z = 0; z < room_height; z++)
            {
                for (int x = 0; x < room_width; x++)
                {
                    if ((bottomleft + (z * _floor_w) + x)==cell_index)
                    {
                        room_cells[(bottomleft + (z * _floor_w) + x)] = Resources.Load("Index") as Material;
                    }
                    else if (z == 0 || z==room_height-1 || x==0 || x==room_width-1)
                    {
                        room_cells[(bottomleft + (z * _floor_w) + x)] = Resources.Load("Corner") as Material;
                    }
                    else
                    {
                        room_cells[(bottomleft + (z * _floor_w) + x)] = Resources.Load("Room") as Material;
                    }
                    cells[(bottomleft + (z * _floor_w) + x)].visited = true;
                    writetext.WriteLine(room_cells.ElementAt(index).Key);
                    index++;
                }
            }

        }
    }

    public void changeMaterial(Tiles[] cells)
    {
        using (StreamWriter writetext = new StreamWriter("Debugs/room_cell_generation_mat_change.txt"))
        {
            
            for (int i = 0; i < (room_cells.Count); i++)
            {
                cells[room_cells.ElementAt(i).Key].changeMaterial(room_cells.ElementAt(i).Value);
            }
            writetext.WriteLine("Cell index: " + this.cell_index);
            writetext.WriteLine("Top Left: " + this.topleft);
            writetext.WriteLine("Top Right: " + this.topright);
            writetext.WriteLine("Bottom Left: " + this.bottomleft);
            writetext.WriteLine("Bottom Right: " + this.bottomright);
        }
    }
}