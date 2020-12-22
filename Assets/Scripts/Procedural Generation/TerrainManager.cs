using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Calls other classes to perform procedural operations
//[RequireComponent(typeof(MeshGen))]
[ExecuteInEditMode]
public class TerrainManager : MonoBehaviour
{
    
    public int xSize;
    public int zSize;
    public float [,]heightMap;

    public Vector3[] Vertices;
    public Vector2[] UV;
    public int[] Triangles;

    [SerializeField] float []xHeight;
    public Mesh mesh;
    public MeshFilter filter;

    public MeshGen meshGen;
    //----------------------------
    public Perlin perlin;

    public void MakeDefaultMap()
    {
        heightMap = new float[xSize, zSize];
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                heightMap[x, z] = 0;
            }
        }
        

    }
    public void MakePerlinMap()
    {
        if (heightMap.Length <1) MakeDefaultMap();

        heightMap = perlin.GenerateNoiseMap(heightMap);
        
        
    }

    public void GenerateTerrain()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        meshGen.GenerateMesh(heightMap);

        Vertices = meshGen.Vertices;
        Triangles = meshGen.Triangles;
        

        mesh.Clear();
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.RecalculateNormals();


    }





}