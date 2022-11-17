using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Maze : MazeGenerator
{
    private void Start()
    {
    }

    public void UIgenerateRooms(int method)
    {
        floors[0].generateRoom(method,max_room_ratio,tries,minimum_created_room);
    }

    public void UIgenerateMazeNoRooms()
    {
        MazeGenerator.floors[0].generateMazeNoRooms();
    }
}