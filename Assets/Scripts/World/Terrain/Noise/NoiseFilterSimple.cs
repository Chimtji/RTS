using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NoiseFilterSimple : INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings settings;
    Vector3 position;

    int size;

    public NoiseFilterSimple(int size, NoiseSettings settings, Vector2 position)
    {
        System.Random prng = new System.Random(settings.seed);

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

        for (int i = 0; i < settings.layers; i++)
        {
            float xPos = (point.x - (size / 2f) + position.x) * frequency;
            float zPos = (point.z - (size / 2f) + position.z) * frequency;
            Vector3 pos = new Vector3(xPos, 0, zPos);
            float v = noise.Evaluate(pos);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistance;
        }

        noiseValue = Mathf.Max(0, noiseValue);
        return noiseValue * settings.strength;

    }
}