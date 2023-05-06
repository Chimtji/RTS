using UnityEngine;
using System;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public bool autoUpdate;

    [Header("Terrain")]
    public TerrainSettings terrainSettings;

    [Header("Vegetation")]
    public TerrainObject[] vegetations;


    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        ChunkMap chunkMap = new ChunkMap(2, terrainSettings.heightMapSettings);

        MeshGenerator meshGenerator = new MeshGenerator();
        ObjectGenerator objectGenerator = new ObjectGenerator();

        // Place Terrain
        meshGenerator.Generate(terrainSettings, chunkMap, transform);

        // Place Vegetation Objects
        foreach (TerrainObject vegetation in vegetations)
        {
            objectGenerator.Generate(vegetation, chunkMap, transform);
            foreach (KeyValuePair<Vector3, GameObject> item in objectGenerator.objects)
            {
                var obj = Instantiate(item.Value, item.Key, Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0));
                obj.transform.parent = objectGenerator.container.transform;
            }
        }
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
            foreach (TerrainObject vegetation in vegetations)
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