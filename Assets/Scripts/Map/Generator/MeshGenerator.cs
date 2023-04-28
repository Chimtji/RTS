using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trout.Utils;

public class MeshGenerator
{
    readonly int TEXTURESIZE = 512;
    readonly TextureFormat TEXTUREFORMAT = TextureFormat.RGB565;


    Material material;
    Transform container;
    float savedMinHeight;
    float savedMaxHeight;

    public void Generate(string meshName, TerrainSettings terrainSettings, ChunkMap chunkMap, Transform container)
    {
        this.container = container;
        this.material = terrainSettings.material;

        Utils.ClearChildren(container);
        CreateChunkMesh(meshName, chunkMap);

        ApplyToMaterial(terrainSettings.textureLayers);
        UpdateMeshHeights(terrainSettings.heightMapSettings.minHeight, terrainSettings.heightMapSettings.maxHeight);


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
            meshObject.transform.parent = container;

            ChunkMesh chunkMesh = new ChunkMesh(chunk);

            meshFilter.sharedMesh = chunkMesh.mesh;
        }
    }

    void ApplyToMaterial(TextureLayer[] layers)
    {
        material.SetInt("layerCount", layers.Length);
        material.SetColorArray("baseColours", layers.Select(x => x.tint).ToArray());
        material.SetFloatArray("baseStartHeights", layers.Select(x => x.startHeight).ToArray());
        material.SetFloatArray("baseBlends", layers.Select(x => x.blendStrength).ToArray());
        material.SetFloatArray("baseColourStrength", layers.Select(x => x.tintStrength).ToArray());
        material.SetFloatArray("baseTextureScales", layers.Select(x => x.textureScale).ToArray());
        Texture2DArray texturesArray = GenerateTextureArray(layers.Select(x => x.texture).ToArray());
        material.SetTexture("baseTextures", texturesArray);
    }

    void UpdateMeshHeights(float minHeight, float maxHeight)
    {
        savedMinHeight = minHeight;
        savedMaxHeight = maxHeight;

        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);
    }

    Texture2DArray GenerateTextureArray(Texture2D[] textures)
    {
        Texture2DArray textureArray = new Texture2DArray(TEXTURESIZE, TEXTURESIZE, textures.Length, TEXTUREFORMAT, true);
        for (int i = 0; i < textures.Length; i++)
        {
            textureArray.SetPixels(textures[i].GetPixels(), i);
        }
        textureArray.Apply();
        return textureArray;
    }
}