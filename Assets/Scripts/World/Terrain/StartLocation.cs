using Trout.Types;
using UnityEngine;

public class StartLocation
{
    /// <summary>
    /// Determines If this location is available.
    /// </summary>
    public bool Enabled
    {
        get
        {
            return CalcLocations() > 0;
        }
    }

    /// <summary>
    /// The position of the the chunk this startLocation is on
    /// </summary>
    public ChunkPosition chunkPosition;

    /// <summary>
    /// The world position of the exact spawn location / center of this start location. 
    /// Beware: This is set async and not on initialization. Atm this is set during terrain generation (Heightmap)
    /// </summary>
    public Vector3 SpawnPosition;

    /// <summary>
    /// The number of this start location in relation to the others.
    /// </summary>
    /// <value></value>
    public int Index
    {
        get
        {
            return CalcLocations();
        }
    }

    /// <summary>
    /// Whether this location is in a corner, at the bottom, top or somewhere else.
    /// </summary>
    private readonly ChunkPositionName side;

    /// <summary>
    /// The amount of players in the map / game
    /// </summary>
    private readonly int numOfPlayers;

    public StartLocation(ChunkPositionName side, int numOfPlayers, ChunkPosition chunkPosition)
    {
        this.side = side;
        this.numOfPlayers = numOfPlayers;
        this.chunkPosition = chunkPosition;
    }

    private int CalcLocations()
    {
        switch (side)
        {

            case ChunkPositionName.Top:
                return 0;
            case ChunkPositionName.TopLeft:
                return 1;
            case ChunkPositionName.Right:
                return 0;
            case ChunkPositionName.TopRight:
                if (numOfPlayers >= 3)
                {
                    return 3;
                }
                return 0;
            case ChunkPositionName.Bottom:
                if (numOfPlayers == 3)
                {
                    return 3;
                }
                return 0;
            case ChunkPositionName.BottomLeft:
                if (numOfPlayers == 4)
                {
                    return 4;
                }
                return 0;
            case ChunkPositionName.BottomRight:
                if (numOfPlayers == 2 || numOfPlayers == 4)
                {
                    return 2;
                }
                return 0;
            case ChunkPositionName.Left:
                return 0;
            default:
                Debug.LogError("Invalid edge position specified.");
                return 0;
        }
    }
}