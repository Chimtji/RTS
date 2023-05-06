using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu()]
public class TerrainSettings : UpdatableData
{
    public string containerName;
    public Material material;
    public HeightMapSettings heightMapSettings;
}