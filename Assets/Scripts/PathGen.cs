using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGen
{
    public static float[,] Generate(Vector2Int size, LineRenderer[] lineRenderers, float smoothStart, float smoothEnd)
    {
        float[,] heightMap = new float[size.x, size.y];

        // Loop through each pixel in the height map
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                // Convert (x, y) into a position in world space
                Vector2 pixelPosition = new Vector2(
                    (float)x / size.x * 2 - 1,
                    (float)y / size.y * 2 - 1
                );

                // Initialize height to maximum (e.g., no dip)
                float minDistance = float.MaxValue;

                // Check each LineRenderer to create dips
                foreach (LineRenderer lr in lineRenderers)
                {
                    // Loop through each segment of the LineRenderer
                    for (int i = 0; i < lr.positionCount - 1; i++)
                    {
                        Vector3 start = lr.GetPosition(i);
                        Vector3 end = lr.GetPosition(i + 1);

                        // Find the closest point on the line segment to the pixel position
                        Vector3 closestPoint = ClosestPointOnLineSegment(start, end, new Vector3(pixelPosition.x, 0, pixelPosition.y));

                        // Calculate the distance from the pixel to the closest point on the line
                        float distance = Vector3.Distance(closestPoint, new Vector3(pixelPosition.x, 0, pixelPosition.y));

                        // Track the minimum distance for this pixel
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

    // Helper function to find the closest point on a line segment
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
