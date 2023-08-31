using System.Collections;
using System.Collections.Generic;
using Trout.Utils;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public TBuilding attributes;
    private InputManager inputManager;

    TerrainChunkMap chunkMap;

    private Vector3 mousePosition;
    private Tile mouseTile;
    private float steepness;

    private GameObject buildingVisual;

    private List<Tile> tiles;
    private Material gridMaterial;
    public Vector3 buildPosition;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private GameObject collider;
    private Rigidbody rigidbody;

    public bool isPlaceable = true;
    private bool isColliding = false;

    public void Setup(TBuilding attributes, InputManager inputManager, TerrainChunkMap chunkMap, Material gridMaterial)
    {
        this.inputManager = inputManager;
        this.attributes = attributes;
        this.chunkMap = chunkMap;
        this.gridMaterial = gridMaterial;

        Create();
    }

    void FixedUpdate()
    {
        // I don't know why I have to do this null check. If it's not here it will throw a null exception when Replace is fired.
        if (inputManager != null)
        {
            Vector3 nextPosition = inputManager.GetMousePosition();
            if (nextPosition != mousePosition)
            {
                mousePosition = nextPosition;
                mouseTile = chunkMap.GetTile(mousePosition);

                UpdateTiles();
                UpdatePlaceable();

                buildingVisual.transform.position = buildPosition;
                collider.transform.position = buildPosition;
            }
        }
    }

    public void Replace(TBuilding attributes)
    {
        this.attributes = attributes;

        GameObject oldBuilding = buildingVisual;
        GameObject newBuilding = Instantiate(attributes.visualBlueprint, buildPosition, Quaternion.identity, transform);

        oldBuilding.SetActive(false);
        buildingVisual = newBuilding;

        Destroy(oldBuilding);
    }

    private void UpdatePlaceable()
    {
        if (isColliding)
        {
            isPlaceable = false;
        }
        else
        {
            if (steepness < 0.5f)
            {
                isPlaceable = true;
            }
            else
            {
                isPlaceable = false;
            }
        }

        meshRenderer.sharedMaterial.SetColor("_Color", isPlaceable ? Color.white : Color.red);
    }

    private void Create()
    {
        mouseTile = chunkMap.GetTile(mousePosition);

        Mesh mesh = new Mesh();
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        collider = new GameObject("Collider");

        TriggerListener triggerListener = collider.AddComponent<TriggerListener>();
        triggerListener.triggerEnterEvent.AddListener(() => { isColliding = true; });
        triggerListener.triggerExitEvent.AddListener(() => { isColliding = false; });

        collider.AddComponent<BoxCollider>();
        collider.transform.parent = transform;
        collider.transform.localScale = new Vector3(attributes.width, 1f, attributes.height);

        rigidbody = collider.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;

        meshRenderer.sharedMaterial = gridMaterial;
        meshFilter.mesh = mesh;

        buildingVisual = Instantiate(attributes.visualBlueprint, mouseTile.bottomLeftCorner, Quaternion.identity, transform);

        UpdateTiles();
    }

    private void UpdateTiles()
    {
        tiles = new List<Tile>();

        for (int width = 0; width < attributes.width; width++)
        {
            for (int height = 0; height < attributes.height; height++)
            {
                if (height == 0 && width == 0)
                {
                    tiles.Add(mouseTile);
                }
                else
                {
                    Vector3 pos = new Vector3(mouseTile.position.x + width, mouseTile.position.y, mouseTile.position.z + height);
                    Tile tile = chunkMap.GetTile(pos);
                    tiles.Add(tile);
                }
            }

        }

        CreateGrid();
    }

    private void CreateGrid()
    {
        Mesh mesh = meshFilter.mesh;
        int verticesWidth = attributes.width * 2;
        int verticesHeight = attributes.height * 2;

        Vector3[] vertices = new Vector3[verticesWidth * verticesHeight];
        int[] triangles = new int[attributes.width * attributes.height * 6];
        Vector2[] uvs = new Vector2[verticesWidth * verticesHeight];

        int vertexIndex = 0;
        foreach (Tile tile in tiles)
        {
            for (int i = 0; i < tile.corners.Count; i++)
            {
                vertices[vertexIndex] = tile.corners[i];
                uvs[vertexIndex] = new Vector2(0, tile.corners[i].y);
                vertexIndex++;
            }
        }

        int baseValue = 0;
        for (int i = 0; i < triangles.Length; i += 6)
        {
            triangles[i] = baseValue + 2;
            triangles[i + 1] = baseValue + 1;
            triangles[i + 2] = baseValue;

            triangles[i + 3] = baseValue + 2;
            triangles[i + 4] = baseValue + 3;
            triangles[i + 5] = baseValue + 1;

            baseValue += 4;
        }

        UpdateSteepness(vertices);
        UpdateGridCenterPosition(vertices);

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

    private void UpdateSteepness(Vector3[] vertices)
    {
        float[] heights = new float[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            heights[i] = vertices[i].y;
        }

        steepness = Utils.GetDifference(heights);
    }

    private void UpdateGridCenterPosition(Vector3[] vertices)
    {
        float[] xPositions = new float[vertices.Length];
        float[] zPositions = new float[vertices.Length];
        float[] yPositions = new float[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            xPositions[i] = vertices[i].x;
            zPositions[i] = vertices[i].z;
            yPositions[i] = vertices[i].y;
        }

        float xPos = Utils.GetAverage(xPositions);
        float yPos = Utils.GetAverage(yPositions);
        float zPos = Utils.GetAverage(zPositions);

        buildPosition = new Vector3(xPos, yPos, zPos);
    }
}
