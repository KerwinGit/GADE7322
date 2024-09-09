using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class creates a texture which flattens 3 pathways leading from the centre of the map to the edge
//this makes use of line renderers to generate a texture map with 3 flat lines with options for smoothing
//by subtracting this height from noise map we can guarantee 3 paths for the enemies to move on
public class PathGen
{
    public static float[,] Generate(Vector2Int size, LineRenderer[] lineRenderers, float smoothStart, float smoothEnd)
    {
        float[,] heightMap = new float[size.x, size.y];

        for (int y = 0; y < size.y; y++) //loops length(height) of map
        {
            for (int x = 0; x < size.x; x++) //loops width of map
            {
                //position on texture map
                Vector2 position = new Vector2(
                    1 - (float)x / size.x * 2 ,
                    1 - (float)y / size.y * 2 
                );

                // initialize height to max
                float minDistance = float.MaxValue;

                // check each linernderer to flatten
                foreach (LineRenderer lr in lineRenderers)
                {
                    // loop through each segment of the LineRenderer
                    for (int i = 0; i < lr.positionCount - 1; i++)
                    {
                        Vector3 start = lr.GetPosition(i);
                        Vector3 end = lr.GetPosition(i + 1);

                        // find the closest point on  line segment to pixel position
                        Vector3 closestPoint = ClosestPointOnLineSegment(start, end, new Vector3(position.x, 0, position.y));

                        // calc distance from the pixel to  closest point on  line
                        float distance = Vector3.Distance(closestPoint, new Vector3(position.x, 0, position.y));

                        // track the minimum distance for this pixel
                        minDistance = Mathf.Min(minDistance, distance);
                    }
                }
                    if (minDistance < smoothStart)
                    {
                        heightMap[x, y] = 1; // Full dip
                    }
                    else if (minDistance > smoothEnd)
                    {
                        heightMap[x, y] = 0; // No dip
                    }
                    else
                    {
                        // Smooth interpolation between full dip and no dip
                        heightMap[x, y] = Mathf.SmoothStep(1, 0, Mathf.InverseLerp(smoothStart, smoothEnd, minDistance));
                    }
            }
        }

        return heightMap;
    }

    // helper method to find the closest point on line segment
    private static Vector3 ClosestPointOnLineSegment(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 lineDir = end - start;
        float lineLength = lineDir.magnitude;
        lineDir.Normalize();

        float projectLength = Vector3.Dot(point - start, lineDir);
        projectLength = Mathf.Clamp(projectLength, 0, lineLength);

        return start + lineDir * projectLength;
    }

}
