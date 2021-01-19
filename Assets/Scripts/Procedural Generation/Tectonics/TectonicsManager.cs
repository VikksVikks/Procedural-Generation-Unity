using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TectonicsManager : MonoBehaviour
{
    public static List<Plate> regionsList;

    public  int xSize;
    public int zSize;
    public float[,] heightMap;
    public int[,] regionsMap;

    //Tectonics DATA:
    
    public int numRegions=10;
    public int LargePlates = 2;
    public int largeGrowMult=2;

    public List<Color> colours;


    //Mesh Data:
    public Color[] ColorMap;
    private Vector3[] Vertices;
    private Vector2[] UV;
    public int[] Triangles;

    public Mesh mesh;
    public MeshFilter filter;
    public MeshGen meshGen;
    //----------------------------
    
    public PressureMap pressuremap = new PressureMap();
    public void TestMap()
    {




    }
    //Initial Methods
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

    //PressureMap and Colors
    public void MakePressureMap()
    {
        SetRegionsList();
        regionsMap = pressuremap.GeneratePressureMap(xSize, zSize, ref regionsList, largeGrowMult);

        MakeDefaultMap();
        GenerateTerrain();
    }
    public void SetRegionsList()
    {
        regionsList = new List<Plate>();

        


        for (int i = 0; i < numRegions; i++)
        {
            regionsList.Add(new Plate(i));
            regionsList[i].tiles = new List<Vector2Int>();
            regionsList[i].outerTiles = new List<Vector2Int>();

        }
        //Large plates setup
        float fraction = (float)(LargePlates / numRegions);
        int numLarge = 0;
        for (int i = 0; i < numRegions; i++)
        {
            if (Random.Range(0, 100) < fraction * 100)
            {
                regionsList[i].isLarge = true;
                numLarge++;
            }
            if (numLarge >= LargePlates) break;
        }
        for (int i = 0; i < numRegions; i++)
            {
            if (LargePlates >= numLarge) break;
            if (!regionsList[i].isLarge)
                {
                    regionsList[i].isLarge = true;
                    LargePlates++;
                }
            }
        
    }
    public void PaintMap()
    {
        colours = GenerateColors(regionsList.Count + 1); // Generate Random colours +1 for emptyTiles;
        //ColorMap =  MakeColorMap(regionsMap, colours);// generates/fills colorMap variable same size as vertex array.

        Debug.Log(regionsMap[1, 1]);
        Debug.Log("Colours are:" + colours.Count);
        int index = 0;
        ColorMap = new Color[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                index = (regionsMap[x, z]);
                Debug.Log("Color index is: " + index);

                ColorMap[i] = colours[index];
                i++;

            }
        }




        //ColorMap is array of colours corrensponding to vertices they are ordered similarly as triangles
        //Now assign colorMap to meshrender;
        mesh.Clear();
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.colors = ColorMap;
        mesh.RecalculateNormals();

        foreach (var item in mesh.colors)
        {
            Debug.Log("vertex color: " + "(" + item.r + "," + item.g + "," + item.b + ")");
        }

    }

    public List<Color> GenerateColors(int num)
    {
        List<Color> Generatedcolours = new List<Color>();
        for (int i = 0; i < num; i++)
        {
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);

            Generatedcolours.Add(new Color(r, g, b));

        }
        return Generatedcolours;
    }
    //Movement, moisture and elevation:

    public void SetPlateAtributes()
    {

    }




}


enum LocationType
{
    Mountains,Hills,Plains,Beach
}


[System.Serializable]
public class Plate
{
    public int index; //Claim index;
    public bool isLarge;
    public List<Vector2Int> tiles;
    public List<Vector2Int> outerTiles;
    public float moisture;    //(-1,1)
    public float elevation; // (-1,1)
    public Vector2 direction; //normalized direction vector
    public float magnitude;  //strength of movement (-1,1)
    public float temperature;
    public Dictionary<int, List<Vector2Int>> Borders; // Index : Tile valuye pairs of given border

    public Plate()
    {

        isLarge = false;
        tiles = new List<Vector2Int>();
        outerTiles = new List<Vector2Int>();
        moisture = 0;
        elevation = 0;
        direction = new Vector2Int(0, 0);
        this.Borders = new Dictionary<int, List<Vector2Int>>(); 
    }
    public Plate(int i) : base()
    {
        index = i;
        
        
    }
    public Plate(int i, bool large) : base()
    {
        index = i;
        isLarge = large;
        
        
    }
    public void EvaluateBorders(int[,] OccupancyMap)//fills dictionary with relevant border tiles with proper neighbour indexes
    {

        foreach (var item in outerTiles)
        {
            //int neighbours[] = FindAllNeighbours(); // returns array of indices of all neighbours from current Tile (excluding itself)
            //Foreach nenighbour in array add key value pair to dictionary (index, Tile(x,z)) - 1 tile can have 3 neighbours => will be there 3times


        }

    }

    public void Initialize()
    {
        this.tiles = new List<Vector2Int>();
        this.outerTiles = new List<Vector2Int>();
        this.direction = new Vector2Int();
        this.Borders = new Dictionary<int, List<Vector2Int>>();

    }
}
