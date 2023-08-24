using UnityEngine;
using System.Collections.Generic;
using Trout.Utils;

public class TerrainChunkMap : Generator
{
    /// <summary>
    /// The settings used to generate the terrain
    /// </summary>
    public TerrainSettings settings;

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

        int shiftedSize = settings.size - 1;

        for (int x = 0; x <= shiftedSize; x++)
        {
            for (int z = 0; z <= shiftedSize; z++)
            {
                Vector2 position = new Vector2(x, z);
                Edge edgeType = Edge.None;

                // if (shiftedSize != 0)
                // {
                //     edgeType = GetEdgeType(x, z);
                // }

                TerrainChunk chunk = new TerrainChunk(position, settings, transform, settings.scale);
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

    private Edge GetEdgeType(int x, int z)
    {
        if (x == 0)
        {
            if (z == settings.size)
            {
                return Edge.TopLeft;
            }
            else if (z == 0)
            {
                return Edge.BottomLeft;
            }
            else
            {
                return Edge.Left;
            }
        }
        else if (x == settings.size)
        {
            if (z == settings.size)
            {
                return Edge.TopRight;
            }
            else if (z == 0)
            {
                return Edge.BottomRight;
            }
            else
            {
                return Edge.Right;
            }
        }
        else if (z == settings.size)
        {
            return Edge.Top;
        }
        else if (z == 0)
        {
            return Edge.Bottom;
        }

        return Edge.None;
    }

}