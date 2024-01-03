using UnityEngine;

public class HeightMap
{
    /// <summary>
    /// The size of the heightmap
    /// </summary>
    public int size;

    /// <summary>
    /// Array of height value at coordinates
    /// </summary>
    public float[,] values;

    /// <summary>
    /// The World position of this height map
    /// </summary>
    public Vector2 position;

    /// <summary>
    /// Information about the start location on this map
    /// </summary>
    public StartLocation startLocation;

    /// <summary>
    /// Which side of the heightmap is an outside edge of the overall terrain chunk map. 
    /// </summary>
    private ChunkPositionName edge;

    /// <summary>
    /// Creates a heightmap based on a noise map
    /// </summary>
    /// <param name="size">The size of the heightmap</param>
    /// <param name="settings">the heightmap settings used for generation</param>
    /// <param name="position">the world position of the heightmap</param>
    public HeightMap(int size, HeightMapSettings settings, Vector2 position, StartLocation startLocation, ChunkPositionName edge)
    {

        this.size = size;
        this.position = position;
        this.edge = edge;
        this.startLocation = startLocation;

        CircleNoiseFilter startLocationMap = (CircleNoiseFilter)NoiseFilterFactory.CreateNoiseFilter(size, settings.startLocation[0].settings, position, edge);
        INoiseFilter[] ground = InitNoiseFilters(settings.ground, new INoiseFilter[settings.ground.Length]);
        INoiseFilter[] waters = InitNoiseFilters(settings.waters, new INoiseFilter[settings.waters.Length]);
        INoiseFilter[] mountains = InitNoiseFilters(settings.mountains, new INoiseFilter[settings.mountains.Length]);

        float waterLevelOffset = settings.waters[0].settings.depth;
        float mountainLevelOffset = settings.mountains[0].settings.depth;
        float groundLevelHeight = settings.ground[0].settings.depth;
        float watersMaskThreshold = settings.waters[0].settings.threshold;
        float mountainMaskThreshold = settings.mountains[0].settings.threshold;

        float[,] noiseMap = new float[size, size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector3 point = new Vector3(x, 0, y);

                float groundBaseElevation = CalcBaseElevation(ground, settings.ground, point);
                float watersBaseElevation = CalcBaseElevation(waters, settings.waters, point) - (this.startLocation.Enabled ? startLocationMap.Evaluate(point) : 0);
                float mountainsBaseElevation = CalcBaseElevation(mountains, settings.mountains, point) - (this.startLocation.Enabled ? startLocationMap.Evaluate(point) : 0);

                float groundNoise = CalcElevation(ground, settings.ground, point, groundBaseElevation) * groundLevelHeight;
                float waterNoise = CalcElevation(waters, settings.waters, point, watersBaseElevation) * waterLevelOffset * Mathf.Clamp01(watersBaseElevation - watersMaskThreshold);
                float mountainNoise = CalcElevation(mountains, settings.mountains, point, mountainsBaseElevation) * mountainLevelOffset * Mathf.Clamp01(mountainsBaseElevation - mountainMaskThreshold);

                float elevation = groundNoise + mountainNoise - waterNoise;

                if (x == Mathf.FloorToInt(startLocationMap.circleLocalCenter.x) && y == Mathf.FloorToInt(startLocationMap.circleLocalCenter.y))
                {
                    this.startLocation.SpawnPosition = new Vector3(startLocationMap.circleWorldCenter.x, elevation, startLocationMap.circleWorldCenter.y);
                }

                noiseMap[x, y] = elevation;
            }
        }
        values = noiseMap;
    }

    private INoiseFilter[] InitNoiseFilters(NoiseLayer[] noise, INoiseFilter[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i] = NoiseFilterFactory.CreateNoiseFilter(size, noise[i].settings, position, edge);
        }
        return list;
    }

    private float CalcElevation(INoiseFilter[] noise, NoiseLayer[] settings, Vector3 point, float baseElevation)
    {
        float elevation = baseElevation;
        for (int i = 1; i < noise.Length; i++)
        {
            if (settings[i].enabled)
            {
                elevation += noise[i].Evaluate(point) * baseElevation;
            }
        }

        return elevation;
    }

    private float CalcBaseElevation(INoiseFilter[] noise, NoiseLayer[] settings, Vector3 point)
    {
        float elevation = 0;


        if (noise.Length > 0 && settings[0].enabled)
        {
            elevation = noise[0].Evaluate(point);
        }

        return elevation;
    }
}

