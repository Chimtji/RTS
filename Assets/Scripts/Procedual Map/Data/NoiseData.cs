using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseData : UpdatableData
{
    public Noise.NormalizeMode normalizeMode;
    public float noiseScale;

    [Range(0, 10)]
    public int octaves;

    [Range(0, 1)]
    public float persistance;

    [Range(1, 20)]
    public float lacunarity;

    [Range(0, 20)]
    public int seed;
    public Vector2 offset;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

        base.OnValidate();

    }

#endif
}