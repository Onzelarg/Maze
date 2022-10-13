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
    string cell_index = "0";
    int min_f;
    int max_f;
    int cell_quantity;
    int[] side=new int[4];

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MazeGenerator _mazeGenerator = (MazeGenerator)target;

        GUILayout.BeginHorizontal("box");

        if (GUILayout.Button("Generate"))
        {
            _mazeGenerator.generate();

        }

        if (GUILayout.Button("ClearG"))
        {
            _mazeGenerator.clear();
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

        if (GUILayout.Button("Cell Info"))
        {
            side = _mazeGenerator.update_data_arr(0, Convert.ToInt32(index), Convert.ToInt32(cell_index));

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
            GUILayout.Label("Cell Index");
            cell_index = GUILayout.TextField(cell_index, 5);
            //left     //1  0
            //right    //2  1
            //front    //3  2
            //back     //4  3
            GUILayout.Label("Left " + side[0]);
            GUILayout.Label("Right " + side[1]);
            GUILayout.Label("Front " + side[2]);
            GUILayout.Label("Back " + side[3]);
        GUILayout.EndHorizontal();



        // GUILayout.BeginHorizontal();
        // GUILayout.EndHorizontal();
    }
   


}