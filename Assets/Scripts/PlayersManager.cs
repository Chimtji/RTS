using System.Collections;
using System.Collections.Generic;
using Trout.Types;
using UnityEngine;

public class PlayersManager : Generator
{
    public GameSettings settings;
    public GameObject terrain;

    public override void Generate()
    {
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        ChunkMap terrainChunks = terrain.GetComponent<TerrainManager>().chunks;

        foreach (KeyValuePair<ChunkPosition, TerrainChunk> chunkData in terrainChunks)
        {
            TerrainChunk chunk = chunkData.Value;

            if (chunk.heightMap.startLocation.enabled)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = chunk.heightMap.startLocation.position;
            }
        }
    }
    // public PlayerAttributes[] playersAttributes;
    // void Start()
    // {
    //     foreach (PlayerAttributes playerAttributes in playersAttributes)
    //     {
    //         Player player = new Player(playerAttributes);
    //     }
    // }
}
