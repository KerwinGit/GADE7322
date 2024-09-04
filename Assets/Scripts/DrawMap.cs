using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMap : MonoBehaviour
{

    public int width;
    public int height;
    public float scale;

    public bool autoUpdate;

    public void Draw()
    {
        float[,] map = MapNoise.CreateNoise(width, height, scale);

        DisplayMap display = FindObjectOfType<DisplayMap>();
        display.DrawNoise(map);
    }

}
