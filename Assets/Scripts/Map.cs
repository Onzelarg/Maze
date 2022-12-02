using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public Tilemap topTMap;
    public Tilemap bottomTMap;
    public Tilemap playerTMap;
    Dictionary<string, Tile> mapTiles;
    public Camera mapCam;
    public Camera miniMap;
    public static List<int> cells;

    void Awake()
    {
        mapTiles = new Dictionary<string, Tile>();
        mapTiles.Add("1001", Resources.Load("Topleft") as Tile);
        mapTiles.Add("0001", Resources.Load("Top") as Tile);
        mapTiles.Add("0101", Resources.Load("Topright") as Tile);

        mapTiles.Add("1000", Resources.Load("Left") as Tile);
        mapTiles.Add("0000", Resources.Load("Empty") as Tile);
        mapTiles.Add("0100", Resources.Load("Right") as Tile);

        mapTiles.Add("1010", Resources.Load("Bottomleft") as Tile);
        mapTiles.Add("0010", Resources.Load("Bottom") as Tile);
        mapTiles.Add("0110", Resources.Load("Bottomright") as Tile);

        mapTiles.Add("0111", Resources.Load("Noleft") as Tile);
        mapTiles.Add("1110", Resources.Load("Notop") as Tile);
        mapTiles.Add("1011", Resources.Load("Noright") as Tile);
        mapTiles.Add("1101", Resources.Load("Nobottom") as Tile);

        mapTiles.Add("1100", Resources.Load("Sides") as Tile);
        mapTiles.Add("0011", Resources.Load("Topbottom") as Tile);

        mapTiles.Add("player", Resources.Load("Player") as Tile);
        cells = new List<int>();
    }

    public void drawTile(string type,Vector3Int pos)
    {
        if (type=="0000")
        {
            bottomTMap.SetTile(pos, mapTiles[type]);
        }
        else
        {
            topTMap.SetTile(pos, mapTiles[type]);
        }
    }

    public int[] getMapSize(int floorIndex)
    {
        int[] mapSize = new int[] { MazeGenerator.floors[floorIndex].floor_width, MazeGenerator.floors[floorIndex].floor_height };
        return mapSize;
    }
    void changeCameraPosition(int[] mapSize)
    {
        Vector3 camPos = new Vector3(0, -490, 0);
        camPos.x = mapSize[0] / 2;
        camPos.z = mapSize[1] / 2;
        mapCam.transform.position = camPos;
        miniMap.transform.position = camPos;
    }
    void moveMinimap(Vector3 pos)
    {
        miniMap.transform.position = pos;
    }

    public void drawEmpty()
    {
        int[] mapSize = getMapSize(0);
        changeCameraPosition(mapSize);
        Vector3Int tilePos = new Vector3Int(0, 0, 0);
        for (int i = 0; i < mapSize[1]; i++)
        {
            tilePos.y = i;
            for (int j = 0; j < mapSize[0]; j++)
            {
                tilePos.x = j;
                drawTile("0000", tilePos);
            }
        }
    }
    string getCellType(int floorIndex,int cellindex)
    {
        string type="";
        Tiles cell = MazeGenerator.floors[floorIndex].cells[cellindex];
        for (int i = 0; i < 4; i++)
        {
            
            if (cell.side[i]==0)
            {
                type += "0";
            }
            else
            {
                type += "1";
            }
        }
        return type;
    }
    Vector3Int getCellPosition(int floorindex,int cellindex)
    {
        Vector3Int result = new Vector3Int(0, 0, 0);
        float size = MazeGenerator.floors[floorindex].tile_size;
        result.x = (int)((MazeGenerator.floors[floorindex].cells[cellindex].x)/size);
        result.y = (int)((MazeGenerator.floors[floorindex].cells[cellindex].z)/size);
        return result;
    }


    public void drawFull()
    {
        drawEmpty();
        int[] mapSize = getMapSize(0);
        changeCameraPosition(mapSize);
        Vector3Int tilePos = new Vector3Int(0, 0, 0);
        for (int i = 0; i < mapSize[1]; i++)
        {
            tilePos.y = i;
            for (int j = 0; j < mapSize[0]; j++)
            {
                tilePos.x = j;
                drawTile(getCellType(0, (i * mapSize[1]+j)), tilePos);
            }
        }
    }
     
    public void updateTile(int index)
    {
        cells.Add(index);
        Vector3Int position = getCellPosition(0, index);
        playerTMap.ClearAllTiles();
        drawTile(getCellType(0, index), position);
        playerTMap.SetTile(position, mapTiles["player"]);
        moveMinimap(new Vector3(position.x,-490,position.y));
    }


}
