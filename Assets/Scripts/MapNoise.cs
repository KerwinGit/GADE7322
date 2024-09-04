using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapNoise
{    
    public static float[,] CreateNoise(int width, int height, float scale)  //draws a 2d perlin noise heightmap using xy parameters
    {
        float[,] noise = new float[width, height];

        if(scale <= 0)
        {
            scale = 0.000000001f;
        }

        for (int y = 0; y< height; y++)
        {
            for (int x = 0; x< width; x++)
            {
                float sampleX = x/scale;
                float sampleY = y/scale;

                float perlin = Mathf.PerlinNoise(sampleX, sampleY);
                noise[x, y] = perlin;
            }
        }

        return noise;
    }

}
