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
        TerrainManager terrainManager = terrain.GetComponent<TerrainManager>();
        ChunkMap terrainChunks = terrainManager.chunks;

        foreach (KeyValuePair<ChunkPosition, TerrainChunk> chunkData in terrainChunks)
        {
            TerrainChunk chunk = chunkData.Value;

            if (chunk.heightMap.startLocation.enabled)
            {
                Tile tile = terrainManager.GetTile(chunk.heightMap.startLocation.position);
                Building building = new Building(settings.players[0].civilization.townHall, tile.bottomLeftCorner);
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
