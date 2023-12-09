using UnityEngine;

[System.Serializable]
public class NoiseStartLocationSettings : ScriptableObject
{
    public float radius = 20f;
    [Range(2, 8)]
    public int amount = 2;
    [Range(0f, 360f)]
    public float rotation = 0f;
    public float strength = 1f;

    public AnimationCurve heightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public float distance = 0f;
    public CircleSide side;
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