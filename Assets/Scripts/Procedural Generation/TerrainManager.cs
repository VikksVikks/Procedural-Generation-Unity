using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Calls other classes to perform procedural operations
//[RequireComponent(typeof(MeshGen))]
[ExecuteInEditMode]
public class TerrainManager : MonoBehaviour
{
    public bool AutoGen;
    public bool AutoPerlin;

    public int xSize;
    public int zSize;
    public float [,]heightMap;
    public int[,] pressureMap;
    public int[] xPresssureMap;

    public float []xheightMap;

    public Color[] ColorMap;

    public Vector3[] Vertices;
    public Vector2[] UV;
    public int[] Triangles;

    [SerializeField] float []xHeight;
    public Mesh mesh;
    public MeshFilter filter;

    public MeshGen meshGen;
    //----------------------------

    public NoiseSettings noiseSettings;

    public Perlin perlin;
    public Generators generators =  new Generators();

    public PressureMap pressuremap = new PressureMap();
    private void OnEnable()
    {
        if (perlin == null) perlin = new Perlin();
    }
    public void GenerateVoronoiMap()
    {
        heightMap = generators.Voronoi(heightMap);
        xheightMap = new float[heightMap.GetLength(0)];
        for (int i = 0; i < heightMap.GetLength(0); i++)
        {
            xheightMap[i] = heightMap[i, 0];
        }
    }

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

        //heightMap = perlin.GenerateNoiseMap(heightMap);
        heightMap = perlin.GenerateMultipleNoiseMap(heightMap);

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

    public void MakePressureMap()
    {
       
       
    }

    public void MakeColorMap(int[,] Map, List<Color> colors)
    {
        int[,] indexMap = pressureMap;
        ColorMap = new Color[(Map.GetLength(0)+1) * (Map.GetLength(1)+1)];
        int i = 0;
        for (int z = 0; z < Map.GetLength(1); z++)
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                
                if (indexMap[x,z] == -1) ColorMap[i] = colors[0];//TODO for now its the index of color if tile is not taken
                else
                {
                    ColorMap[i] = colors[indexMap[x, z]];
                }
                
            }
        }
        //return colorMap;
    }
    
}