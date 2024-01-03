using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu()]
public class TerrainSettings : UpdatableData
{
    [Header("General")]
    /// <summary>
    /// The scale of the map
    /// </summary>
    public int scale = 1;

    /// <summary>
    /// The total size of the chunk. Can be any of 49, 73, 97, 121, 145, 169, 193, 217, 241 because of limit of mesh vertices.
    /// </summary>
    public int meshSize = 97;

    /// <summary>
    /// The size of the spawnable part of the chunk
    /// </summary>
    public int gridSize
    {
        get { return (meshSize - 3) * scale; }
    }

    /// <summary>
    /// The material set on the mesh
    /// </summary>
    public Material material;

    [Header("Height Settings")]
    public HeightMapSettings heightMapSettings;

    [Header("Water")]
    public GameObject waterPrefab;
    public LayerMask layerMask;

    [Header("Area Heights")]
    public float waterMinHeight;
    public float waterMaxHeight;
    public float groundMinHeight;
    public float groundMaxHeight;
    public float mountainMinHeight;
    public float mountainMaxHeight;

    [Header("Trees")]
    public GameObject treePrefab;
    [Range(0, 10)]
    public float treeMaxHeight;
    [Range(0, 10)]
    public float treeMinHeight;
    public float treeNoiseMax;
    public float treeNoiseMin;
    public HeightMapSettings treesHeightMapSettings;
}