using Unity.AI.Navigation;
using UnityEngine;

public class DrawMap : MonoBehaviour
{
    //modes to help visualise the different textures drawn
    public enum DrawMode
    {
        NoiseMap,
        ColourMap,
        Mesh,
        Falloff,
        Paths
    }
    public DrawMode drawMode;

    [Header("Dimensions")]
    public int width;
    public int height;
    public float scale;

    [Header("Noise")]
    public int octaves;
    [Range(0f, 1f)]
    public float persistence;   //graininess
    public float lacunarity;    //cloudiness
    public int seed;            //affects sample points chosen
    public Vector2 offset;
    public float heightMulti;
    public AnimationCurve heightCurve;
    public bool autoUpdate;

    [Header("Colour")]
    public Terrain[] regions;
    [SerializeField] Gradient terrainGradient;
    [SerializeField] Material material;
    [SerializeField] Texture2D gradientTexture;


    [Header("Smoothing + Centre")]
    public float smoothStart;
    public float smoothEnd;
    public Vector2Int mapSize;
    public bool useFalloff;
    float[,] falloff;

    [Header("Paths")]
    public LineRenderer[] lineRenderers;
    float[,] paths;
    public float pathSmoothStart;
    public float pathSmoothEnd;
    public LineRendererSetup lineRenderersSetup;

    [Header("Mesh")]
    public GameObject meshGO;
    public GameObject navMesh;

    private void Awake()
    {
        mapSize = new Vector2Int(width, height);

        //creates centre texture map
        falloff = CentreValley.Generate(mapSize, smoothStart, smoothEnd);

        //creates path texture map
        lineRenderersSetup.SetupRandomAngledLineRenderers();
        paths = PathGen.Generate(mapSize, lineRenderers, pathSmoothStart, pathSmoothEnd);

        if (width < 1)
        {
            width = 1;
        }
        if (height < 1)
        {
            height = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

        //random seed from system clock
        seed = System.DateTime.Now.Ticks.GetHashCode();

        Draw();

        meshGO.AddComponent<MeshCollider>();
        navMesh.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private void GradientToTexture()
    {
        gradientTexture = new Texture2D(1, 100);
        Color[] pixelColours = new Color[100];
        for (int i = 0; i < 100; i++)
        {
            pixelColours[i] = terrainGradient.Evaluate((float)i / 100);
        }
        gradientTexture.SetPixels(pixelColours);
        gradientTexture.Apply();
    }

    public void Draw()
    {
        float[,] map = MapNoise.CreateNoise(width, height, seed, scale, octaves, persistence, lacunarity, offset);

        Color[] colours = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (useFalloff)
                {
                    map[x, y] = Mathf.Clamp01(map[x, y] - falloff[x, y]);
                }

                map[x, y] = Mathf.Clamp01(map[x, y] - paths[x, y]);

                float currHeight = map[x, y];
                for (int i = 0; i < height; i++)
                {
                    if (currHeight <= regions[i].height)
                    {
                        colours[y * width + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }


        DisplayMap display = FindObjectOfType<DisplayMap>();

        //creates texture map/mesh based on mode selected
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGen.NoiseTexture(map));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGen.ColourTexture(colours, width, height));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGen.GenTerrain(map, heightMulti, heightCurve), TextureGen.ColourTexture(colours, width, height));
        }
        else if (drawMode == DrawMode.Falloff)
        {
            display.DrawTexture(TextureGen.NoiseTexture(CentreValley.Generate(mapSize, smoothStart, smoothEnd)));
        }
        else if (drawMode == DrawMode.Paths)
        {
            display.DrawTexture(TextureGen.NoiseTexture(PathGen.Generate(mapSize, lineRenderers, pathSmoothStart, pathSmoothEnd)));
        }
       
    }

    private void Start()
    {
         GradientToTexture();
        float minHeight = meshGO.GetComponent<MeshFilter>().mesh.bounds.min.y + transform.position.y;
        float maxHeight = meshGO.GetComponent<MeshFilter>().mesh.bounds.max.y + transform.position.y;
        
    }
    private void Update()
    {
        GradientToTexture();
        float minHeight = meshGO.GetComponent<MeshFilter>().mesh.bounds.min.y + transform.position.y;
        float maxHeight = meshGO.GetComponent<MeshFilter>().mesh.bounds.max.y + transform.position.y;
        material.SetTexture("terrainGradient", gradientTexture);
    }

    private void OnValidate() // same as awake, just so that we can test generating in scene view
    {

        mapSize = new Vector2Int(width, height);

        falloff = CentreValley.Generate(mapSize, smoothStart, smoothEnd);

        lineRenderersSetup.SetupRandomAngledLineRenderers();
        paths = PathGen.Generate(mapSize, lineRenderers, pathSmoothStart, pathSmoothEnd);

        if (width < 1)
        {
            width = 1;
        }
        if (height < 1)
        {
            height = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        

        Draw();
    }

    [System.Serializable]
    public struct Terrain
    {
        public string name;
        public float height;
        public Color colour;
    }

}
