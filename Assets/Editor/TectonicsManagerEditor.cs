using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TectonicsManager))]
public class TectonicsManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        //DrawDefaultInspector();

        TectonicsManager manager = (TectonicsManager)target;


        if (GUILayout.Button("Generate Map"))
        {
            manager.MakeDefaultMap();
            manager.GenerateTerrain();
        }
        if (GUILayout.Button("Make Pressure Map"))
        {
            manager.MakeDefaultMap();
            manager.GenerateTerrain();
            manager.MakePressureMap();
        }
        if (GUILayout.Button("Color Map"))
        {
            manager.PaintMap();
        }
        
        if (GUILayout.Button("TEST Map"))
        {
            manager.TestMap();
        }

    }

}
