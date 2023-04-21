using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Trout.Utils;

public class TerrainGenerator : MonoBehaviour
{
    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureSettings textureSettings;
    public GameSettings gameSettings;
    public Dictionary<Vector2, TerrainChunk> terrain = new Dictionary<Vector2, TerrainChunk>();

    void Start()
    {
        Generate();
    }

    public Dictionary<Vector2, TerrainChunk> Generate()
    {
        textureSettings.ApplyToMaterial();
        textureSettings.UpdateMeshHeights(heightMapSettings.minHeight, heightMapSettings.maxHeight);

        Utils.ClearChildren(transform);
        terrain.Clear();

        int mapSize = gameSettings.players.GetLength(0) - 1;
        for (int x = 0; x <= mapSize; x++)
        {
            for (int z = 0; z <= mapSize; z++)
            {
                Vector2 position = new Vector2(x, z);
                Edge edge = Edge.None;

                if (x == 0 && z == mapSize)
                {
                    edge = Edge.TopLeft;
                }
                else if (x == 0 && z == 0)
                {
                    edge = Edge.BottomLeft;
                }
                else if (x == mapSize && z == mapSize)
                {
                    edge = Edge.TopRight;
                }
                else if (x == mapSize && z == 0)
                {
                    edge = Edge.BottomRight;
                }
                else if (z == mapSize)
                {
                    edge = Edge.Top;
                }
                else if (z == 0)
                {
                    edge = Edge.Bottom;
                }
                else if (x == mapSize)
                {
                    edge = Edge.Right;
                }
                else if (x == 0)
                {
                    edge = Edge.Left;
                }

                TerrainChunk chunk = new TerrainChunk(position, heightMapSettings, meshSettings, transform, textureSettings.material, edge);
                terrain.Add(position, chunk);

            }
        }

        return terrain;
    }

    void OnValidate()
    {

        if (meshSettings != null)
        {
            meshSettings.OnValuesUpdated -= OnValuesUpdated;
            meshSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (heightMapSettings != null)
        {
            heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
            heightMapSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (textureSettings != null)
        {
            textureSettings.OnValuesUpdated -= OnTextureValuesUpdated;
            textureSettings.OnValuesUpdated += OnTextureValuesUpdated;
        }
    }

    void OnTextureValuesUpdated()
    {
        textureSettings.ApplyToMaterial();
    }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            Generate();
        }
    }
}

public enum Edge
{
    Top, Left, Right, Bottom, None, TopLeft, TopRight, BottomLeft, BottomRight
}