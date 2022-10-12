using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorInspector : Editor
{

    bool g_left = false;    //1
    bool g_right = false;   //2
    bool g_front = false;   //3
    bool g_back = false;    //4
    string index=" ";

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

        if (GUILayout.Button("Check"))
        { 
            
        }


        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

            GUILayout.Label("Left");
            if (EditorGUILayout.Toggle(g_left))
            {
                g_left = true;
            }
            else
            {
                g_left = false;
            }

            GUILayout.Label("Right");
            if (EditorGUILayout.Toggle(g_right))
            {
                g_right = true;
            }
            else
            {
                g_right = false;
            }

            GUILayout.Label("Front");
            if (EditorGUILayout.Toggle(g_front))
            {
                g_front = true;
            }
            else
            {
                g_front = false;
            }

            GUILayout.Label("Back");
            if (EditorGUILayout.Toggle(g_back))
            {
                 g_back = true;
            }
            else
            {
                g_back = false;
            }


        

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Index");
        index = GUILayout.TextField(index, 5);
        GUILayout.EndHorizontal();



       // GUILayout.BeginHorizontal();
       // GUILayout.EndHorizontal();
    }
   


}