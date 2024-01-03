using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType
    {
        Simplex, SimplexRigid, Circle
    }
    public FilterType filterType;
    [Range(0, 1)]
    public float persistance = 0.6f;
    public float strength = 1;
    public float roughness = 2;
    public float baseRoughness = 1;
    [Range(1, 8)]
    public int layers = 1;
    public float threshold = 0.5f;
    public float depth = 1f;

    public int seed;
    public Vector2 offset;

    public float radius = 20f;
    [Range(2, 8)]
    public int amount = 2;
    [Range(0f, 360f)]
    public float rotation = 0f;

    public AnimationCurve heightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public float distance = 0f;

    // public void validateValues()
    // {
    //     scale = Mathf.Max(scale, 0.01f);
    //     octaves = Mathf.Max(octaves, 1);
    //     lacunarity = Mathf.Max(lacunarity, 1);
    //     persistance = Mathf.Clamp01(persistance);
    // }
}
public enum CircleSide
{
    Top,
    Bottom,
    Left,
    Right,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}