using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGen
{
    
    public static MapMesh GenTerrain(float[,] heightMap, float heightMulti, AnimationCurve heightCurve)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float topLeftX = (width - 1) / 2;
        float topLeftZ = (height - 1) / 2;

        MapMesh mapMesh = new MapMesh(width, height);

        int vertIndex = 0;

        for (int y = 0; y < height; y++) 
        {
            for (int x = 0; x < width; x++)
            {
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