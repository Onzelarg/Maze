using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorInspector : Editor
{

    /*
    bool g_left = false;    //1
    bool g_right = false;   //2
    bool g_front = false;   //3
    bool g_back = false;    //4
    */
    string index="0";
    int cell_index = 0;
    int min_f;
    int max_f;
    int cell_quantity;
    int[] side=new int[8];
    int seed = 12345678;
    string _seed = "12345678";
    string _room = "0";
    int method;
    bool overlap = true;
    bool merged = false;
    bool touch = false;
    string room_id = "0";
    float min = 0.05f;
    float max = 0.2f;
    bool[] visited = new bool[] { false,false};
    int ballz=10;
    int cubex = 3;
    int cubey = 3;
    int cubez = 3;

    char[] letterIndex = { 'A','a','B','b','C','c','D','d','E','e','F','f','G','g','H','h','I','i','J',
        'j','K','k','L','l','M','m','N','n','O','o','P','p','Q','q','R','r','S','s','T','t','U','u','V',
        'v','W','w','X','x','Y','y','Z','z'};

    int[] letterValue={1000,999,2000,1999,3000,3999,4000,4999,5000,5999,6000,6999,7000,7999,8000,8999,9000,
        9999,10000,10999,11000,11999,12000,12999,13000,13999,14000,14999,15000,15999,16000,16999,17000,17999,
        18000,18999,19000,19999,20000,20999,21000,21999,22000,22999,23000,23999,24000,24999,25000,25999,26000,26999};

    int returnSeed(string _seed)
    {
        int seed = 0;
        try
        {
            seed = Convert.ToInt32(_seed);
        }
        catch (Exception)
        {
            for (int i = 0; i < _seed.Length; i++)
            {
                for (int j = 0; j < letterIndex.Length; j++)
                {
                    if (_seed[i] == letterIndex[j])
                    {
                        seed = seed + letterValue[j];
                        Debug.Log("Seed: " + seed);
                        break;
                    }
                }
            }
        }
        return seed;
    }

     
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MazeGenerator _mazeGenerator = (MazeGenerator)target;

        GUILayout.BeginHorizontal("box");

        if (GUILayout.Button("Generate"))
        {
            _mazeGenerator.clear();
            seed = returnSeed(_seed);
            
            _mazeGenerator.generate(seed,min,max);

        }

        if (GUILayout.Button("ClearG"))
        {
            _mazeGenerator.clear();
        }
        if (GUILayout.Button("ClearNV"))
        {
            _mazeGenerator.clearNotvisited();
        }


        /*
        if (GUILayout.Button("ClearW"))
        {
            _mazeGenerator.clearWall();
        }

        if (GUILayout.Button("Walls"))
        {
            int[] side =new int[] { 0, 0, 0, 0 };
            if (g_left) { side[0] = 1; }
            if (g_right) { side[1] = 1; }
            if (g_front) { side[2] = 1; }
            if (g_back) { side[3] = 1; }

          //  Tiles cell=new Tiles(new Vector3(0,0,0), 8f);
           // cell.generateSide(new int[] { 1, 1, 1, 1 },0);
            //_mazeGenerator.generateSide(side,new Vector3(0,0,0),0);
        }
        */

        if (GUILayout.Button("Floor Info"))
        {
            min_f=_mazeGenerator.update_data(0,Convert.ToInt32(index));
            max_f=_mazeGenerator.update_data(1, Convert.ToInt32(index));
            cell_quantity=_mazeGenerator.update_data(2, Convert.ToInt32(index));
        }
        if (GUILayout.Button("Make walls"))
        {
            _mazeGenerator.makeWalls();
        }
        if (GUILayout.Button("Gen no room"))
        {
            _mazeGenerator.generateMazeNoRooms();
        }
        if (GUILayout.Button("C con"))
        {
            _mazeGenerator.ccon();
        }


        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();



        if (GUILayout.Button("Fix corridor"))
        {
            _mazeGenerator.fixc();
        }


        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

            GUILayout.Label("Min");
             min = EditorGUILayout.FloatField(min);

            GUILayout.Label("Max");
            max = EditorGUILayout.FloatField(max);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
       
        if (GUILayout.Button("Room Gen"))
        {
            _mazeGenerator.generateRooms(method);

        }
        if (GUILayout.Button("Room G_all"))
        {
            _mazeGenerator.generateRooms(method);
            _mazeGenerator.room();
        }
        GUILayout.Label("Overlap: ");
        if (EditorGUILayout.Toggle(overlap))
        {
            overlap = true;
            merged = false;
            touch = false;
            method = 1;
        } 
        else
        {
            overlap = false;
        }
        GUILayout.Label("Merged: ");
        if (EditorGUILayout.Toggle(merged))
        {
            overlap = false;
            merged = true;
            touch = false;
            method = 2;
        }
        else
        {
            merged = false;
        }
        GUILayout.Label("Only touch: ");
        if (EditorGUILayout.Toggle(touch))
        {
            overlap = false;
            merged = false;
            touch = true;
            method = 3;
        }
        else
        {
            touch = false;
        }
        GUILayout.EndHorizontal();

        /*
        GUILayout.BeginHorizontal();

            GUILayout.Label("Left");
            if (EditorGUILayout.Toggle(g_left))
            {
                
            }
            else
            {
                
            }        

        GUILayout.EndHorizontal();

        */

        GUILayout.BeginHorizontal();
            GUILayout.Label("Min Floor: "+min_f);
            GUILayout.Label("Max Floor: "+max_f);
            GUILayout.Label("Cell Quantity: "+cell_quantity);
            GUILayout.Label("Index");
            index = GUILayout.TextField(index, 5);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Seed");
        _seed = GUILayout.TextField(_seed, 10);
        
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Room");
        _room = GUILayout.TextField(_room, 2);
        if (GUILayout.Button("Room"))
        {
            _mazeGenerator.room(Convert.ToInt32(_room));
        }
        if (GUILayout.Button("Room all"))
        {
            _mazeGenerator.room();
        }
      
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Room Index");
        room_id = GUILayout.TextField(room_id, 2);

        if (GUILayout.Button("Room cek"))
        {
            _mazeGenerator.roomCek(Convert.ToInt32(room_id));
        }


        GUILayout.EndHorizontal();


        // GUILayout.BeginHorizontal();
        // GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Cell Index");
        cell_index = EditorGUILayout.IntField(cell_index);
        if (GUILayout.Button("Cell Info"))
        {
            side = _mazeGenerator.update_data_arr(0, Convert.ToInt32(index), Convert.ToInt32(cell_index));
            visited = _mazeGenerator.isVisited(cell_index);
        }
        if (GUILayout.Button("Destroy wall"))
        {
            _mazeGenerator.clearWall(cell_index);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        
        //left     //1  0
        //right    //2  1
        //front    //3  2
        //back     //4  3
        GUILayout.Label("Left " + side[0]);
        GUILayout.Label("Right " + side[1]);
        GUILayout.Label("Front " + side[2]);
        GUILayout.Label("Back " + side[3]);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Neighbors: " + side[4]+" , " + side[5] + " , " + side[6] + " , " + side[7]);
        GUILayout.Toggle(visited[0],"Visited");
        GUILayout.Toggle(visited[1], "Is inside");
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Player stuff");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn player"))
        {
            _mazeGenerator.spawnPlayer();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Ballz amount");
        ballz = EditorGUILayout.IntField(ballz);
        if (GUILayout.Button("Spawn ballz"))
        {
            spawnBallz();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.Label("Cube X");
        cubex = EditorGUILayout.IntField(cubex);

        GUILayout.Label("Cube Y");
        cubey = EditorGUILayout.IntField(cubey);

        GUILayout.Label("Cube Z");
        cubez = EditorGUILayout.IntField(cubez);

        if (GUILayout.Button("Spawn ballz"))
        {
            spawnCubes();
        }
        GUILayout.EndHorizontal();



    }
    void spawnBallz()
    {
        GameObject ballziz = Resources.Load("pushme") as GameObject;
        for (int i = 0; i < ballz; i++)
        {
            ballziz=UnityEngine.Object.Instantiate(ballziz, new Vector3(0, i*1, 0), new Quaternion());
            ballziz.transform.name = "Ballz " + i;
        }
    }

    void spawnCubes()
    {
        GameObject cube = Resources.Load("pushcube") as GameObject;
        GameObject player=GameObject.FindGameObjectWithTag("Player");
        Vector3 cubeloc = player.transform.position;
        cubeloc.y = -0.15f;
        Vector3 offset = new Vector3(20,0,20);
        Vector3 iteroffset = new Vector3(0, 0, 0);
        float cubesize = 0.3f;
        float scale = 2;
        cubeloc += offset;
        Vector3 cubescale = new Vector3(scale, scale, scale);
        cubesize *= scale;
        for (int i = 0; i < cubey; i++)
        {
            iteroffset.y += cubesize;
            iteroffset.x = 0;

            for (int j = 0; j < cubex; j++)
            {
                iteroffset.x += cubesize;
                iteroffset.z = 0;
                for (int k = 0; k < cubez; k++)
                {
                    iteroffset.z += cubesize;
                    cube = UnityEngine.Object.Instantiate(cube, cubeloc+iteroffset, new Quaternion());
                    cube.transform.localScale = cubescale;
                    cube.transform.name = "Cube " + (k+k*j+k*j*i);
                }
            }
            
        }
    }

    

}