using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu()]
public class TerrainSettings : UpdatableData
{
    public string containerName;
    public Material material;
    public HeightMapSettings heightMapSettings;
    public GameObject waterPrefab;

    [Header("Area Heights")]
    public float waterMinHeight;
    public float waterMaxHeight;
    public float groundMinHeight;
    public float groundMaxHeight;
    public float mountainMinHeight;
    public float mountainMaxHeight;

}