using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables
{
    public static float tile_size;
    public static int floor_width;
    public static int floor_height;
    public static float tile_scale;

    public static void updateVar(float tsi,float tss,int fh,int fw)
    {
        tile_size = tsi;
        floor_height = fh;
        floor_width = fw;
        tile_scale = tss;
    }

}
