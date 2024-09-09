using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class TextureGen  //draws 2d textures
{

    public static Texture2D ColourTexture(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D NoiseTexture(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Color[] colours = new Color[width * height];
        for (int i = 0; i < width; i++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colours[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]); //creates array of values between white and black based on index
                }
            }
        }

        return ColourTexture(colours, width, height);
    }

}
