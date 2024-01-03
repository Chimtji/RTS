using UnityEngine;
using System.Collections.Generic;
using Trout.Utils;
using Trout.Types;

public class TerrainManager : Generator
{

    /// <summary>
    /// The Game Manager Object in the scene
    /// </summary>
    public GameObject gameManager;

    /// <summary>
    /// The UI Manager Object in the scene
    /// </summary>
    public GameObject uiManager;

    /// <summary>
    /// The dictionary that holds each generated chunk
    /// </summary>
    public ChunkMap chunks = new ChunkMap();

    /// <summary>
    /// The settings used to generate the terrain
    /// </summary>
    private TerrainSettings settings
    {
        get
        {
            return gameManager.GetComponent<GameManager>().settings.mapSettings;
        }
    }

    /// <summary>
    /// The General settings of the game
    /// </summary>
    private GameSettings gameSettings
    {
        get
        {
            return gameManager.GetComponent<GameManager>().settings;
        }
    }

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
                StartLocation startLocation = new StartLocation(edgeType, gameSettings.amountOfPlayers, chunkPosition);

                TerrainChunk chunk = new TerrainChunk(chunkPosition, settings, transform, startLocation, edgeType);

                TerrainShared shared = chunk.gameObject.AddComponent<TerrainShared>();
                shared.terrainManager = this;
                shared.uiManager = uiManager.GetComponent<UIManager>();
                shared.gameManager = gameManager.GetComponent<GameManager>();


                chunks.Add(chunkPosition, chunk);
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

    public ChunkPosition WorldToChunkPosition(Vector3 position)
    {
        float chunkX = Mathf.Floor((position.x + settings.gridSize / 2) / settings.gridSize);
        float chunkZ = Mathf.Floor((position.z + settings.gridSize / 2) / settings.gridSize);

        return new ChunkPosition(chunkX, chunkZ);
    }

    private int CalcMapSize()
    {
        int size = gameSettings.amountOfPlayers * gameSettings.amountOfPlayers;

        // Right now we only support 2 - 4 players. We want to support more later.
        if (size > 9)
        {
            size = 9;
        }

        // We just want to know the length of each side
        return Mathf.FloorToInt(Mathf.Sqrt(size));
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