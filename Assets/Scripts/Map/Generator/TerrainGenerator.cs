using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trout.Utils;

public class TerrainGenerator : MonoBehaviour
{
    public bool autoUpdate;

    [Header("Landscape")]
    public TerrainSettings terrainSettings;

    [Header("Weather")]
    public Material cloudMaterial;
    public float cloudHeight;
    public Mesh cloudMesh;

    [Header("Vegetation")]
    public Vegetation[] vegetations;


    ChunkMap chunkMap;

    // Containers
    GameObject groundContainer;
    GameObject lakesContainer;
    GameObject treesContainers;
    GameObject cloudsContainer;

    //Generators
    CloudGenerator cloudGenerator;

    void Start()
    {
        Generate();
    }

    void Update()
    {
        Regenerate();
    }

    public void Generate()
    {
        Utils.ClearChildren(transform);

        CreateGround();
        CreateLakes();
        CreateVegetation();
        CreateClouds();
    }

    void CreateGround()
    {
        chunkMap = new ChunkMap(2, terrainSettings);

        groundContainer = new GameObject("Ground");
        groundContainer.transform.parent = transform;

        foreach (KeyValuePair<Vector2, Chunk> item in chunkMap.chunks)
        {
            Chunk chunk = item.Value;

            GameObject meshObject = new GameObject("Ground Chunk");
            MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
            MeshCollider meshCollider = meshObject.AddComponent<MeshCollider>();
            meshRenderer.sharedMaterial = terrainSettings.material;

            meshRenderer.sharedMaterial.SetFloat("_Water_Height", terrainSettings.waterMinHeight);
            meshRenderer.sharedMaterial.SetFloat("_Ground_Height", terrainSettings.groundMinHeight);
            meshRenderer.sharedMaterial.SetFloat("_Mountain_Height", terrainSettings.mountainMinHeight);

            meshObject.transform.position = new Vector3(chunk.worldPosition.x, 0, chunk.worldPosition.y);
            meshObject.transform.parent = groundContainer.transform;

            ChunkMesh chunkMesh = new ChunkMesh(chunk);
            chunk.mesh = chunkMesh.mesh;
            meshFilter.sharedMesh = chunkMesh.mesh;
        }

    }

    void CreateLakes()
    {
        lakesContainer = new GameObject("Lakes");
        lakesContainer.transform.parent = transform;

        foreach (KeyValuePair<Vector2, Chunk> chunk in chunkMap.chunks)
        {
            Vector2 chunkPosition = chunk.Key;
            Chunk chunkData = chunk.Value;


            List<List<Vector3>> lakes = chunkData.GetLakes();
            foreach (List<Vector3> lake in lakes)
            {
                GameObject lakeObject = new GameObject("Lake");
                lakeObject.transform.parent = lakesContainer.transform;

                foreach (Vector3 coord in lake)
                {
                    var waterObject = Instantiate(terrainSettings.waterPrefab, new Vector3(coord.x, terrainSettings.waterMaxHeight / 2f, coord.z), Quaternion.identity);
                    waterObject.transform.parent = lakeObject.transform;

                }
            }
        }
    }

    void CreateVegetation()
    {
        VegetationGenerator objectGenerator = new VegetationGenerator();

        foreach (Vegetation vegetation in vegetations)
        {
            objectGenerator.Generate(vegetation, chunkMap, transform);
            foreach (KeyValuePair<Vector3, GameObject> item in objectGenerator.objects)
            {
                var vegetationObject = Instantiate(item.Value, item.Key, Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0));
                vegetationObject.transform.parent = objectGenerator.container.transform;
            }
        }
    }

    void CreateClouds()
    {

        Debug.Log(chunkMap.mapSizeWorld);

        cloudsContainer = new GameObject("Clouds");
        cloudsContainer.transform.parent = transform;

        MeshRenderer meshRenderer = cloudsContainer.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = cloudsContainer.AddComponent<MeshFilter>();
        MeshCollider meshCollider = cloudsContainer.AddComponent<MeshCollider>();
        meshRenderer.sharedMaterial = cloudMaterial;
        meshFilter.mesh = cloudMesh;


        cloudsContainer.transform.position = new Vector3(chunkMap.mapCenterWorld.x, 10f, chunkMap.mapCenterWorld.y);
        cloudsContainer.transform.localScale = new Vector3(chunkMap.mapSizeWorld, chunkMap.mapSizeWorld, 0f);



        cloudGenerator = new CloudGenerator(cloudMaterial, cloudHeight, cloudMesh, cloudsContainer);
        cloudGenerator.Generate();
    }

    void Regenerate()
    {
        cloudGenerator.Generate();
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
