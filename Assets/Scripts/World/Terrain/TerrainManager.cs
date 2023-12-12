using UnityEngine;
using System.Collections.Generic;
using Trout.Utils;
using UnityEngine.UI;
using System;
using Trout.Types;

public class TerrainManager : Generator
{
    /// <summary>
    /// The settings used to generate the terrain
    /// </summary>
    public TerrainSettings settings;
    public GameSettings gameSettings;

    /// <summary>
    /// The dictionary that holds each generated chunk
    /// </summary>
    public ChunkMap chunks = new ChunkMap();

    public ChunkMap chunkies { get; set; }

    private void Update()
    {
        foreach (KeyValuePair<ChunkPosition, TerrainChunk> chunkData in chunks)
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

        for (int x = 0; x < CalcMapSize(); x++)
        {
            for (int z = 0; z < CalcMapSize(); z++)
            {
                ChunkPosition chunkPosition = new ChunkPosition(x, z);

                ChunkPositionName edgeType = GetEdgeType(x, z);
                bool isStartLocation = CalcStartLocation(edgeType);

                TerrainChunk chunk = new TerrainChunk(chunkPosition, settings, transform, isStartLocation, edgeType);
                chunks.Add(chunkPosition, chunk);
            }
        }

        chunkies = chunks;
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

    public ChunkPosition WorldToChunkPosition(Vector3 position)
    {
        float chunkX = Mathf.Floor((position.x + settings.gridSize / 2) / settings.gridSize);
        float chunkZ = Mathf.Floor((position.z + settings.gridSize / 2) / settings.gridSize);

        return new ChunkPosition(chunkX, chunkZ);
    }

    private int CalcMapSize()
    {
        int size = gameSettings.players.Length * gameSettings.players.Length;

        // Right now we only support 2 - 4 players. We want to support more later.
        if (size > 9)
        {
            size = 9;
        }

        // We just want to know the length of each side
        return Mathf.FloorToInt(Mathf.Sqrt(size));
    }

    private bool CalcStartLocation(ChunkPositionName edge)
    {
        int numOfPlayers = gameSettings.players.Length;

        switch (edge)
        {

            case ChunkPositionName.Top:
                return false;
            case ChunkPositionName.TopLeft:
                return true;
            case ChunkPositionName.Right:
                return false;
            case ChunkPositionName.TopRight:
                if (numOfPlayers >= 3)
                {
                    return true;
                }
                return false;
            case ChunkPositionName.Bottom:
                if (numOfPlayers == 3)
                {
                    return true;
                }
                return false;
            case ChunkPositionName.BottomLeft:
                if (numOfPlayers == 4)
                {
                    return true;
                }
                return false;
            case ChunkPositionName.BottomRight:
                if (numOfPlayers == 2 || numOfPlayers == 4)
                {
                    return true;
                }
                return false;
            case ChunkPositionName.Left:
                return false;
            default:
                Debug.LogError("Invalid edge position specified.");
                return false;
        }
    }

    private ChunkPositionName GetEdgeType(int x, int z)
    {
        int mapSize = CalcMapSize() - 1;

        if (x == 0)
        {
            if (z == 0)
            {
                return ChunkPositionName.BottomLeft;
            }
            else if (z == mapSize)
            {
                return ChunkPositionName.TopLeft;

            }
            else
            {
                return ChunkPositionName.Left;
            }
        }
        else if (x == mapSize)
        {
            if (z == 0)
            {
                return ChunkPositionName.BottomRight;

            }
            else if (z == mapSize)
            {
                return ChunkPositionName.TopRight;

            }
            else
            {
                return ChunkPositionName.Right;
            }
        }
        else if (z == 0)
        {
            return ChunkPositionName.Bottom;
        }
        else if (z == mapSize)
        {
            return ChunkPositionName.Top;

        }

        return ChunkPositionName.Middle;
    }

}