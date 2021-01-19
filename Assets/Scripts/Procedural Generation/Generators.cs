using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Class having other generators

[System.Serializable]
public class Generators
{

   public Generators() { }


    public class PerlinParameters
    {
        public float mPerlinXScale = 0.01f;
        public float mPerlinYScale = 0.01f;
        public int mPerlinOctaves = 3;
        public float mPerlinPersistance = 8;
        public float mPerlinHeightScale = 0.09f;
        public int mPerlinOffsetX = 0;
        public int mPerlinOffsetY = 0;
        public bool remove = false;
    }

    public List<PerlinParameters> perlinParameters = new List<PerlinParameters>()
    {
        new PerlinParameters()
    };

    //VORONOI
    public float voronoiFallOff = 0.2f;
    public float voronoiDropOff = 0.6f;
    public float voronoiMinHeight = 0.1f;
    public float voronoiMaxHeight = 0.5f;
    public int voronoiPeaks = 5;
    public enum VoronoiType { Linear = 0, Power = 1, SinPow = 2, Combined = 3 }
    public VoronoiType voronoiType = VoronoiType.Linear;

    //Midpoint Displacement
    public float MPDheightMin = -2f;
    public float MPDheightMax = 2f;
    public float MPDheightDampenerPower = 2.0f;
    public float MPDroughness = 2.0f;

    public int smoothAmount = 2;


    public Terrain terrain;
    public TerrainData terrainData;



    public float[,] Voronoi(float[,] heightMap)
    {
        int xMax = heightMap.GetLength(0);//width
        int zMax = heightMap.GetLength(1);//height

        float Sum=0;
        //average
        for (int z = 0; z < zMax; z++)
        {
            for (int x = 0; x < xMax; x++)
            {
                Sum += heightMap[x, z];
            }
        }
        float average = Sum / (zMax * xMax);
        Debug.Log("Average = " + average);
        for (int p = 0; p < voronoiPeaks; p++)
        {

            //Pick Random point as peak and raise it by random and save it as Vector3;

            int xpeak = Random.Range(0, heightMap.GetLength(0));
            int zpeak = Random.Range(0, heightMap.GetLength(1));

            Vector3 peak = new Vector3(xpeak, heightMap[xpeak, zpeak] + Random.Range(voronoiMinHeight, voronoiMaxHeight), zpeak);
            Debug.Log("Peak at: " + peak.x + ", " + peak.z + "with height: " + peak.y);

            heightMap[xpeak, zpeak] = peak.y;
            //Go over all vertices and set their height based on distance to peak "linear distance:" h = peak.y - a*distance, h>average
            for (int x = 0; x < xMax; x++)
            {
                for (int z = 0; z < zMax; z++)
                {
                    if (x == peak.x && z == peak.z) continue;
                    float Distance = Vector2.Distance(new Vector2(peak.x, peak.z), new Vector2(x, z));
                    float h = peak.y - Distance * voronoiFallOff;

                    if (h>heightMap[x,z])
                    {
                        heightMap[x, z] = h;
                    }
                }
            }


            
        }
        return heightMap;
    }


   


}
