using UnityEngine;
using System.Collections.Generic;

public class LineRendererSetup : MonoBehaviour
{
    [SerializeField] private LineRenderer[] lineRenderers; // Assign three LineRenderers in the inspector
    [SerializeField] private Vector3 center; // The center of the flattened area
    [SerializeField] private float radius = 10f; // Radius of the lines
    [SerializeField] private float length = 20f; // Length of the lines
    [SerializeField] private Color[] colors; // Colors for each LineRenderer
    [SerializeField] private float frequency = 1f; // Frequency of the sine wave
    [SerializeField] private float amplitude = 1f; // Amplitude of the sine wave
    [SerializeField] private float minAngle = 15f; // Minimum angle between lines
    [SerializeField] private MeshGenerator meshgen;

    private void OnValidate()
    {
        if (lineRenderers == null || lineRenderers.Length != 3)
        {
            Debug.LogError("Please assign exactly three LineRenderers.");
            return;
        }

        SetupRandomAngledLineRenderers();
        meshgen.GenerateTerrain();
    }

    public void SetupRandomAngledLineRenderers()
    {
        if (lineRenderers.Length != 3) return;

        // Generate random angles with the minimum angle constraint
        float[] angles = GenerateAngles(lineRenderers.Length, minAngle);

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            LineRenderer lr = lineRenderers[i];

            if (colors.Length > i)
            {
                lr.startColor = colors[i];
                lr.endColor = colors[i];
            }

            lr.startWidth = 2f;
            lr.endWidth = 2f;
            lr.positionCount = 10; // Number of points to create a smooth curve

            // Calculate end position based on the angle
            Vector3 direction = Quaternion.Euler(0, angles[i], 0) * Vector3.forward; // Set direction based on angle
            Vector3 endPosition = center + direction * (radius + length);

            // Set positions for sinusoidal line
            Vector3[] points = new Vector3[10];
            points[0] = center;
            points[9] = endPosition;

            float segmentLength = (endPosition - center).magnitude / (points.Length - 1);

            for (int j = 1; j < 9; j++)
            {
                float t = (float)j / (points.Length - 1);
                float sinOffset = Mathf.Sin(t * Mathf.PI * frequency) * amplitude;
                // Vary the amplitude based on position for non-consistent curvature
                float variability = Mathf.Lerp(1f, Random.Range(0.5f, 1.5f), t);
                Vector3 offset = new Vector3(sinOffset * variability, 0, 0);
                points[j] = Vector3.Lerp(center, endPosition, t) + offset;
            }

            lr.SetPositions(points);
        }
    }

    private float[] GenerateAngles(int count, float minAngle)
    {
        float[] angles = new float[count];
        HashSet<float> angleSet = new HashSet<float>();
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle;
            do
            {
                angle = Random.Range(0f, 360f);
            } while (IsAngleTooClose(angle, angleSet, minAngle));

            angleSet.Add(angle);
            angles[i] = angle;
        }

        return angles;
    }

    private bool IsAngleTooClose(float angle, HashSet<float> existingAngles, float minAngle)
    {
        foreach (float existing in existingAngles)
        {
            float diff = Mathf.Abs(Mathf.DeltaAngle(angle, existing));
            if (diff < minAngle)
            {
                return true;
            }
        }
        return false;
    }
}
