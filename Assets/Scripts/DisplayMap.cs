using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMap : MonoBehaviour
{
    public Renderer renderer;

    public void DrawNoise(float[,] noise)
    {
        int width = noise.GetLength(0);
        int height = noise.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colours = new Color[width * height];
        for(int i = 0; i < width; i++)
        {
            for(int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colours[y * width + x] = Color.Lerp(Color.black, Color.white, noise[x, y]); //creates array of values between white and black based on index
                }
            }
        }

        texture.SetPixels(colours);
        texture.Apply();

        renderer.sharedMaterial.mainTexture = texture;
        renderer.transform.localScale = new Vector3(width, 1, height);
    }
}
