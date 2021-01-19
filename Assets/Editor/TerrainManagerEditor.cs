using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    
   
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        //DrawDefaultInspector();
       
        TerrainManager terrainMan = (TerrainManager)target;


        





        if (GUILayout.Button("Make default map")) { terrainMan.MakeDefaultMap(); }
        if (GUILayout.Button("Generate Map")|| terrainMan.AutoGen) 
        { 
           
            terrainMan.GenerateTerrain();
            //terrainMan.mesh = terrainMan.meshGen.mesh;
        }
        if (GUILayout.Button("Perlin Noise")|| terrainMan.AutoPerlin) { terrainMan.MakePerlinMap(); }

        if(GUILayout.Button("Voronoi Noise")) { terrainMan.GenerateVoronoiMap(); }

        if (GUILayout.Button("Make Pressure Map")) { terrainMan.MakePressureMap(); }
       
    }
    
    

    

}
