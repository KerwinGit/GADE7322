using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class creates texture height map with a flat centre and smoothly raised edges with variables to control smoothness
//this texture will be used to help guarantee a flat valley in the centre of our map when procedurally generating our terrain
//by subtracting the height of this texture from the height of the noise texture we get a flat centre
public class CentreValley   
{    
    public static float[,] Generate(Vector2Int size, float smoothStart, float smoothEnd)
    {
        float[,] heightMap = new float[size.x, size.y];

        for(int y = 0; y < size.y; y++) //loops length(height) of map
        {
            for(int x = 0; x < size.x; x++) //loops width of map
            {
                //position on texture map
                Vector2 position = new Vector2(
                    (float)x / size.x * 2 - 1,
                    (float)y / size.y * 2 - 1
                    );

                float t = Mathf.Max(Mathf.Abs(position.x), Mathf.Abs(position.y));  

                if(t < smoothStart)
                {
                    heightMap[x, y] = 1;    //raises edges
                }
                else if(t > smoothEnd)  //flattens centre
                {
                    heightMap[x, y] = 0;
                }
                else        //creates slope between center and edge
                {
                    heightMap[x, y] = Mathf.SmoothStep(1,0,Mathf.InverseLerp(smoothStart, smoothEnd, t));
                }
            }
        }

        return heightMap;
    }

}
