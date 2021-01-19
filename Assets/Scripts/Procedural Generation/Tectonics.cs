using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Tectonics
{

    /*
     * The tectonic movement will cause noise and elevation to increase greatly,
     * in addition to generating mountain peaks. Precipitation can soften the terrain and create "paths"
     * between mountains that rivers will later use (in another process during the generation of the terrain).
     * The temperature will apply with greater or lesser force a thermal erosion algorithm,
     * which will cause flattening in the terrain, cliffs and cannons.
    */
    public int numRegions = 2;
    float FractionContinental = 0.3f;
    public List<Color> colors = new List<Color>();

    public int xMax;
    public int zMax;

    //maps;


    //Generate perlin map to use as baseline to setUP plate movement;
    //put x random points on map to represent Plates - make rules they don't clump in one corner;



    /*Voronoi version:
     * Pick N locations for plates
     * each round grow all plates in all sizes.
     * Grow all around then roll, if true grow once more in that direction random.Range(0,100) -> 0,20 nothing; 20-40 = north growth - all vertexes grow +1 to north if possible.
     * 
     * Every plate has own List<> of Own nodes which needs to run grow algo.
     * 
     * Use breadthFirstSearch to fill plates: Start at tree root and explore all neighbours before moving to the next then remove previous point(all neighbours explored)
    */
    public int[,] PressureMap(float [,]heightMap)
    {
        xMax = heightMap.GetLength(0);
        zMax = heightMap.GetLength(1);
        //[,] Regions = new float[xMax, zMax];//marks if it is free or which plates owns it. 0 = free
        

       

        int[,] occupiedMap = new int[xMax, zMax];//-1 for empty tile otherwise 1,2,3,4, id of region owning it.
       //-------------------------------------------------------------------------------------------------------

        occupiedMap = FillMap(occupiedMap, -1);

        int EmptyTiles = occupiedMap.Length;

        List<List<Vector2Int>> Regions = new List<List<Vector2Int>>();
        List<Vector2Int> CurrentNodes = new List<Vector2Int>();
        List<List<Vector2Int>> InitialNodes = new List<List<Vector2Int>>();

        //GENERATE INITIAL POINTS
        Vector2Int[] initialpoints = GeneratePoints(numRegions, occupiedMap);
        int numberOfRegions = initialpoints.Length;
        for (int i = 0; i < numberOfRegions; i++)
        {
            Regions.Add( new List<Vector2Int>() { initialpoints[i] });
            InitialNodes.Add(new List<Vector2Int>() { initialpoints[i] });
            int x = initialpoints[i].x;
            int y = initialpoints[i].y;
            occupiedMap[x, y] = i;
            EmptyTiles--;
        }


        //Plates hold coords of all of its members;
        //current plates holds coords of nodes which are being explored in current step;
        //StartNodes hold coords of nodes from which we explore in current step

        //1. Go over all regions
        //2. Go over search list - InitialNodes in current regions
        //3. Find neighbours for each InitialNodes
        //4. Check and assign Empty neighbours to CurrentNodes. and claim it for given region - i in occupyMap
        //5. pass InitialNodes to Regions for current iteration
        //6. add CurrentNodes to -> InitialRegions (Clear InitialRegions)
        //7. Clear currentNodes
        //Go again for next region until all spaces free
        int MaxIterations = 30;
        int iteration = 0;
        int occupiedTiles = CurrentNodes.Count;
        Debug.Log("List count is: " + occupiedTiles);
        while (occupiedTiles<(xMax*zMax)-10)
        {
            for (int i = 0; i < numberOfRegions; i++) //1. over all regions.
            {
                //2. Get neighbours for initnodes and claim/check occupancy
                
                CurrentNodes = FindAllNeighbours(InitialNodes[i],ref occupiedMap, i);

                foreach (var item in CurrentNodes)
                {
                    Regions[i].Add(item);
                }
                InitialNodes[i] = CurrentNodes;

                occupiedTiles += CurrentNodes.Count;
                Debug.Log("Current count is: " + occupiedTiles);
                CurrentNodes.Clear();
                
            }
            iteration++;
            //Get # of occupied tiles - Sum of all Regions
            Debug.Log("iteration num: " + iteration);
            if (iteration > MaxIterations) { Debug.Log("REACHED MAX ITERATIONS!"); break; }

        }







        return occupiedMap;
    }

    private List<Vector2Int> FindAllNeighbours(List<Vector2Int> initialNodes, ref int[,] map, int index)//Go over all initialnodes and find respective neighbours including checking for occupancy
    {
        List<Vector2Int> resultNodes = new List<Vector2Int>();
        foreach (var node in initialNodes)
        {

            List<Vector2Int> neighbours = GetNeighbours(map.GetLength(0), map.GetLength(1), node.x, node.y);
            foreach (var item in neighbours)
            {
                int value = map[item.x, item.y];
                Debug.Log("Find neighbours at: " + item+ "and value: "+value);
            }
            
            //Check for occupancy:
            foreach (var item in neighbours)
            {
                //if Item is on empty slot then add it and set it to ownership
                if(map[item.x,item.y] == -1) 
                {
                    Debug.Log("Claiming" + item);
                    resultNodes.Add(item);
                    map[item.x, item.y] = index;
                }
            }

        }
        return resultNodes;
    }

    //TODO Check for same rolls !!!
    private Vector2Int[] GeneratePoints( int numPoints, int [,]Map)
    {

        Vector2Int[] points = new Vector2Int[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            Vector2Int point = RollPeakPosition(0, 0, Map.GetLength(0), Map.GetLength(1));
            points[i] = point;
        }
        return points;
    }

    private List<Vector2Int> GetNeighbours(int xMax, int zMax,int x , int z)//Only 4 directions allowed up,down,left,right
    {
        List<Vector2Int> neightbours = new List<Vector2Int>();
        
            if(z < zMax-1) neightbours.Add(new Vector2Int(x, z + 1));

            if(z>0) neightbours.Add(new Vector2Int(x, z - 1));

            if(x>0) neightbours.Add(new Vector2Int(x - 1, z));

            if(x<xMax-1) neightbours.Add(new Vector2Int(x + 1, z));


        

        return neightbours;
    }
    private void BFSearch(float[,] map, List<Vector2> OcupiedTiles)
    {

    }
    
    private Vector2Int RollPeakPosition(int xMin, int zMin,int xMax, int zMax)
    {
        
        Vector2Int peak = new Vector2Int(Random.Range(xMin, xMax), Random.Range(zMin, zMax));
        return peak;
    }

    private int[,] FillMap(int [,]Map, int value)
    {
        for (int z = 0; z < Map.GetLength(1); z++)
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                Map[x, z] = value;
            }
        }
        return Map;
    }
}
