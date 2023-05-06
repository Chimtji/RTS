using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Trout.Utils;

[System.Serializable]
public class ObjectGenerator
{
    public GameObject container;

    public Dictionary<Vector3, GameObject> objects = new Dictionary<Vector3, GameObject>();

    public void Generate(TerrainObject terrainObject, ChunkMap chunkMap, Transform outerContainer)
    {
        Utils.RemoveObject(terrainObject.containerName);


        this.container = new GameObject(terrainObject.containerName);
        container.transform.parent = outerContainer;

        foreach (KeyValuePair<Vector2, Chunk> item in chunkMap.chunks)
        {
            Chunk chunk = item.Value;
            NoiseMap noiseMap = new NoiseMap(chunk.size, terrainObject.noiseSettings, item.Key);
            List<Vector3> chunkGrid = chunk.GetChunkGrid();

            chunkGrid.ForEach(coord =>
            {
                if (!objects.ContainsKey(coord))
                {
                    float noiseAtCoord = noiseMap.values[
                        (int)chunk.ToLocalPosition(coord).x,
                        (int)chunk.ToLocalPosition(coord).z
                    ];

                    if (coord.y < terrainObject.maxSpawnHeight && coord.y > terrainObject.minSpawnHeight)
                    {
                        if (noiseAtCoord > 0.2 && noiseAtCoord < 0.4)
                        {
                            objects.Add(coord, terrainObject.prefab);
                        }
                    }
                }
            });
        }
    }
}