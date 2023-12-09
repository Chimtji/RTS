using UnityEngine;
using UnityEngine.UI;

public class NoiseStartLocation
{
    public float[,] values;
    private int size;
    private NoiseStartLocationSettings settings;
    public RawImage noiseDebug;
    private Edge edge;

    public NoiseStartLocation(int chunkSize, NoiseStartLocationSettings settings, bool hasPlayerSpawn, Edge edge)
    {
        this.edge = edge;
        this.size = chunkSize;
        this.settings = settings;
        float[,] noiseMap = new float[size, size];

        if (hasPlayerSpawn)
        {
            Vector2 center = CalcCircleCenter();
            noiseMap = CalcNoiseMap(noiseMap, center);
        }

        values = noiseMap;
    }

    private float[,] CalcNoiseMap(float[,] noiseMap, Vector2 center)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distFromCenter = Vector2.Distance(center, new Vector2(x, y));
                float normalizedDistance = distFromCenter / settings.radius;

                // Ensure that normalized distance doesn't exceed 1
                normalizedDistance = Mathf.Clamp01(normalizedDistance);

                float noiseValue = settings.strength * (1f - normalizedDistance);

                noiseMap[x, y] = settings.heightCurve.Evaluate(noiseValue);
            }
        }

        return noiseMap;
    }

    private Vector2 CalcCircleCenter()
    {
        Vector2 center = Vector2.zero;
        switch (edge)
        {
            case Edge.Bottom:
                center = new Vector2(size / 2, size - settings.radius - settings.distance);
                break;
            case Edge.Top:
                center = new Vector2(size / 2, settings.radius + settings.distance);
                break;
            case Edge.Left:
                center = new Vector2(settings.radius + settings.distance, size / 2);
                break;
            case Edge.Right:
                center = new Vector2(size - settings.radius - settings.distance, size / 2);
                break;
            case Edge.TopLeft:
                center = new Vector2(settings.radius + settings.distance, settings.radius + settings.distance);
                break;
            case Edge.TopRight:
                center = new Vector2(size - settings.radius - settings.distance, settings.radius + settings.distance);
                break;
            case Edge.BottomLeft:
                center = new Vector2(settings.radius + settings.distance, size - settings.radius - settings.distance);
                break;
            case Edge.BottomRight:
                center = new Vector2(size - settings.radius - settings.distance, size - settings.radius - settings.distance);
                break;
            default:
                Debug.LogError("Invalid circle position specified.");
                break;
        }

        return center;
    }
}