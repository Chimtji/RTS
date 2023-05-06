using UnityEngine;

[CreateAssetMenu()]
public class TerrainObject : UpdatableData
{
    public GameObject prefab;
    public string containerName;

    public float maxSpawnHeight;

    public float minSpawnHeight;

    public NoiseSettings noiseSettings;

}