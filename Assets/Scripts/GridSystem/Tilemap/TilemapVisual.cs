using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trout.Utils;

public class TilemapVisual : MonoBehaviour
{
    private Grid<Tilemap.TilemapObject> grid;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(Grid<Tilemap.TilemapObject> grid)
    {
        this.grid = grid;
        UpdateTilemapVisual();

        grid.OnGridCellChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<Tilemap.TilemapObject>.OnGridCellChangedEventArgs e)
    {
        UpdateTilemapVisual();
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateTilemapVisual();
        }
    }

    private void UpdateTilemapVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z = 0; z < grid.GetHeight(); z++)
            {
                int index = x * grid.GetHeight() + z;

                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                Tilemap.TilemapObject gridObject = grid.GetGridCell(x, z);
                Tilemap.TilemapObject.TilemapSprite tilemapSprite = gridObject.GetTilemapSprite();

                Vector2 gridValueUv;
                if (tilemapSprite == Tilemap.TilemapObject.TilemapSprite.None)
                {
                    gridValueUv = Vector2.zero;
                    quadSize = Vector3.zero;
                }
                else
                {
                    gridValueUv = Vector2.one;
                }

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.ToWorldPositionY(x, z) + quadSize * 0.5f, 0f, quadSize, Vector2.zero, Vector2.zero);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
