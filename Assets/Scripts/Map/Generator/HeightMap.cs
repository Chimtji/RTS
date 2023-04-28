using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMap
{
    /// <summary>
    /// Array of height value at coordinates
    /// </summary>
    public float[,] values;

    /// <summary>
    /// The min height value
    /// </summary>
    public float minValue;

    /// <summary>
    /// The Max height value
    /// </summary>
    public float maxValue;


    /// <summary>
    /// Creates a heightmap based on a noise map
    /// </summary>
    /// <param name="size">The size of the heightmap</param>
    /// <param name="settings">the heightmap settings used for generation</param>
    /// <param name="position">the world position of the heightmap</param>
    public HeightMap(int size, HeightMapSettings settings, Vector2 position)
    {
        NoiseMap noiseMap = new NoiseMap(size, settings.noiseSettings, position);
        FalloffMap falloffMap = new FalloffMap(size);
        AnimationCurve heightCurve_threadsafe = new AnimationCurve(settings.heightCurve.keys);

        minValue = float.MaxValue;
        maxValue = float.MinValue;

        for (int w = 0; w < size; w++)
        {
            for (int h = 0; h < size; h++)
            {
                if (settings.useFalloff)
                {
                    noiseMap.values[w, h] = noiseMap.values[w, h] - falloffMap.values[w, h];
                }

                noiseMap.values[w, h] *= heightCurve_threadsafe.Evaluate(noiseMap.values[w, h]) * settings.heightMultiplier;

                // Remove Mathf.Floor if terrain should not be stepped.
                noiseMap.values[w, h] = Mathf.Floor(noiseMap.values[w, h]) / 2;

                if (noiseMap.values[w, h] > maxValue)
                {
                    maxValue = noiseMap.values[w, h];
                }
                if (noiseMap.values[w, h] > minValue)
                {
                    minValue = noiseMap.values[w, h];
                }
            }
        }

        values = noiseMap.values;
    }
}

