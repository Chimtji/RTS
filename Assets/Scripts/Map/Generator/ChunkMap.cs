using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkMap
{
    public int mapSize;
    public HeightMapSettings heightMapSettings;

    /// <summary>
    /// How deep the map should go from height 0
    /// </summary>
    readonly float EDGEDEPTH = 5f;

    /// <summary>
    /// The scale of each chunk in the world. This is a constant because it's not supposed to change once set in development.
    /// </summary>
    readonly float CHUNKSCALE = 1f;

    /// <summary>
    /// the number of vertices per line. This is a constant because of the square limit of each mesh.
    /// </summary>
    /// { 48, 72, 96, 120, 144, 168, 192, 216, 240 };
    readonly int CHUNKSIZE = 97;

    public Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

    public ChunkMap(int mapSize, HeightMapSettings heightMapSettings)
    {
        this.mapSize = mapSize;
        this.heightMapSettings = heightMapSettings;
        CreateChunkList();
    }

    /// <summary>
    /// This function expects a map to have the same width and length and returns an array of map parts according to the width * height.
    /// </summary>
    public void CreateChunkList()
    {
        for (int x = 0; x <= mapSize; x++)
        {
            for (int z = 0; z <= mapSize; z++)
            {
                Vector2 position = new Vector2(x, z);
                Edge edgeType = GetEdgeType(x, z);
                Chunk chunk = CreateChunk(position, edgeType);
                chunks.Add(position, chunk);
            }
        }
    }

    /// <summary>
    /// Returns the CHUNKSIZE converted to world size.
    /// </summary>
    public float GetChunkWorldSize()
    {
        return (CHUNKSIZE - 3) * CHUNKSCALE;
    }

    /// <summary>
    /// This function creates the chunk with its heightmap and adds to the list of chunks.
    /// </summary>
    /// <param name="position">the chunk position</param>
    /// <param name="edgeType">the edge type of the chunk</param>
    private Chunk CreateChunk(Vector2 position, Edge edgeType)
    {
        Vector2 chunkWorldPos = position * GetChunkWorldSize();
        Vector2 chunkCenterWorldPosition = chunkWorldPos / CHUNKSCALE;

        HeightMap heightMap = new HeightMap(CHUNKSIZE, heightMapSettings, chunkCenterWorldPosition);

        Bounds bounds = new Bounds(chunkCenterWorldPosition, Vector2.one * GetChunkWorldSize());
        Chunk chunk = new Chunk(chunkCenterWorldPosition, position, heightMap, edgeType, bounds, CHUNKSIZE, CHUNKSCALE, EDGEDEPTH);
        return chunk;

    }

    /// <summary>
    /// This function returns which edge a given chunk position has.
    /// </summary>
    /// <param name="x">the x chunk position</param>
    /// <param name="z">the z chunk position</param>
    /// <returns></returns>
    private Edge GetEdgeType(int x, int z)
    {
        if (x == 0 && z == mapSize)
        {
            return Edge.TopLeft;
        }
        else if (x == 0 && z == 0)
        {
            return Edge.BottomLeft;
        }
        else if (x == mapSize && z == mapSize)
        {
            return Edge.TopRight;
        }
        else if (x == mapSize && z == 0)
        {
            return Edge.BottomRight;
        }
        else if (z == mapSize)
        {
            return Edge.Top;
        }
        else if (z == 0)
        {
            return Edge.Bottom;
        }
        else if (x == mapSize)
        {
            return Edge.Right;
        }
        else if (x == 0)
        {
            return Edge.Left;
        }

        return Edge.None;
    }
}