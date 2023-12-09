using System.Collections.Generic;
using UnityEngine;
using Trout.Utils;
using System.Linq;
using System;
using UnityEngine.UI;

public class TerrainChunk
{
    /// <summary>
    /// The settings used to generate the terrain in the chunk
    /// </summary>
    public TerrainSettings settings;

    /// <summary>
    /// The center position of the chunk in the world
    /// </summary>
    public Vector3 worldPosition;

    /// <summary>
    /// The Heightmap generated for this chunk
    /// </summary>
    public HeightMap heightMap;

    /// <summary>
    /// The mesh generated in chunk
    /// </summary>
    public Mesh mesh;

    /// <summary>
    ///  The gameobject that holds this chunk
    /// </summary>
    public GameObject gameObject;

    /// <summary>
    /// The grid of tiles on this chunk. This is used for spawning objects.
    /// </summary>
    public Grid grid;

    /// <summary>
    /// This is the list of tiles with water on this chunk grouped by lakes
    /// </summary>
    public List<List<Tile>> lakes = new List<List<Tile>>();
    public List<Vector3> trees = new List<Vector3>();
    public Edge edge;

    public TerrainChunk(
        Vector2 mapPosition,
        TerrainSettings settings,
        Transform container,
        bool hasPlayerSpawn,
        Edge edge
        )
    {
        this.edge = edge;
        this.settings = settings;
        worldPosition = GetWorldPosition(mapPosition);

        Spawn(container, edge, hasPlayerSpawn);
    }

    /// <summary>
    /// Returns the tile that contains the passed tile position.
    /// </summary>
    /// <param name="position">The position in a tile position format</param>
    public Tile GetTile(Vector3 position)
    {
        return grid.tiles.First(tile => tile.Key == WorldToTilePosition(position)).Value;
    }

    public Vector2 TilePositionToTileCoordinate(Vector3 position)
    {
        return new Vector2(position.x + (settings.gridSize / 2), position.z + (settings.gridSize / 2));
    }
    public Vector2 WorldToTilePosition(Vector3 position)
    {
        return new Vector2(Mathf.Floor(position.x) + (settings.scale / 2) + 0.5f, Mathf.Floor(position.z) + (settings.scale / 2) + 0.5f);
    }

    private void Spawn(Transform container, Edge edge, bool hasPlayerSpawn)
    {
        gameObject = new GameObject("Terrain Chunk");
        gameObject.transform.parent = container.transform;
        gameObject.transform.position = worldPosition;
        gameObject.layer = settings.layerMask.value;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshRenderer.sharedMaterial = settings.material;

        NoiseStartLocation startLocation = new NoiseStartLocation(settings.meshSize, settings.startLocationSettings, hasPlayerSpawn, edge);
        heightMap = new HeightMap(settings.meshSize, settings.heightMapSettings, new Vector2(worldPosition.x, worldPosition.z), startLocation);

        mesh = new TerrainChunkMesh(this).mesh;
        grid = new Grid(worldPosition, settings.meshSize - 2, settings.scale, mesh);

        meshFilter.sharedMesh = mesh;

        // Utils.ClearChildren(GameObject.Find("DebugContainer"));

        // SpawnWater();
        // SpawnTrees();
    }

    private void SpawnTrees()
    {
        trees = new List<Vector3>();

        foreach (KeyValuePair<Vector2, Tile> tile in grid.tiles)
        {
            foreach (Vector3 corner in tile.Value.corners)
            {
                if (corner.y < settings.treeMaxHeight && corner.y > settings.treeMinHeight)
                {
                    if (!trees.Contains(corner))
                    {
                        trees.Add(corner);
                    }
                }
            }
        }

        GameObject treesObj = new GameObject("Trees");
        treesObj.transform.parent = gameObject.transform;
        foreach (Vector3 coord in trees)
        {
            GameObject tree = GameObject.Instantiate(settings.treePrefab);
            tree.transform.position = coord;
            tree.transform.parent = treesObj.transform;
            tree.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);
        }
    }

    private void SpawnWater()
    {
        lakes = new List<List<Tile>>();
        float maxHeight = settings.waterMaxHeight;
        List<Tile> waterTiles = new List<Tile>();

        foreach (KeyValuePair<Vector2, Tile> tile in grid.tiles)
        {
            if (tile.Value.corners.Any(corner => corner.y < maxHeight))
            {
                waterTiles.Add(tile.Value);
            }

        }

        GameObject lakesObj = new GameObject("Lakes");
        lakesObj.transform.parent = gameObject.transform;

        lakes = Utils.GroupAllChainedTiles(waterTiles, 1.2f);
        foreach (List<Tile> lake in lakes)
        {
            GameObject lakeObj = new GameObject("Lake");
            lakeObj.transform.parent = lakesObj.transform;

            foreach (Tile tile in lake)
            {
                GameObject water = GameObject.Instantiate(settings.waterPrefab);
                water.transform.position = new Vector3(tile.position.x, 0.5f, tile.position.z);
                water.transform.parent = lakeObj.transform;
            }
        }
    }

    private void debug(Vector3 pos)
    {
        GameObject container = GameObject.Find("DebugContainer");
        GameObject tileObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        tileObj.transform.parent = container.transform;
        tileObj.transform.position = pos;
        tileObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    private void debugGrid(List<List<Tile>> tiles)
    {
        GameObject gridDebug = new GameObject("Grid");
        gridDebug.transform.parent = GameObject.Find("DebugContainer").transform;

        foreach (List<Tile> group in tiles)
        {
            foreach (Tile tile in group)
            {
                GameObject tileObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                tileObj.transform.parent = gridDebug.transform;
                tileObj.transform.position = tile.position;
                tileObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            }
        }
    }

    private Vector3 GetWorldPosition(Vector3 localPosition)
    {
        Vector2 pos = localPosition * settings.gridSize / settings.scale;
        return new Vector3(pos.x, 0f, pos.y);
    }

}
public enum Edge
{
    Top, Left, Right, Bottom, None, TopLeft, TopRight, BottomLeft, BottomRight, All

}