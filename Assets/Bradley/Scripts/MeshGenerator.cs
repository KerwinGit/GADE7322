using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private Vector3[] verticesArr;
    [SerializeField] private int[] triangleArr;
    [SerializeField] private int xSize;
    [SerializeField] private int zSize;

    [SerializeField] private Gradient gradient;

    [SerializeField] private float noiseScale = 0.3f; // Control noise frequency
    [SerializeField] private float heightMultiplier = 2f; // Control height of terrain

    [SerializeField] private int seed; // Seed for randomizing terrain

    [SerializeField] private LineRenderer[] lineRenderers; // LineRenderers to flatten along
    [SerializeField] private float flattenRadius = 1.5f; // Radius within which to flatten terrain along Line Renderers

    private float minHeight = float.MaxValue;
    private float maxHeight = float.MinValue;

    Color[] colours;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        seed = Random.Range(0, 10000); // Randomize the seed at start
        GenerateTerrain();
    }

    private void OnValidate()
    {
        if (mesh != null)
        {
            GenerateTerrain();
        }
    }

    private void GenerateTerrain()
    {
        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        verticesArr = new Vector3[(xSize + 1) * (zSize + 1)];

        int centerX = xSize / 2;
        int centerZ = zSize / 2;
        int flattenCenterRadius = 3; // Adjust this to control the size of the flattened central area

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise((x + seed) * noiseScale, (z + seed) * noiseScale) * heightMultiplier;

                // Flatten the central area
                if (Mathf.Abs(x - centerX) <= flattenCenterRadius && Mathf.Abs(z - centerZ) <= flattenCenterRadius)
                {
                    y = 0f;
                }

                verticesArr[i] = new Vector3(x, y, z);

                // Track min/max heights only for non-flattened areas
                if (y > 0f)
                {
                    if (y < minHeight) minHeight = y;
                    if (y > maxHeight) maxHeight = y;
                }

                i++;
            }
        }

        // Flatten along the paths of LineRenderers
        FlattenAlongLineRenderers();

        int vert = 0;
        int tris = 0;
        triangleArr = new int[xSize * zSize * 6];

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangleArr[tris + 0] = vert + 0;
                triangleArr[tris + 1] = vert + xSize + 1;
                triangleArr[tris + 2] = vert + 1;
                triangleArr[tris + 3] = vert + 1;
                triangleArr[tris + 4] = vert + xSize + 1;
                triangleArr[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }

        ApplyColors();
    }

    private void FlattenAlongLineRenderers()
    {
        if (lineRenderers == null || lineRenderers.Length == 0) return;

        for (int i = 0; i < verticesArr.Length; i++)
        {
            Vector3 vertex = verticesArr[i];

            foreach (LineRenderer lr in lineRenderers)
            {
                // Loop through each LineRenderer's positions to flatten terrain along the line
                for (int j = 0; j < lr.positionCount - 1; j++)
                {
                    Vector3 start = lr.GetPosition(j);
                    Vector3 end = lr.GetPosition(j + 1);

                    // Find the closest point on the line segment to the vertex
                    Vector3 closestPoint = ClosestPointOnLineSegment(start, end, vertex);

                    // Flatten if within radius
                    if (Vector3.Distance(closestPoint, vertex) <= flattenRadius)
                    {
                        verticesArr[i].y = 0f; // Flatten the vertex
                    }
                }
            }
        }
    }

    private Vector3 ClosestPointOnLineSegment(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 lineDirection = (end - start).normalized;
        float length = Vector3.Distance(start, end);
        float projection = Vector3.Dot(point - start, lineDirection);

        projection = Mathf.Clamp(projection, 0, length);
        return start + lineDirection * projection;
    }

    private void ApplyColors()
    {
        colours = new Color[verticesArr.Length];

        for (int i = 0; i < verticesArr.Length; i++)
        {
            float y = verticesArr[i].y;

            // If the vertex is at y = 0 (flattened), color it brown
            if (Mathf.Approximately(y, 0f))
            {
                colours[i] = Color.Lerp(new Color(0.55f, 0.27f, 0.07f), new Color(0.6f, 0.3f, 0.1f), 0.5f); // Brown
            }
            else
            {
                // Otherwise, use the gradient based on height
                float height = Mathf.InverseLerp(minHeight, maxHeight, y);
                colours[i] = gradient.Evaluate(height);
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticesArr;
        mesh.triangles = triangleArr;
        mesh.colors = colours;

        mesh.RecalculateNormals();
    }
}


