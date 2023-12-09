using UnityEngine;
using System.Collections.Generic;
using Trout.Utils;
using UnityEngine.UI;

public class TerrainChunkMap : Generator
{
    /// <summary>
    /// The settings used to generate the terrain
    /// </summary>
    public TerrainSettings settings;
    public GameSettings gameSettings;

    /// <summary>
    /// The dictionary that holds each generated chunk
    /// </summary>
    /// <typeparam name="Vector2">The position of the chunk in relation to the other chunks</typeparam>
    /// <typeparam name="TerrainChunk">The chunk data</typeparam>
    public Dictionary<Vector2, TerrainChunk> chunks = new Dictionary<Vector2, TerrainChunk>();

    private void Start()
    {
        Generate();
    }

    private void Update()
    {
        foreach (KeyValuePair<Vector2, TerrainChunk> chunkData in chunks)
        {
            TerrainChunk chunk = chunkData.Value;
            MeshCollider chunkCollider = chunk.gameObject.GetComponent<MeshCollider>();
            chunkCollider.sharedMesh = chunk.mesh;
        }
    }

    public override void OnValidate()
    {
        base.OnValidate();
        settings.OnValuesUpdated -= base.OnValuesChange;
        settings.OnValuesUpdated += base.OnValuesChange;

    }

    /// <summary>
    /// This function expects a map to have the same width and length and returns an array of map parts according to the width * height.
    /// </summary>
    public override void Generate()
    {
        chunks.Clear();
        Utils.ClearChildren(gameObject);

        for (int x = 0; x < settings.size; x++)
        {
            for (int z = 0; z < settings.size; z++)
            {
                Vector2 position = new Vector2(x, z);

                Edge edgeType = GetEdgeType(x, z);
                bool hasPlayerSpawn = CalcPlayerSpawnPosition(edgeType);

                TerrainChunk chunk = new TerrainChunk(position, settings, transform, hasPlayerSpawn, edgeType);
                chunks.Add(position, chunk);
            }
        }
    }


    /// <summary>
    /// Takes a world position and returns the tile in a chunk
    /// </summary>
    /// <param name="position">A world position</param>
    public Tile GetTile(Vector3 position)
    {
        TerrainChunk chunk = chunks[WorldToChunkPosition(position)];
        return chunk.GetTile(position);
    }

    public Vector2 WorldToChunkPosition(Vector3 position)
    {
        float chunkX = Mathf.Floor((position.x + settings.gridSize / 2) / settings.gridSize);
        float chunkZ = Mathf.Floor((position.z + settings.gridSize / 2) / settings.gridSize);

        return new Vector2(chunkX, chunkZ);
    }

    private bool CalcPlayerSpawnPosition(Edge edge)
    {
        int numOfPlayers = gameSettings.players.Length;

        switch (edge)
        {
            case Edge.All:
                // do something arena like here (close combat)
                return false;
            case Edge.Top:
                return false;
            case Edge.TopLeft:
                if (numOfPlayers >= 2)
                {
                    return true;
                }
                return false;
            case Edge.Right:
                return false;
            case Edge.TopRight:
                if (numOfPlayers >= 4)
                {
                    return true;
                }
                return false;
            case Edge.Bottom:
                return false;
            case Edge.BottomLeft:
                if (numOfPlayers >= 4)
                {
                    return true;
                }
                return false;
            case Edge.BottomRight:
                if (numOfPlayers >= 2)
                {
                    return true;
                }
                return false;
            case Edge.Left:
                return false;
            case Edge.None:
                return false;
            default:
                Debug.LogError("Invalid edge position specified.");
                return false;
        }
    }

    private Edge GetEdgeType(int x, int z)
    {
        if (x == 0 && z == 0)
        {
            return Edge.BottomLeft;
        }
        if (x == 1 && z == 0)
        {
            return Edge.BottomRight;
        }
        if (x == 0 && z == 1)
        {
            return Edge.TopLeft;
        }
        if (x == 1 && z == 1)
        {
            return Edge.TopRight;
        }

        return Edge.None;
    }

}