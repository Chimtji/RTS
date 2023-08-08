using UnityEngine;
using System;
using System.Collections.Generic;
using Trout.Utils;

public class MapGenerator : MonoBehaviour
{
    public bool autoUpdate;

    [Header("Terrain")]
    public TerrainSettings terrainSettings;

    [Header("Vegetation")]
    public Vegetation[] vegetations;


    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        // ChunkMap chunkMap = new ChunkMap(2, terrainSettings);

        // TerrainGenerator terrainGenerator = new TerrainGenerator();
        // ObjectGenerator objectGenerator = new ObjectGenerator();

        // Place Terrain
        // terrainGenerator.Generate(terrainSettings, chunkMap, transform);


        // foreach (KeyValuePair<Vector2, Chunk> chunkPair in chunkMap.chunks)
        // {
        //     Chunk chunk = chunkPair.Value;
        //     Dictionary<Vector2, Lake> lakes = chunk.GetLakes();
        //     foreach (KeyValuePair<Vector2, Lake> lakePair in lakes)
        //     {
        //         var waterObject = Instantiate(new GameObject("Water"), new Vector3(lakePair.Key.x, 0, lakePair.Key.y), Quaternion.identity);
        //     }

        // }

        // Place Water
        // Transform waterContainer = transform.Find("Water");
        // Utils.ClearChildren(waterContainer);
        // var water = Instantiate(terrainSettings.waterPrefab, new Vector3(chunkMap.mapCenterWorld.x, 0, chunkMap.mapCenterWorld.y), Quaternion.identity);
        // water.transform.name = "WaterElement";
        // water.transform.parent = waterContainer;
        // water.transform.position = new Vector3(-(chunkMap.chunks[new Vector2(0, 0)].gridSize - 2f) / 2f, 0, -(chunkMap.chunks[new Vector2(0, 0)].gridSize - 2f) / 2f);

        // Place Vegetation Objects
        // foreach (TerrainObject vegetation in vegetations)
        // {
        //     objectGenerator.Generate(vegetation, chunkMap, transform);
        //     foreach (KeyValuePair<Vector3, GameObject> item in objectGenerator.objects)
        //     {
        //         var vegetationObject = Instantiate(item.Value, item.Key, Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0));
        //         vegetationObject.transform.parent = objectGenerator.container.transform;
        //     }
        // }
    }

    void OnValidate()
    {
        if (terrainSettings != null)
        {
            terrainSettings.OnValuesUpdated -= OnValuesUpdated;
            terrainSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (vegetations != null)
        {
            foreach (Vegetation vegetation in vegetations)
            {
                if (vegetation != null)
                {
                    vegetation.OnValuesUpdated -= OnValuesUpdated;
                    vegetation.OnValuesUpdated += OnValuesUpdated;

                }

            }
        }
    }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            Generate();
        }
    }
}