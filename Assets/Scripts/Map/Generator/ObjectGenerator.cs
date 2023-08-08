using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Trout.Utils;

[System.Serializable]
public class VegetationGenerator
{
    public GameObject container;

    public Dictionary<Vector3, GameObject> objects = new Dictionary<Vector3, GameObject>();

    public void Generate(Vegetation vegetation, ChunkMap chunkMap, Transform outerContainer)
    {
        Utils.RemoveObject(vegetation.containerName);


        this.container = new GameObject(vegetation.containerName);
        container.transform.parent = outerContainer;

        foreach (KeyValuePair<Vector2, Chunk> item in chunkMap.chunks)
        {
            Chunk chunk = item.Value;
            NoiseMap noiseMap = new NoiseMap(chunk.size, vegetation.noiseSettings, item.Key);
            List<Vector3> chunkGrid = chunk.GetChunkGrid();

            chunkGrid.ForEach(coord =>
            {
                if (!objects.ContainsKey(coord))
                {
                    float noiseAtCoord = noiseMap.values[
                        (int)chunk.ToLocalPosition(coord).x,
                        (int)chunk.ToLocalPosition(coord).z
                    ];

                    if (coord.y < vegetation.maxSpawnHeight && coord.y > vegetation.minSpawnHeight)
                    {
                        if (noiseAtCoord > 0.2 && noiseAtCoord < 0.4)
                        {
                            objects.Add(coord, vegetation.prefab);
                        }
                    }
                }
            });
        }
    }
}