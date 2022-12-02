using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    int posX = 0;
    int posY = 0;
    string type = "0000";
    public override void OnInspectorGUI()
    {
        Map mapeditor = (Map)target;
        DrawDefaultInspector();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Get size"))
        {
            mapeditor.getMapSize(0);
        }
        
        GUILayout.Label("PosX");
        posX = EditorGUILayout.IntField(posX);
        GUILayout.Label("PosY");
        posY = EditorGUILayout.IntField(posY);
        GUILayout.Label("Type");
        type = GUILayout.TextField(type, 4);
        if (GUILayout.Button("Draw tile"))
        {
            mapeditor.drawTile(type,new Vector3Int(posX,posY,0));
        }
        if (GUILayout.Button("Draw empty"))
        {
            mapeditor.drawEmpty();
        }
        if (GUILayout.Button("Draw full"))
        {
            mapeditor.drawFull();
        }
        GUILayout.EndHorizontal();
    }
}
