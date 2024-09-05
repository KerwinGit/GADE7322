using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DrawMap))]
public class DrawMapEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawMap drawer = (DrawMap)target;

        if (DrawDefaultInspector())
        {
            if(drawer.autoUpdate)
            {
                drawer.Draw();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            drawer.Draw();
        }
    }

}
