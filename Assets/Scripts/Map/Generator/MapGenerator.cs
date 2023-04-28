using UnityEngine;
using System;
using System.Collections.Generic;
public class MapGenerator : MonoBehaviour
{
    public bool autoUpdate;

    [Header("Terrain")]
    public Transform terrainContainer;
    public TerrainSettings terrainSettings;

    [Header("Vegetation")]
    public TerrainObject[] vegetations;


    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        ChunkMap chunkMap = new ChunkMap(3, terrainSettings.heightMapSettings);

        MeshGenerator meshGenerator = new MeshGenerator();
        ObjectGenerator objectGenerator = new ObjectGenerator();

        // Place Terrain
        meshGenerator.Generate("Terrain Chunk", terrainSettings, chunkMap, terrainContainer);

        // Place Vegetation Objects
        foreach (TerrainObject vegetation in vegetations)
        {
            objectGenerator.Generate(vegetation);
        }
    }

    void OnValidate()
    {
        if (terrainSettings != null)
        {
            terrainSettings.OnValuesUpdated -= OnValuesUpdated;
            terrainSettings.OnValuesUpdated += OnValuesUpdated;
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