using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMap : MonoBehaviour
{
    public Renderer mapRenderer;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture)
    {
        mapRenderer.sharedMaterial.mainTexture = texture;
        mapRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MapMesh mapMesh, Texture2D noiseMap)
    {
        meshFilter.sharedMesh = mapMesh.GenerateMesh();
        meshRenderer.sharedMaterial.mainTexture = noiseMap;
    }
}