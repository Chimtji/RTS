using UnityEngine;

[CreateAssetMenu()]
public class Vegetation : UpdatableData
{
    public GameObject prefab;
    public string containerName;

    public float maxSpawnHeight;

    public float minSpawnHeight;

    public NoiseSettings noiseSettings;

}