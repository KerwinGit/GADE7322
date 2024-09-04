using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMap : MonoBehaviour
{

    public int width;
    public int height;
    public float scale;

    public int octaves;
    [Range(0f, 1f)]
    public float persistence;   //graininess
    public float lacunarity;    //cloudiness

    public int seed;            //affects sample points chosen
    public Vector2 offset;

    public bool autoUpdate;

    public void Draw()
    {
        float[,] map = MapNoise.CreateNoise(width, height, seed, scale, octaves, persistence, lacunarity, offset);

        DisplayMap display = FindObjectOfType<DisplayMap>();
        display.DrawNoise(map);
    }

    private void OnValidate()
    {
        if(width < 1)
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

    }

}
