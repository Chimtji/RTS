using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeightMap
{
    /// <summary>
    /// Array of height value at coordinates
    /// </summary>
    public float[,] values;

    /// <summary>
    /// Creates a heightmap based on a noise map
    /// </summary>
    /// <param name="size">The size of the heightmap</param>
    /// <param name="settings">the heightmap settings used for generation</param>
    /// <param name="position">the world position of the heightmap</param>
    public HeightMap(int size, HeightMapSettings settings, Vector2 pos, NoiseSpawnPositions playerSpawnMap)
    {
        // INoiseFilter ground = NoiseFilterFactory.CreateNoiseFilter(size, settings.ground.settings, pos);
        INoiseFilter[] ground = InitNoiseFilters(size, settings.ground, new INoiseFilter[settings.ground.Length], pos);
        INoiseFilter[] waters = InitNoiseFilters(size, settings.waters, new INoiseFilter[settings.waters.Length], pos);
        INoiseFilter[] mountains = InitNoiseFilters(size, settings.mountains, new INoiseFilter[settings.mountains.Length], pos);
        // INoiseFilter water = NoiseFilterFactory.CreateNoiseFilter(size, settings.waters[0].settings, pos);
        // INoiseFilter mountain = NoiseFilterFactory.CreateNoiseFilter(size, settings.mountains[0].settings, pos);

        float waterLevelOffset = settings.waters[0].settings.oceanFloorDepth;
        float mountainLevelOffset = settings.mountains[0].settings.oceanFloorDepth;
        float groundLevelHeight = settings.ground[0].settings.oceanFloorDepth;
        float watersMaskThreshold = settings.waters[0].settings.oceanThreshold;
        float mountainMaskThreshold = settings.mountains[0].settings.oceanThreshold;

        float[,] noiseMap = new float[size, size];

        Texture2D debugNoiseTexture = new Texture2D(size, size);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector3 position = new Vector3(x, 0, y);
                float groundBaseElevation = CalcBaseElevation(ground, settings.ground, position);
                float watersBaseElevation = CalcBaseElevation(waters, settings.waters, position) - playerSpawnMap.values[x, y];
                float mountainsBaseElevation = CalcBaseElevation(mountains, settings.mountains, position) - playerSpawnMap.values[x, y];

                float groundNoise = CalcElevation(ground, settings.ground, position, groundBaseElevation) * groundLevelHeight;
                float waterNoise = CalcElevation(waters, settings.waters, position, watersBaseElevation) * waterLevelOffset * Mathf.Clamp01(watersBaseElevation - watersMaskThreshold);
                float mountainNoise = CalcElevation(mountains, settings.mountains, position, mountainsBaseElevation) * mountainLevelOffset * Mathf.Clamp01(mountainsBaseElevation - mountainMaskThreshold);

                float elevation = groundNoise + mountainNoise - waterNoise;


                if (x <= 0)
                {
                    elevation = 0;
                }

                noiseMap[x, y] = elevation;

                // Debug
                Color color = new Color(elevation, elevation, elevation);
                debugNoiseTexture.SetPixel(x, y, color);
            }
        }
        values = noiseMap;
        debugNoiseTexture.Apply();
        DrawDebugNoiseImage(debugNoiseTexture);
    }

    void DrawDebugNoiseImage(Texture2D debugNoiseTexture)
    {
        RawImage debugNoiseImage = GameObject.Find("DebugNoiseImage").GetComponent<RawImage>();
        if (debugNoiseImage != null)
        {
            debugNoiseImage.texture = debugNoiseTexture;
        }
        else
        {
            Debug.LogError("RawImage reference is not set!");
        }
    }

    private INoiseFilter[] InitNoiseFilters(int size, NoiseLayer[] noise, INoiseFilter[] list, Vector2 position)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i] = NoiseFilterFactory.CreateNoiseFilter(size, noise[i].settings, position);
        }
        return list;
    }

    private float CalcElevation(INoiseFilter[] noise, NoiseLayer[] settings, Vector3 position, float baseElevation)
    {
        float elevation = baseElevation;
        for (int i = 1; i < noise.Length; i++)
        {
            if (settings[i].enabled)
            {
                // Debug.Log(noise[i].Evaluate(position) * baseElevation);
                elevation += noise[i].Evaluate(position) * baseElevation;
            }
        }

        return elevation;
    }

    private float CalcBaseElevation(INoiseFilter[] noise, NoiseLayer[] settings, Vector3 position)
    {
        float elevation = 0;


        if (noise.Length > 0 && settings[0].enabled)
        {
            elevation = noise[0].Evaluate(position);
        }

        return elevation;
    }
}

