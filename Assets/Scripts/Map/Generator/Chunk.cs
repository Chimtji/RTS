using UnityEngine;

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

    Bounds bounds;

    public Chunk(Vector2 worldPosition, Vector2 chunkPosition, HeightMap heightMap, Edge edgeType, Bounds bounds, int size, float scale, float edgeDepth)
    {
        this.worldPosition = worldPosition;
        this.chunkPosition = chunkPosition;
        this.heightMap = heightMap;
        this.edgeType = edgeType;
        this.bounds = bounds;
        this.size = size;
        this.scale = scale;
        this.edgeDepth = edgeDepth;
    }

    /// <summary>
    /// Converts a local coordinate inside a chunk to a world coordinate.
    /// </summary>
    /// <param name="x">the x position of coord</param>
    /// <param name="z">the z position of coord</param>
    public Vector2 ChunkCoordToWorldCoord(float x, float z)
    {
        float coordX = x + worldPosition.x;
        float coordZ = z + worldPosition.y;

        Vector2 worldCoord = new Vector2(coordX, coordZ);

        return worldCoord;
    }
}

public enum Edge
{
    Top, Left, Right, Bottom, None, TopLeft, TopRight, BottomLeft, BottomRight
}
