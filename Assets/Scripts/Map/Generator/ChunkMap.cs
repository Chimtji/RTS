using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkMap
{
    public int mapSize;
    public Vector2 mapCenterWorld;
    public float mapSizeWorld
    {
        get
        {
            return GRIDSIZE * (mapSize + 1);
        }
    }
    public TerrainSettings terrainSettings;

    /// <summary>
    /// How deep the map should go from height 0
    /// </summary>
    public readonly float EDGEDEPTH = 5f;

    /// <summary>
    /// The scale of each chunk in the world. This is a constant because it's not supposed to change once set in development.
    /// </summary>
    public readonly float CHUNKSCALE = 1f;

    /// <summary>
    /// the number of vertices per line. This is a constant because of the square limit of each mesh.
    /// </summary>
    /// { 48, 72, 96, 120, 144, 168, 192, 216, 240 };
    public readonly int CHUNKSIZE = 97;

    public Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

    /// <summary>
    /// The Size of the spawnable grid on a chunk.
    /// </summary>
    public float GRIDSIZE
    {
        get
        {
            return (CHUNKSIZE - 3) * CHUNKSCALE;
        }
    }

    public ChunkMap(int mapSize, TerrainSettings terrainSettings)
    {
        this.mapSize = mapSize - 1;
        this.terrainSettings = terrainSettings;
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

                if (mapSize == 0)
                {
                    edgeType = Edge.All;
                }

                Chunk chunk = CreateChunk(position, edgeType);
                chunks.Add(position, chunk);
            }
        }

        mapCenterWorld = new Vector2(((mapSize) * CHUNKSIZE * CHUNKSCALE) / 2, ((mapSize) * CHUNKSIZE * CHUNKSCALE) / 2);
    }

    /// <summary>
    /// This function creates the chunk with its heightmap and adds to the list of chunks.
    /// </summary>
    /// <param name="position">the chunk position</param>
    /// <param name="edgeType">the edge type of the chunk</param>
    private Chunk CreateChunk(Vector2 position, Edge edgeType)
    {
        Vector2 chunkWorldPos = position * GRIDSIZE;
        Vector2 chunkCenterWorldPosition = chunkWorldPos / CHUNKSCALE;

        HeightMap heightMap = new HeightMap(CHUNKSIZE, terrainSettings.heightMapSettings, chunkCenterWorldPosition);

        Bounds bounds = new Bounds(chunkCenterWorldPosition, Vector2.one * GRIDSIZE);
        Chunk chunk = new Chunk(chunkCenterWorldPosition, position, heightMap, edgeType, bounds, CHUNKSIZE, CHUNKSCALE, EDGEDEPTH, GRIDSIZE, terrainSettings);
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