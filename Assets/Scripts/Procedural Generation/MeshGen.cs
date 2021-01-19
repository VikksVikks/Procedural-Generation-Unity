using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Turns Given HeightMaps Into TerrainMesh -> returns mesh;
//all mesh operations here


[ExecuteInEditMode]
public class MeshGen
{



    //-------------------
    public Vector3[,] vertexmap;
    public  Vector3[] Vertices;   
    public  Vector2[] UV;
    public  int[] Triangles;
   
    public  Mesh mesh;


    public int xMax;
    public int zMax;
    //-----------------------------------
    float[,] Heightmap;
    private void OnEnable()
    {
        //if (!mesh) mesh = new Mesh();
    }
    private void Start()
    {
       

    }


    public void SetValues(float [,] heightmap)
    {
        
        Heightmap = heightmap;
        Debug.Log("Size: " + Heightmap.GetLength(0));
        xMax = Heightmap.GetLength(0);
        zMax = Heightmap.GetLength(1);

    }
    public void GenerateMesh(float[,] heightmap) {


        SetValues(heightmap);
        

        

        

        //1. Convert Heightmap to vertexmap
        //2. VertexMap-> make vertices
        //3. Make triangles

        //HeightToVertexMap();

        MapToVertices();
        MakeTriangles();

    }


   

    //To make Vector[,] of x y - not needed.
   /* public void HeightToVertexMap()
    {
        vertexmap = new Vector3[xMax, zMax];
        for (int z = 0; z <zMax; z++)
        {
            for (int x = 0; x < xMax; x++)
            {
                vertexmap[x, z] = new Vector3(x, 0, z);
            }
        }
    }*/
    


    public void MapToVertices()
    {


        Vertices = new Vector3[(xMax + 1) * (zMax + 1)];
        for (int i = 0, z = 0; z < zMax; z++)
        {
            for (int x = 0; x < xMax; x++)
            {
                Vertices[i] = new Vector3(x, Heightmap[x,z], z);
                i++;

            }
        }

        
    }
    
    public void MakeTriangles()//will always make grid x row by x row (xsize = # of vertices)
    {
        int vert = 0;
        int tris = 0;
        Triangles = new int[(xMax - 1) * (zMax - 1) * 6];



        for (int y = 0; y < zMax - 1; y++)
        {
            for (int x = 0; x < xMax - 1; x++)//-1 due to array index starting at 0
            {
                // 1st triangle is: 0th, 0th+row,1st
                //2nd triangle is: 1st, 0th+row, 1st+row
                
                Triangles[tris + 0] = vert + 0;
                Triangles[tris + 1] = vert + xMax;
                Triangles[tris + 2] = vert + 1;
                //yield return new WaitForSeconds(0.1f);
                Triangles[tris + 3] = vert + 1;
                Triangles[tris + 4] = vert + xMax;
                Triangles[tris + 5] = vert + xMax + 1;
                //yield return new WaitForSeconds(0.1f);
                vert++;
                tris = tris + 6;

            }
            vert++;//after finishing row dont do triangle from end of the line to new line

        }
        
    }


    /* private void OnDrawGizmos()
    {
        if (Vertices == null)return;

        for (int i = 0; i < Vertices.Length; i++)
        {
            Gizmos.DrawSphere(Vertices[i], .1f);
        }

    }*/

    


}
