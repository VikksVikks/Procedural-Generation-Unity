using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable]
public class PressureMap
{
    //Max number of tiles - xMax index is 1st thats out of bounds;
    private int xMax;
    private int zMax;
    private int numRegions;
    private int largePlates;

    public int growMult; //how faster large plates grow

    //Privates:
    public int[,] Map;
    private List<Plate> platesList;

   


    public PressureMap()
    {

    }


    public PressureMap(int x, int y, int Numreg, int largePlates, int multiplier)
    {
        
        xMax = x;
        this.zMax = y;
        this.numRegions = Numreg;
        Map = new int[xMax, zMax];
        this.largePlates = largePlates;
        this.growMult = multiplier;
        platesList = new List<Plate>();
        for (int i = 0; i < Numreg; i++)
        {
            platesList.Add(new Plate());
        }

    }

    

    public int[,] GeneratePressureMap(int x, int z,ref List<Plate> plates, int multiplier)
    {

        xMax = x;
        zMax = z;
        numRegions = plates.Count;
        largePlates = 0;
        foreach (var item in plates)
        {
            if (item.isLarge) largePlates++;
        }
        growMult = multiplier;
        platesList = new List<Plate>();
        platesList = plates;       
        Map = new int[x,z];


        Map = MakePressureMap();
        plates = platesList;

        return Map;
    }

    private int [,] MakePressureMap()
    {


        int[,] Map = new int[xMax, zMax];
        fillMap(ref Map, -1);
        int EmptyTiles = Map.Length;



        //Initialize regions
        

        Debug.Log("Regions:" + platesList.Count);
        List<List<Vector2Int>> borders = new List<List<Vector2Int>>();


        EmptyTiles = InitializeRegions();
        int InitializeRegions()
        {
            for (int i = 0; i < platesList.Count;)
            {
                //Initialize point or random spot which is not occupied and not bordering other spot.
                Vector2Int point = FindRandomPoint(Map);
                if (Map[point.x, point.y] == -1 && EmptyAround(point, Map))
                {
                    Debug.Log("index is: " + i);
                    platesList[i].index = i;
                    Map[point.x, point.y] = i;

                    Debug.Log("Trying to add point: " + point);

                    platesList[i].tiles.Add(point);

                    borders.Add(new List<Vector2Int>() { point });
                    EmptyTiles--;
                    i++;
                }

            }
            return EmptyTiles;
        }
        
        

        Debug.Log("Initialized to Regions: " + platesList.Count + "And borders: " + borders.Count);
        //Setup List to label regions that can grow:
        List<int> IndexesToGrow = new List<int>();
        for (int i = 0; i < numRegions; i++)
        {
            IndexesToGrow.Add(i);
        }

        Debug.Log("Borders");
        Debug.Log(borders);
        Debug.Log("Indexes to grow");
        foreach (var item in IndexesToGrow)
        {
            Debug.Log(item + " ");
        }

        //---------------------Main Loop
        //Pick Random border Tile that can grow and GrowIt
        //Grow in all dirs for now.
        //
        List<Vector2Int> neighbours = new List<Vector2Int>();

        while (EmptyTiles > 0)
        {


            //pick random tile from tiles that can grow (indexes to grow);
            Debug.Log("Picking Index to grow");
            Debug.Log("Size of collection:" + IndexesToGrow.Count);
            int regionIndex = IndexesToGrow[Random.Range(0, IndexesToGrow.Count)];// Random index roll from indexes to grow

            Debug.Log("Picked Index: "+regionIndex);

            //if it is largePlate , grow it by multiplier. 

            if (platesList[regionIndex].isLarge)
            {
                for (int i = 0; i < growMult; i++)
                {
                    if (!IndexesToGrow.Contains(regionIndex)) { break; }
                    int maxIndex = borders[regionIndex].Count-1;//Max possible index at given region borders
                    int tileIndex = Random.Range(0, maxIndex+1);


                    GrowRegion(ref Map, ref EmptyTiles, borders, regionIndex, tileIndex);

                    //Check if borders still have empty tiles: (Can it stil grow ?)
                    CheckBorders(borders, IndexesToGrow, regionIndex);
                }
            }
            else
            {
                int maxIndex = borders[regionIndex].Count - 1;//Max possible index at given region borders
                int tileIndex = Random.Range(0, maxIndex+1);


                GrowRegion(ref Map, ref EmptyTiles, borders, regionIndex, tileIndex);

                //Check if borders still have empty tiles: (Can it stil grow ?)
                CheckBorders(borders, IndexesToGrow, regionIndex);
            }


        }


        return Map;

    }







    private void CheckBorders(List<List<Vector2Int>> borders, List<int> IndexesToGrow, int regionIndex)
    {
        if (borders[regionIndex].Count <= 0)//No more empty indexes, remove index from regionIndex to mark it as fully grown.
        {
            Debug.Log("no more empty tiles at borders: " + borders[regionIndex].Count);
            if (IndexesToGrow.Remove(regionIndex))
            {
                Debug.Log("Success at removing from indexesTogrow: " + regionIndex);
            }
            else { Debug.Log("Fail at removing from indexesTogrow: " + regionIndex); }

        }
    }

    private void GrowRegion(ref int[,] Map, ref int EmptyTiles, List<List<Vector2Int>> borders, int regionIndex, int tileIndex)
    {
        List<Vector2Int> neighbours;
        Vector2Int tile = borders[regionIndex][tileIndex];
        borders[regionIndex].RemoveAt(tileIndex);
        //-----GROW TILE-------------------------------------------------

        neighbours = GrowTile(ref Map, tile, regionIndex);

        //Check for which have empty space around
        foreach (var item in neighbours)
        {
            if (EmptyAround(item, Map))//CAn grow around so add it to borders
            {
                borders[regionIndex].Add(item);
            }
        }
        //either way add all neighbours to regions owned tiles
        platesList[regionIndex].tiles.AddRange(neighbours);

        EmptyTiles -= neighbours.Count;
        // Calculate if neighbours are borderTiles - tiles around each have other region indexes;
        foreach (var item in neighbours)
        {
            if (IsBorder(item, Map, regionIndex))//if it is border then add to list "BorderTiles"
            {
                platesList[regionIndex].outerTiles.Add(item);
            }
        }

        neighbours.Clear();

    }

    private bool IsBorder(Vector2Int tile, int[,] map, int claimIndex)
    {
        //To be border: Must have around index of other region (-1 does not count) and should not be side tile

        int x = tile.x;
        int y = tile.y;
        //UP:
        if ((x < xMax - 1)) //check corners - is not corner
        {
            if (map[x + 1, y] != claimIndex && map[x + 1, y] != -1)//if there is tile with other index than this one and -1. (another region)
            {
                return true;
            }

        }
        //Down
        if (x > 0)
        {
            if (map[x - 1, y] != claimIndex && map[x - 1, y] != -1) { return true; }
        }
        if ((y > 0))//Can grow Left
        {
            if (map[x, y - 1] != claimIndex && map[x, y - 1] != -1) { return true; }

        }
        if ((y < zMax - 1))//Can grow Right
        {
            if (map[x, y + 1] != claimIndex && map[x, y + 1] != -1)
            {
                return true;
            }

        }
        return false;
    }
    private bool EmptyAround(Vector2Int tile, int[,] map)
    {
        int x = tile.x;
        int y = tile.y;
        if ((x < xMax - 1) && (map[x + 1, y] == -1))//Can grow Up
        {
            return true;
        }
        if ((x > 0) && (map[x - 1, y] == -1))//Can grow Down
        {
            return true;
        }
        if ((y > 0) && (map[x, y - 1] == -1))//Can grow Left
        {

            return true;
        }
        if ((y < zMax - 1) && (map[x, y + 1] == -1))//Can grow Right
        {
            return true;
        }
        return false;
    }
    //Search all neighbours for empty tile and if within bounds then claim
    //return all tiles that got claimed and marked with index
    private List<Vector2Int> GrowTile(ref int[,] map, Vector2Int tile, int claimIndex)
    {
        int x = tile.x;
        int y = tile.y;
        List<Vector2Int> neighbours = new List<Vector2Int>();
        if ((x < xMax - 1) && (map[x + 1, y] == -1))//Can grow Up
        {
            neighbours.Add(new Vector2Int(x + 1, y));
            map[x + 1, y] = claimIndex;
        }
        if ((x > 0) && (map[x - 1, y] == -1))//Can grow Down
        {
            neighbours.Add(new Vector2Int(x - 1, y));
            map[x - 1, y] = claimIndex;
        }
        if ((y > 0) && (map[x, y - 1] == -1))//Can grow Left
        {
            neighbours.Add(new Vector2Int(x, y - 1));
            map[x, y - 1] = claimIndex;

        }
        if ((y < zMax - 1) && (map[x, y + 1] == -1))//Can grow Right
        {
            neighbours.Add(new Vector2Int(x, y + 1));
            map[x, y + 1] = claimIndex;
        }
        return neighbours;
    }

    private void fillMap(ref int[,] Map, int value)
    {
        for (int z = 0; z < Map.GetLength(1); z++)
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                Map[x, z] = value;
            }

        }
    }


    private Vector2Int FindRandomPoint(int[,] Map)
    {
        Vector2Int result = new Vector2Int();
        while (true)
        {
           
            result = new Vector2Int(Random.Range(0, xMax), Random.Range(0, zMax)); //TODO Cs rand
            if (Map[result.x, result.y] == -1)
            {

                break;

            }
        }
        return result;
    }



}





    

