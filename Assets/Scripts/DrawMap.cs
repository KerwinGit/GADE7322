using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawMap : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColourMap,
        Mesh,
        Falloff,
        Paths
    }
    public DrawMode drawMode;

    public int width;
    public int height;
    public float scale;

    public int octaves;
    [Range(0f, 1f)]
    public float persistence;   //graininess
    public float lacunarity;    //cloudiness

    public int seed;            //affects sample points chosen
    public Vector2 offset;

    public float heightMulti;
    public AnimationCurve heightCurve;

    public bool autoUpdate;

    public Terrain[] regions;

    public float smoothStart;
    public float smoothEnd;
    public Vector2Int mapSize;

    public bool useFalloff;
    float[,] falloff;

    public LineRenderer[] lineRenderers; // LineRenderers to flatten along
    float[,] paths;
    public float pathSmoothStart;
    public float pathSmoothEnd;

    public LineRendererSetup lineRenderersSetup;

    public GameObject meshGO;

    private void Awake()
    {
        mapSize = new Vector2Int(width, height);

        falloff = CentreValley.Generate(mapSize, smoothStart, smoothEnd);

        paths = PathGen.Generate(mapSize, lineRenderers, pathSmoothStart, pathSmoothEnd);

        if (width < 1)
        {
            width = 1;
        }
        if (height < 1)
        {
            height = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

        seed =  System.DateTime.Now.Ticks.GetHashCode();

        lineRenderersSetup.SetupRandomAngledLineRenderers();

        Draw();

        meshGO.AddComponent<MeshCollider>();
    }

    public void Draw()
    {
        float[,] map = MapNoise.CreateNoise(width, height, seed, scale, octaves, persistence, lacunarity, offset);

        Color[] colours = new Color[width*height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(useFalloff)
                {
                    map[x,y] = Mathf.Clamp01(map[x,y] - falloff[x,y]);
                }

                map[x, y] = Mathf.Clamp01(map[x, y] - paths[x, y]);

                float currHeight = map[x, y];
                for (int i = 0; i < height; i++)
                {
                    if (currHeight <= regions[i].height)
                    {
                        colours[y*width+x] = regions[i].colour;
                        break;
                    }
                }
            }
        }


        DisplayMap display = FindObjectOfType<DisplayMap>();

        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGen.NoiseTexture(map));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGen.ColourTexture(colours, width, height));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGen.GenTerrain(map, heightMulti, heightCurve), TextureGen.ColourTexture(colours, width, height));
        }
        else if(drawMode == DrawMode.Falloff)
        {
            display.DrawTexture(TextureGen.NoiseTexture(CentreValley.Generate(mapSize, smoothStart, smoothEnd)));
        }
        else if(drawMode == DrawMode.Paths)
        {
            display.DrawTexture(TextureGen.NoiseTexture(PathGen.Generate(mapSize, lineRenderers, smoothStart, smoothEnd)));
        }
    }    

    private void OnValidate()
    {
        mapSize = new Vector2Int(width, height);

        falloff = CentreValley.Generate(mapSize, smoothStart, smoothEnd);

        paths = PathGen.Generate(mapSize, lineRenderers, pathSmoothStart, pathSmoothEnd);

        if (width < 1)
        {
            width = 1;
        }
        if (height < 1)
        {
            height = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

        lineRenderersSetup.SetupRandomAngledLineRenderers();

        Draw();
    }

    [System.Serializable]
    public struct Terrain
    {
        public string name;
        public float height;
        public Color colour;
    }

}
