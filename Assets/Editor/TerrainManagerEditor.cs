using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    public bool recalculate=false;
   

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();


        DrawDefaultInspector();
       
        TerrainManager terrainMan = (TerrainManager)target;


        if(GUILayout.Button("Make default map")) { terrainMan.MakeDefaultMap(); }
        if (GUILayout.Button("Generate Map")) 
        { 
            terrainMan.GenerateTerrain();
            //terrainMan.mesh = terrainMan.meshGen.mesh;
        }
        if (GUILayout.Button("Perlin Noise")) { terrainMan.MakePerlinMap(); }
        if (EditorGUILayout.Toggle(recalculate))
        {
            recalculate = true;
        }

        if (recalculate)
        { 
            terrainMan.GenerateTerrain();
        
        }
    }

    

}
