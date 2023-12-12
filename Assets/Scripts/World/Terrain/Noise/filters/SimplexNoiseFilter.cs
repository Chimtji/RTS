using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SimplexNoiseFilter : INoiseFilter
{
    SimplexNoise noise = new SimplexNoise();
    NoiseSettings settings;
    int size;
    Vector3 position;

    SimplexType type;

    public SimplexNoiseFilter(int size, NoiseSettings settings, Vector2 position, SimplexType type)
    {
        System.Random prng = new System.Random(settings.seed);

        this.type = type;
        this.size = size;
        this.settings = settings;
        this.position = new Vector3(
            prng.Next(-100000, 100000) + settings.offset.x + position.x,
            0,
            prng.Next(-100000, 100000) - settings.offset.y - position.y
        );
    }
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness / 100;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.layers; i++)
        {
            float xPos = (point.x - (size / 2f) + position.x) * frequency;
            float zPos = (point.z - (size / 2f) + position.z) * frequency;
            Vector3 pos = new Vector3(xPos, 0, zPos);

            float v = noise.Evaluate(pos);

            if (type == SimplexType.Rigid)
            {
                v = 1 - Mathf.Abs(noise.Evaluate(pos));
                v *= v;
                v *= weight;
                weight = v;
                noiseValue += v * amplitude;
            }
            else
            {
                noiseValue += (v + 1) * 0.5f * amplitude;
            }

            frequency *= settings.roughness;
            amplitude *= settings.persistance;
        }

        if (type == SimplexType.Rigid)
        {
            noiseValue = Mathf.Max(0, noiseValue + settings.threshold);

        }
        else
        {
            noiseValue = Mathf.Max(0, noiseValue);

        }


        return noiseValue * settings.strength;
    }
}

public enum SimplexType
{
    Normal, Rigid
}