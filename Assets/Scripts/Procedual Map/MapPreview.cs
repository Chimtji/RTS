using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapPreview : MonoBehaviour
{
    public enum DrawMode { NoiseMap, Mesh, FalloffMap };
    public DrawMode drawMode;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureData;
    public MapSettings mapSettings;

    public Material terrainMaterial;


    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int editorPreviewLOD;

    public bool autoUpdate;

    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;


    public HeightMapSettings treesSettings;
    public GameObject TreePrefab;
    public GameObject TreesContainer;
    [Range(0, 10)]
    public float treeMaxHeight;
    [Range(0, 10)]
    public float treeMinHeight;
    public float treeNoiseMax;
    public float treeNoiseMin;


    public void DrawMapInEditor()
    {
        textureData.ApplyToMaterial(terrainMaterial);
        textureData.UpdateMeshHeights(terrainMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);
        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero);

        if (drawMode == DrawMode.NoiseMap)
        {
            DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            MeshData mesh = MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, editorPreviewLOD);
            DrawMesh(mesh);
            PlaceTrees(mesh);
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            DrawTexture(TextureGenerator.TextureFromHeightMap(new HeightMap(FalloffGenerator.GenerateFalloffMap(meshSettings.numVertsPerLine), 0, 1)));
        }
    }

    public void DrawTexture(Texture2D texture)
    {
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10f;

        textureRender.gameObject.SetActive(true);
        meshFilter.gameObject.SetActive(false);
    }

    public void DrawMesh(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();

        textureRender.gameObject.SetActive(false);
        meshFilter.gameObject.SetActive(true);
    }

    void OnTextureValuesUpdated()
    {
        textureData.ApplyToMaterial(terrainMaterial);
    }

    void OnValidate()
    {

        if (meshSettings != null)
        {
            meshSettings.OnValuesUpdated -= OnValuesUpdated;
            meshSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (heightMapSettings != null)
        {
            heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
            heightMapSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnTextureValuesUpdated;
            textureData.OnValuesUpdated += OnTextureValuesUpdated;
        }
        if (treesSettings != null)
        {
            treesSettings.OnValuesUpdated -= OnValuesUpdated;
            treesSettings.OnValuesUpdated += OnValuesUpdated;
        }

    }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    void PlaceTrees(MeshData mesh)
    {

        // Destroy all trees first
        int childs = TreesContainer.transform.childCount;
        for (int i = childs - 1; i > 0; i--)
        {
            GameObject.DestroyImmediate(TreesContainer.transform.GetChild(i).gameObject);
        }

        Dictionary<Vector3, GameObject> Trees = new Dictionary<Vector3, GameObject>();

        HeightMap treesMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, treesSettings, Vector2.zero);
        int size = mesh.vertices.GetLength(0);
        Vector3 position;

        int offset = treesMap.values.GetLength(0) / 2;
        for (int i = 0; i < size; i++)
        {
            if (!Trees.ContainsKey(mesh.vertices[i]))
            {
                if (mesh.vertices[i].y > treeMinHeight && mesh.vertices[i].y < treeMaxHeight)
                {
                    if (treesMap.values[Mathf.FloorToInt(mesh.vertices[i].x + offset), Mathf.FloorToInt(mesh.vertices[i].z + offset)] > treeNoiseMin && treesMap.values[Mathf.FloorToInt(mesh.vertices[i].x + offset), Mathf.FloorToInt(mesh.vertices[i].z + offset)] < treeNoiseMax)
                    {
                        position = new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z);
                        Trees.Add(position, TreePrefab);
                    }
                }
            }
        }

        foreach (KeyValuePair<Vector3, GameObject> tree in Trees)
        {
            GameObject instantiatedTree = Instantiate(TreePrefab, tree.Key, Quaternion.identity);
            instantiatedTree.transform.SetParent(TreesContainer.transform);
        }
    }
}