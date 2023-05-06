using System.Collections.Generic;
using UnityEngine;
using Trout.Utils;
using System.Linq;

public class Chunk
{
    /// <summary>
    /// The scale of the chunk
    /// </summary>
    public float scale;
    /// <summary>
    /// The size of the chunk
    /// </summary>
    public int size;
    /// <summary>
    /// The center position of the chunk in the world
    /// </summary>
    public Vector2 worldPosition;

    /// <summary>
    /// The position of the chunk in relation to the other chunks.
    /// </summary>
    public Vector2 chunkPosition;

    /// <summary>
    /// The Heightmap generated for this chunk
    /// </summary>
    public HeightMap heightMap;

    /// <summary>
    /// The edge type this chunk has
    /// </summary>
    public Edge edgeType;
    /// <summary>
    /// The depth of the edge from height 0
    /// </summary>
    public float edgeDepth;

    public Mesh mesh;
    Bounds bounds;
    public float gridSize;

    public Chunk(Vector2 worldPosition, Vector2 chunkPosition, HeightMap heightMap, Edge edgeType, Bounds bounds, int size, float scale, float edgeDepth, float gridSize)
    {
        this.worldPosition = worldPosition;
        this.chunkPosition = chunkPosition;
        this.heightMap = heightMap;
        this.edgeType = edgeType;
        this.bounds = bounds;
        this.size = size;
        this.scale = scale;
        this.edgeDepth = edgeDepth;
        this.gridSize = gridSize;
    }

    public float GetAvgCellHeight(float x, float z)
    {
        float[] neighborHeights = {
                heightMap.values[(int)x, (int)z],
                heightMap.values[(int)x + 1, (int)z],
                heightMap.values[(int)x, (int)z + 1],
                heightMap.values[(int)x + 1, (int)z + 1]
            };

        float[] minMaxNeighbors = {
                neighborHeights.Max(),
                neighborHeights.Min(),
            };
        float height = Utils.GetAvegerage(minMaxNeighbors);

        return height;
    }

    public Vector3 ToLocalPosition(Vector3 coord)
    {
        // float height = GetAvgCellHeight(x, z);

        float coordX = (coord.x - worldPosition.x) + (size - 2) / 2;
        float coordZ = (coord.z - worldPosition.y) + (size - 2) / 2;

        Vector3 localCoord = new Vector3(coordX, coord.y, coordZ);

        return localCoord;
    }

    /// <summary>
    /// Converts a local position inside a chunk to a world position.
    /// </summary>
    /// <param name="x">the x position of coord</param>
    /// <param name="z">the z position of coord</param>
    public Vector3 ToWorldPosition(float x, float z)
    {
        float height = GetAvgCellHeight(x, z);

        float topLeftX = ((size - 1) / -2f) + worldPosition.x;
        float topLeftZ = ((size - 1) / 2f) + worldPosition.y;

        float coordX = (topLeftX + x) * scale + 0.5f;
        float coordY = height;
        float coordZ = (topLeftZ - z) * scale - 0.5f;

        Vector3 worldCoord = new Vector3(coordX, coordY, coordZ);

        return worldCoord;
    }

    public List<Vector3> GetChunkGrid()
    {
        List<Vector2> localCoords = GetLocalGrid();
        List<Vector3> worldCoords = new List<Vector3>();

        localCoords.ForEach(coord =>
        {
            Vector3 worldCoord = ToWorldPosition(coord.x, coord.y);
            Vector3 position = worldCoord;

            worldCoords.Add(position);
        });

        return worldCoords;
    }

    private List<Vector2> GetLocalGrid()
    {
        List<Vector2> coords = new List<Vector2>();

        for (int x = 1; x <= gridSize; x++)
        {
            for (int z = 1; z <= gridSize; z++)
            {
                coords.Add(new Vector2(x, z));
            }
        }

        return coords;
    }
}

public enum Edge
{
    Top, Left, Right, Bottom, None, TopLeft, TopRight, BottomLeft, BottomRight, All
}
