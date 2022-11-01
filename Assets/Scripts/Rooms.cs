using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class Room
{
    public int[] topleft = new int[3];
    public int[] topright = new int[3];
    public int[] bottomleft = new int[3];
    public int[] bottomright = new int[3];

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
            setMaterial(_floor_w,cells,created_rooms,writetext);
            
        }
    }

    void setMaterial(int _floor_w, Tiles[] cells, int created_rooms,StreamWriter wt)
    {
            int index = 0;
            for (int z = 0; z < room_height; z++)
            {
                for (int x = 0; x < room_width; x++)
                {
                    if ((bottomleft[0] + (z * _floor_w) + x) == cell_index)
                    {
                        room_cells[(bottomleft[0] + (z * _floor_w) + x)] = Resources.Load("Index") as Material;
                    }
                    else if (z == 0 || z == room_height - 1 || x == 0 || x == room_width - 1)
                    {
                        room_cells[(bottomleft[0] + (z * _floor_w) + x)] = Resources.Load("Corner") as Material;
                    }
                    else
                    {
                        room_cells[(bottomleft[0] + (z * _floor_w) + x)] = Resources.Load("Room") as Material;
                    }
                    cells[(bottomleft[0] + (z * _floor_w) + x)].visited = true;
                    wt.WriteLine(room_cells.ElementAt(index).Key);
                    index++;
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
            writetext.WriteLine("Top Left: " + this.topleft[0]);
            writetext.WriteLine("Top Right: " + this.topright[0]);
            writetext.WriteLine("Bottom Left: " + this.bottomleft[0]);
            writetext.WriteLine("Bottom Right: " + this.bottomright[0]);
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

}