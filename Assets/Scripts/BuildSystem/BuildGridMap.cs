using UnityEngine;
using System.Collections.Generic;
using Trout.Utils;
using Trout.Types;

public class BuildGridMap : Generator
{
    public GameObject terrain;
    public string gridLayer;
    public Material material;
    private GameObject gridContainer;
    List<GameObject> chunks = new List<GameObject>();

    private void Start()
    {
        Generate();
    }

    private void Update()
    {
        Rebuild();
    }

    public override void Generate()
    {
        chunks.Clear();
        Utils.ClearChildren(gameObject);
        gridContainer = new GameObject("Building Grid");
        gridContainer.transform.parent = gameObject.transform;
        SetBuildMode(false);


        ChunkMap terrainChunks = terrain.GetComponent<TerrainManager>().chunks;

        foreach (KeyValuePair<ChunkPosition, TerrainChunk> chunkData in terrainChunks)
        {
            TerrainChunk chunk = chunkData.Value;
            GameObject chunkObj = new GameObject("Chunk");
            chunkObj.transform.parent = gridContainer.transform;
            chunkObj.transform.position = chunk.worldPosition;

            chunkObj.layer = LayerMask.NameToLayer(gridLayer);

            MeshRenderer meshRenderer = chunkObj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = chunkObj.AddComponent<MeshFilter>();
            MeshCollider meshCollider = chunkObj.AddComponent<MeshCollider>();
            meshRenderer.sharedMaterial = material;
            meshFilter.sharedMesh = chunk.mesh;

            chunks.Add(chunkObj);
        }
    }

    public void SetBuildMode(bool buildMode)
    {
        gridContainer.SetActive(buildMode);
    }

    private void Rebuild()
    {
        foreach (GameObject chunk in chunks)
        {
            MeshCollider chunkCollider = chunk.GetComponent<MeshCollider>();
            MeshFilter meshFilter = chunk.GetComponent<MeshFilter>();
            chunkCollider.sharedMesh = meshFilter.mesh;
        }
    }
}