using UnityEngine;
using System.Collections;

public class NoiseMap
{
    /// <summary>
    /// Array of noise value at coordinates
    /// </summary>
    public float[,] values;

    /// <summary>
    /// Creates a perlin noise map
    /// </summary>
    /// <param name="size">The size of the noisemap</param>
    /// <param name="settings">the noise settings for noise generation</param>
    /// <param name="position">the world position of the map</param>
    public NoiseMap(int size, NoiseSettings settings, Vector2 position)
    {
        float[,] noiseMap = new float[size, size];

        System.Random prng = new System.Random(settings.seed);
        Vector2[] octaveOffsets = new Vector2[settings.octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < settings.octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + settings.offset.x + position.x;
            float offsetY = prng.Next(-100000, 100000) - settings.offset.y - position.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= settings.persistance;
        }

        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        float halfSize = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {

                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < settings.octaves; i++)
                {
                    float sampleX = (x - halfSize + octaveOffsets[i].x) / settings.scale * frequency;
                    float sampleY = (y - halfSize + octaveOffsets[i].y) / settings.scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= settings.persistance;
                    frequency *= settings.lacunarity;
                }

                if (noiseHeight > maxLocalNoiseHeight)
                {
                    maxLocalNoiseHeight = noiseHeight;
                }
                if (noiseHeight < minLocalNoiseHeight)
                {
                    minLocalNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;

                // if (settings.normalizeMode == NormalizeMode.Global)
                // {
                float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
                noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);

                // }
            }
        }

        // if (settings.normalizeMode == NormalizeMode.Local)
        // {
        //     for (int y = 0; y < size; y++)
        //     {
        //         for (int x = 0; x < size; x++)
        //         {
        //             noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
        //         }
        //     }
        // }

        values = noiseMap;
    }

}