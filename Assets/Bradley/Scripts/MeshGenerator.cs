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

    private float minHeight = 0;
    private float maxHeight;

    Color[] colours;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


    }



    // Update is called once per frame
    void Update()
    {
        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        verticesArr = new Vector3[(xSize + 1) * (zSize + 1)];

        int centerX = xSize / 2;
        int centerZ = zSize / 2;
        int flattenRadius = 3; // Adjust this to control the size of the flattened area

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;

                // Flatten the central area
                if (Mathf.Abs(x - centerX) <= flattenRadius && Mathf.Abs(z - centerZ) <= flattenRadius)
                {
                    y = 0f;
                }

                if (y > maxHeight)
                {
                    maxHeight = y;
                }
                verticesArr[i] = new Vector3(x, y, z);
                i++;
            }
        }

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
        colours = new Color[verticesArr.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                float height = Mathf.InverseLerp(minHeight, maxHeight, verticesArr[i].y);
                colours[i] = gradient.Evaluate(height);
                i++;
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

    //private void OnDrawGizmos()
    //{
    //    if (verticesArr == null)
    //        return;


    //    for (int i = 0; i < verticesArr.Length; i++)
    //    {
    //        Gizmos.DrawSphere(verticesArr[i], .1f);
    //    }
    //}
}
