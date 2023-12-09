using UnityEngine;
using System.Collections;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType
    {
        Simple, Rigid
    }
    public enum ElevationType
    {
        Raise, Lower
    }
    public FilterType filterType;
    public ElevationType elevationType;

    [Range(0, 1)]
    public float persistance = 0.6f;
    public float strength = 1;
    public float roughness = 2;
    public float baseRoughness = 1;
    [Range(1, 8)]
    public int layers = 1;
    public float oceanThreshold = 0.5f;
    public float oceanFloorDepth = 1;

    public int seed;
    public Vector2 offset;

    // public void validateValues()
    // {
    //     scale = Mathf.Max(scale, 0.01f);
    //     octaves = Mathf.Max(octaves, 1);
    //     lacunarity = Mathf.Max(lacunarity, 1);
    //     persistance = Mathf.Clamp01(persistance);
    // }
}