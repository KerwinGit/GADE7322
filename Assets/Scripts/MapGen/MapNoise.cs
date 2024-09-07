using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapNoise
{    
    public static float[,] CreateNoise(int width, int height, int seed, float scale, int octaveCount, float persistence, float lacunarity, Vector2 offset)  //draws a 2d perlin noise heightmap using xy parameters
    {
        float[,] noise = new float[width, height];

        System.Random rand = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaveCount];
        for (int i = 0; i<octaveCount; i++)
        {
            float xOffset = rand.Next(-100000, 100000) +offset.x;
            float yOffset = rand.Next(-100000, 100000) +offset.y;
            octaveOffsets[i] = new Vector2(xOffset, yOffset);
        }

        if(scale <= 0)
        {
            scale = 0.000000001f;
        }

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        for (int y = 0; y< height; y++)
        {
            for (int x = 0; x< width; x++)
            {
                float amplitude = 1;
                float frequency = 1;        //frequency affects the spacing between height value changes
                float noiseHeight = 0;

                for(int i = 0; i< octaveCount; i++)
                {
                    float sampleX = ((x - halfWidth) / scale - octaveOffsets[i].x) * frequency;
                    float sampleY = ((y - halfHeight) / scale - octaveOffsets[i].y) * frequency;

                    float perlin = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlin * amplitude;

                    //increase each octave
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                //normalise heights
                if(noiseHeight>maxHeight)
                {
                    maxHeight = noiseHeight;
                }
                else if (noiseHeight < minHeight)
                {
                    minHeight = noiseHeight;
                }
                noise[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noise[x,y] = Mathf.InverseLerp(minHeight,maxHeight, noise[x, y]);
            }
        }


                return noise;
    }

}
