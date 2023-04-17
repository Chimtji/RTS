using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{
    public int colliderLODIndex;
    public LODInfo[] detailLevels;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureSettings;
    public MapSettings mapSettings;

    public Transform viewer;
    public Material mapMaterial;

    Vector2 viewerPosition;


    void Start()
    {
        textureSettings.ApplyToMaterial(mapMaterial);
        textureSettings.UpdateMeshHeights(mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);
        PlaceChunks();
    }

    void PlaceChunks()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        for (int x = 0; x <= mapSettings.size - 1; x++)
        {
            for (int z = 0; z <= mapSettings.size - 1; z++)
            {
                Vector2 position = new Vector2(x, z);
                TerrainChunk chunk = new TerrainChunk(position, heightMapSettings, meshSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial);
                chunk.Load();

            }
        }
    }
}

[System.Serializable]
public struct LODInfo
{
    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int lod;
    public float visibleDstThreshold;
    public float sqrVisibleDstThreshold
    {
        get
        {
            return visibleDstThreshold * visibleDstThreshold;
        }
    }
}