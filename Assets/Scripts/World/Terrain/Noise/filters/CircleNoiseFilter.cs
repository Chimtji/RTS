using UnityEngine;

public class CircleNoiseFilter : INoiseFilter
{
    /// <summary>
    /// Array of noise value at coordinates
    /// </summary>
    public float[,] values;

    /// <summary>
    /// The center of the circle in a world position
    /// </summary>
    public Vector2 circleWorldCenter;

    /// <summary>
    /// The center of the circle in a local position
    /// </summary>
    public Vector2 circleLocalCenter;

    /// <summary>
    /// The size of the noise map
    /// </summary>
    private int size;

    /// <summary>
    /// The settings used to render the noise
    /// </summary>
    private NoiseSettings settings;

    /// <summary>
    /// The world position of the Noise map
    /// </summary>
    private Vector2 position;
    private ChunkPositionName edge;

    public CircleNoiseFilter(int chunkSize, NoiseSettings settings, Vector2 position, ChunkPositionName edge)
    {
        size = chunkSize;
        this.edge = edge;
        this.position = position;
        this.settings = settings;

        circleLocalCenter = CalcCircleLocalCenter();
        values = CreateNoise();
        circleWorldCenter = CalcCircleCenterWorld();
    }

    public float Evaluate(Vector3 point)
    {
        return values[(int)point.x, (int)point.z];
    }

    public Vector2 CalcCircleCenterWorld()
    {
        float xPos = position.x - size / 2f + circleLocalCenter.x;
        float zPos = position.y + size / 2f - circleLocalCenter.y;

        return new Vector2(xPos, zPos);
    }

    private float[,] CreateNoise()
    {
        float[,] noiseMap = new float[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float distFromCenter = Vector2.Distance(circleLocalCenter, new Vector2(x, y));
                float normalizedDistance = distFromCenter / settings.radius;

                normalizedDistance = Mathf.Clamp01(normalizedDistance);

                float noiseValue = settings.strength * (1f - normalizedDistance);

                noiseMap[x, y] = settings.heightCurve.Evaluate(noiseValue);
            }
        }

        return noiseMap;
    }

    private Vector2 CalcCircleLocalCenter()
    {
        Vector2 center = Vector2.zero;
        switch (edge)
        {
            case ChunkPositionName.Bottom:
                center = new Vector2(size / 2, size - settings.radius - settings.distance);
                break;
            case ChunkPositionName.Top:
                center = new Vector2(size / 2, settings.radius + settings.distance);
                break;
            case ChunkPositionName.Left:
                center = new Vector2(settings.radius + settings.distance, size / 2);
                break;
            case ChunkPositionName.Right:
                center = new Vector2(size - settings.radius - settings.distance, size / 2);
                break;
            case ChunkPositionName.TopLeft:
                center = new Vector2(settings.radius + settings.distance, settings.radius + settings.distance);
                break;
            case ChunkPositionName.TopRight:
                center = new Vector2(size - settings.radius - settings.distance, settings.radius + settings.distance);
                break;
            case ChunkPositionName.BottomLeft:
                center = new Vector2(settings.radius + settings.distance, size - settings.radius - settings.distance);
                break;
            case ChunkPositionName.BottomRight:
                center = new Vector2(size - settings.radius - settings.distance, size - settings.radius - settings.distance);
                break;
            default:
                Debug.LogError("Invalid circle position specified.");
                break;
        }

        return center;
    }
}