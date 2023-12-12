using UnityEngine;

public struct StartLocation
{
    public bool enabled;
    public Vector3 position;
    public ChunkPositionName side;

    public StartLocation(bool enabled, Vector3 position, ChunkPositionName side)
    {
        this.enabled = enabled;
        this.position = position;
        this.side = side;
    }
}