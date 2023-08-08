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
    /// The total size of the chunk
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

    /// <summary>
    /// The mesh generated in chunk
    /// </summary>
    public Mesh mesh;

    Bounds bounds;

    /// <summary>
    /// The size of the spawnable part of the chunk
    /// </summary>
    public float gridSize;

    TerrainSettings terrainSettings;

    public Chunk(Vector2 worldPosition, Vector2 chunkPosition, HeightMap heightMap, Edge edgeType, Bounds bounds, int size, float scale, float edgeDepth, float gridSize, TerrainSettings terrainSettings)
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
        this.terrainSettings = terrainSettings;
    }

    public float GetAvgTileHeight(float x, float z)
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
        float height = GetAvgTileHeight(x, z);

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

    public List<List<Vector3>> GetLakes()
    {
        List<Vector3> chunkGrid = GetChunkGrid();
        float maxHeight = terrainSettings.groundMinHeight;
        List<Vector3> waterTiles = chunkGrid.FindAll(delegate (Vector3 coord) { return coord.y < maxHeight; });

        // Create a dictionary to store the neighbors of each vector
        Dictionary<Vector3, List<Vector3>> neighborsDict = new Dictionary<Vector3, List<Vector3>>();

        // Populate the neighborsDict with neighbors for each vector
        for (int i = 0; i < waterTiles.Count; i++)
        {
            Vector3 vector = waterTiles[i];
            neighborsDict[vector] = new List<Vector3>();

            for (int j = 0; j < waterTiles.Count; j++)
            {
                if (i != j && IsVectorWithinThreshold(vector, waterTiles[j]))
                {
                    neighborsDict[vector].Add(waterTiles[j]);
                }
            }
        }

        // Perform DFS to group the vectors
        HashSet<Vector3> visited = new HashSet<Vector3>();
        List<List<Vector3>> groupedVectors = new List<List<Vector3>>();

        foreach (Vector3 vector in waterTiles)
        {
            if (!visited.Contains(vector))
            {
                List<Vector3> group = new List<Vector3>();
                DFS(vector, neighborsDict, visited, group);
                groupedVectors.Add(group);
            }
        }

        return groupedVectors;
    }

    private void DFS(Vector3 vector, Dictionary<Vector3, List<Vector3>> neighborsDict, HashSet<Vector3> visited, List<Vector3> group)
    {
        visited.Add(vector);
        group.Add(vector);

        foreach (Vector3 neighbor in neighborsDict[vector])
        {
            if (!visited.Contains(neighbor))
            {
                DFS(neighbor, neighborsDict, visited, group);
            }
        }
    }

    private bool IsVectorWithinThreshold(Vector3 vectorA, Vector3 vectorB)
    {
        float distanceThreshold = 1.2f; // Adjust the threshold as needed
        float distance = Vector3.Distance(vectorA, vectorB);
        return distance <= distanceThreshold;
    }



    private List<Vector2> GetLocalGrid()
    {
        List<Vector2> coords = new List<Vector2>();

        // The edge (the mesh that makes the walls that go downwards on each side) takes 1 coord length on each side
        int edge = 1;

        // DONT KNOW: For some reason we have to start at x, z = 1 + edge. Don't know why we can't start at zero and 
        // simply position center accordingly. There may be some mismatch in position of noise, chunk and grid.
        for (int x = 1 + edge; x <= gridSize - edge; x++)
        {
            for (int z = 1 + edge; z <= gridSize - edge; z++)
            {
                coords.Add(new Vector2(x, z));
            }
        }

        return coords;
    }
}

public class Lake
{
    public List<Vector3> waters = new List<Vector3>();
}

public enum Edge
{
    Top, Left, Right, Bottom, None, TopLeft, TopRight, BottomLeft, BottomRight, All

}