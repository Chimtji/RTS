using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trout.Utils;

public class MeshGenerator
{
    Material material;
    GameObject container;

    public void Generate(TerrainSettings terrainSettings, ChunkMap chunkMap, Transform outerContainer)
    {
        Utils.RemoveObject("Terrain");
        this.container = new GameObject("Terrain");
        container.transform.parent = outerContainer;
        this.material = terrainSettings.material;

        CreateChunkMesh(terrainSettings.containerName, chunkMap);
    }

    void CreateChunkMesh(string meshName, ChunkMap chunkMap)
    {

        foreach (KeyValuePair<Vector2, Chunk> item in chunkMap.chunks)
        {
            Chunk chunk = item.Value;

            GameObject meshObject = new GameObject(meshName);
            MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
            MeshCollider meshCollider = meshObject.AddComponent<MeshCollider>();
            meshRenderer.material = material;

            meshObject.transform.position = new Vector3(chunk.worldPosition.x, 0, chunk.worldPosition.y);
            meshObject.transform.parent = container.transform;

            ChunkMesh chunkMesh = new ChunkMesh(chunk);
            chunk.mesh = chunkMesh.mesh;
            meshFilter.sharedMesh = chunkMesh.mesh;
        }
    }
}