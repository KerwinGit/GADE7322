using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGen
{
    
    public static MapMesh GenTerrain(float[,] heightMap, float heightMulti, AnimationCurve heightCurve)
    {
        //width and height from parameters
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //top left corner to start generating from
        float topLeftX = (width - 1) / 2f;
        float topLeftZ = (height - 1) / 2f;

        //creates new mesh
        MapMesh mapMesh = new MapMesh(width, height);

        //tracks vertex array position
        int vertIndex = 0;

        //nested loop for each position in height map
        for (int y = 0; y < height; y++) 
        {
            for (int x = 0; x < width; x++)
            {
                //sets position of vertex with Y value calc from height map
                mapMesh.verts[vertIndex] = new Vector3(topLeftX - x, heightCurve.Evaluate(heightMap[x,y]) * heightMulti, topLeftZ - y);
                mapMesh.uvs[vertIndex] = new Vector2((float)x/width, (float)y/height);

                if(x < width - 1 && y < height -1) // draw triangles
                {
                    mapMesh.AddTri(vertIndex, vertIndex + width + 1, vertIndex + width);
                    mapMesh.AddTri(vertIndex + width + 1, vertIndex, vertIndex +1);
                }

                vertIndex++;
            }
        }

        return mapMesh;

    }

}


public class MapMesh
{
    public Vector3[] verts;
    public int[] tris;
    public Vector2[] uvs;

    int triIndex;

    public MapMesh(int width, int height)
    {
        verts = new Vector3[width * height];
        uvs = new Vector2[width * height];
        tris = new int[(width-1) * (height-1) * 6];
    }

    public void AddTri(int a, int b, int c)
    {
        tris[triIndex] = c;
        tris[triIndex + 1] = b;
        tris[triIndex + 2] = a;

        triIndex += 3;
    }


    //method creates mesh
    public Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}