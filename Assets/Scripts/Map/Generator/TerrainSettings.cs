using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu()]
public class TerrainSettings : UpdatableData
{
    public Material material;
    public HeightMapSettings heightMapSettings;
    public TextureLayer[] textureLayers;
}

[System.Serializable]
public class TextureLayer
{
    public Texture2D texture;
    public Color tint;
    [Range(0, 1)]
    public float tintStrength = 0.7f;
    public float startHeight = 0;
    [Range(0, 1)]
    public float blendStrength = 0;
    public float textureScale = 1;
}