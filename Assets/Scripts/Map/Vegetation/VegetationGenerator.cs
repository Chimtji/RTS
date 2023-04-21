using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trout.Utils;

public class VegetationGenerator : MonoBehaviour
{
    public GameObject terrain;
    public VegetationSettings vegetationSettings;

    Dictionary<Vector3, GameObject> trees = new Dictionary<Vector3, GameObject>();

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        GenerateTrees();
    }

    public void GenerateTrees()
    {
        Utils.ClearChildren(transform);
        trees.Clear();

        TerrainGenerator terrainGenerator = terrain.GetComponent<TerrainGenerator>();
        foreach (KeyValuePair<Vector2, TerrainChunk> terrainChunk in terrainGenerator.terrain)
        {
            TerrainChunk chunk = terrain.GetComponent<TerrainGenerator>().terrain[terrainChunk.Key];
            TerrainChunkMesh chunkMesh = chunk.mesh;

            HeightMap chunkTreeMap = HeightMapGenerator.GenerateHeightMap(chunk.meshSettings.numVertsPerLine, chunk.meshSettings.numVertsPerLine, vegetationSettings, Vector2.zero);
            int chunkSize = chunkMesh.vertices.GetLength(0);

            int pointLocalOffset = chunkTreeMap.values.GetLength(0) / 2;

            for (int i = 0; i < chunkSize; i++)
            {
                Vector3 chunkPoint = chunkMesh.vertices[i];

                float pointX = chunkPoint.x + pointLocalOffset;
                float pointZ = chunkPoint.z + pointLocalOffset;
                float height = chunkPoint.y;

                float treeNoiseAtPoint = chunkTreeMap.values[Mathf.FloorToInt(pointX), Mathf.FloorToInt(pointZ)];

                Vector2 worldPoint = chunk.ChunkPosToWorldPos(pointX, pointZ);
                Vector3 worldPosition = new Vector3(worldPoint.x - pointLocalOffset, height, worldPoint.y - pointLocalOffset);

                if (!trees.ContainsKey(worldPosition))
                {
                    if (chunkPoint.y > vegetationSettings.treeMinHeight && chunkPoint.y < vegetationSettings.treeMaxHeight)
                    {
                        if (treeNoiseAtPoint > vegetationSettings.treeNoiseMin && treeNoiseAtPoint < vegetationSettings.treeNoiseMax)
                        {
                            trees.Add(worldPosition, vegetationSettings.treePrefab);
                        }
                    }
                }
            }




        }


        foreach (KeyValuePair<Vector3, GameObject> tree in trees)
        {
            GameObject instantiatedTree = Instantiate(vegetationSettings.treePrefab, tree.Key, Quaternion.identity);
            instantiatedTree.transform.SetParent(transform);
        }
    }

    void OnValidate()
    {
        if (vegetationSettings != null)
        {
            vegetationSettings.OnValuesUpdated -= OnValuesUpdated;
            vegetationSettings.OnValuesUpdated += OnValuesUpdated;
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
