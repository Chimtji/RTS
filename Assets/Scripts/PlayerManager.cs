using System.Collections;
using System.Collections.Generic;
using Trout.Types;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : Generator
{
    public GameObject gameManager;
    public GameObject terrainManager;
    public GameObject buildManager;
    public GameObject uiManager;
    public GameObject dataManager;

    private GameSettings settings
    {
        get
        {
            return gameManager.GetComponent<GameManager>().settings;
        }
    }

    private TerrainManager terrain
    {
        get
        {
            return terrainManager.GetComponent<TerrainManager>();
        }
    }

    private ChunkMap chunkMap
    {
        get
        {
            return terrain.chunks;
        }
    }

    private GameObject buildings;
    private GameObject units;
    private GameObject uiActions;
    private GameObject uiInformation;

    public override void Generate()
    {
        dataManager.GetComponent<DataManager>().ClearAll();
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        foreach (KeyValuePair<ChunkPosition, TerrainChunk> chunkData in chunkMap)
        {
            TerrainChunk chunk = chunkData.Value;
            StartLocation startLocation = chunk.heightMap.startLocation;

            if (startLocation.Enabled && startLocation.Index == settings.playerNumber)
            {
                Player player = settings.player;
                Vector3 position = terrain.GetTile(chunk.heightMap.startLocation.SpawnPosition).bottomLeftCorner;
                BuildingData buildingData = player.civilization.headquarter;
                BuildManager builder = buildManager.GetComponent<BuildManager>();

                builder.CreateBuilding(buildingData, position);

                break;
            }
        }
    }
}
