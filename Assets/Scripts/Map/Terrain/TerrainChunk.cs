using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    public Vector2 coord;
    public TerrainChunkMesh mesh;
    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;

    GameObject meshObject;
    Vector2 centerWorldPosition;
    Bounds bounds;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    HeightMap heightMap;



    public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, Transform parent, Material material, Edge edge)
    {
        this.coord = coord;
        this.heightMapSettings = heightMapSettings;
        this.meshSettings = meshSettings;

        centerWorldPosition = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
        bounds = new Bounds(centerWorldPosition, Vector2.one * meshSettings.meshWorldSize);


        meshObject = new GameObject("Terrain Chunk");
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshCollider = meshObject.AddComponent<MeshCollider>();
        meshRenderer.material = material;

        meshObject.transform.position = new Vector3(centerWorldPosition.x, 0, centerWorldPosition.y);
        meshObject.transform.parent = parent;

        heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, centerWorldPosition);
        mesh = MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, edge);

        meshFilter.sharedMesh = mesh.CreateMesh();

    }

    public Vector2 ChunkPosToWorldPos(float x, float y)
    {
        float coordX = x + centerWorldPosition.x;
        float coordY = y + centerWorldPosition.y;

        Vector2 worldPosition = new Vector2(coordX, coordY);

        return worldPosition;
    }
}
