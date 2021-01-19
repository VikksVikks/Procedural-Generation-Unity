using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Class That Generates 2d Noise map for terrain using perlin noise
// Output is saved into local 

[System.Serializable]
public class Perlin
{

    // Start is called before the first frame update
    //Vector3[,] vertexMap;
    public bool Add=false;

    public float[,] heightMap;
    public float perlinXScale = 0.01f;
    public float perlinYScale = 0.01f;
    public int perlinOffsetX = 0;
    public int perlinOffsetY = 0;
    public int perlinOctaves = 3;
    public float perlinPersistance = 8;
    public float perlinHeightScale = 0.09f;

    public float Perlinfrequency = 1f;
    public float PerlinAmplitude = 1f;
    public int  PerlinSamples = 4;

    // Update is called once per frame

    public Perlin()
    {
       
    }

    public float[,] GenerateMultipleNoiseMap(float[,] heightMap)
    {
        int xMax = heightMap.GetLength(0);
        int zMax = heightMap.GetLength(1);


        for (int z = 0; z < zMax; z++)
        {
            for (int x = 0; x < xMax; x++)
            {


                heightMap[x, z] += MultipleNoise(x + perlinOffsetX, z + perlinOffsetY, PerlinSamples)*perlinHeightScale;


            }
        }
        return heightMap;


    }
    public float MultipleNoise(float x , float y, int samples)
    {
        float noiseSum = 0;
        float amplitude = PerlinAmplitude;
        float frequency = Perlinfrequency;

        for (int i = 0; i < samples; i++)
        {
            noiseSum = Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;
            frequency *= 2;
            amplitude *= 0.5f;
        }
        return noiseSum;

       
    }
    public float[,] GenerateNoiseMap(float[,] heightMap)
    {
        int xMax = heightMap.GetLength(0);
        int zMax = heightMap.GetLength(1);

        for (int z = 0; z < zMax; z++)
        {
            for (int x = 0; x < xMax; x++)
            {
                if (Add)
                {
                    heightMap[x, z] += fBM((x + perlinOffsetX) * perlinXScale,
                                        (z + perlinOffsetY) * perlinYScale,
                                        perlinOctaves,
                                        perlinPersistance) * perlinHeightScale;
                }
                else
                {
                    heightMap[x, z] = fBM((x + perlinOffsetX) * perlinXScale,
                                        (z + perlinOffsetY) * perlinYScale,
                                        perlinOctaves,
                                        perlinPersistance) * perlinHeightScale;
                }
            }
        }
        return heightMap;

    }
    
    public static float fBM(float x, float y, int oct, float persistance)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        for (int i = 0; i < oct; i++)
        {
            total += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistance;
            frequency *= 2;
        }

        return total / maxValue;
    }

    public static float Map(float value, float originalMin, float originalMax, float targetMin, float targetMax)
    {
        return (value - originalMin) * (targetMax - targetMin) / (originalMax - originalMin) + targetMin;
    }

}
