using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid
{
    public Vector3 position;
    public int size;
    public int scale = 1;
    public float cellSize;
    public Dictionary<Vector2, Tile> tiles;

    private List<Tile> tilesList = new List<Tile>();

    public Grid(Vector3 position, int size, float cellSize, Mesh mesh)
    {
        this.position = position;
        this.size = size;
        this.cellSize = cellSize;
        CreateGrid(mesh);
        // debugGrid();
    }

    // public Tile GetTile(Vector3 position)
    // {
    //     return tilesList.Find(tile => Vector3.Distance(tile.position, position) <= 1f);
    // }

    private void CreateGrid(Mesh mesh)
    {
        List<Dictionary<Vector3, int>> tilesData = groupVerticesByTiles(mesh);
        tilesList = new List<Tile>();

        for (int i = 0; i < tilesData.Count; i++)
        {
            Dictionary<Vector3, int> tileData = tilesData[i];
            List<Vector3> corners = new List<Vector3>();

            foreach (KeyValuePair<Vector3, int> coordinate in tileData)
            {
                corners.Add(coordinate.Key);
            }

            Tile tile = new Tile(cellSize, corners);
            tilesList.Add(tile);
        }

        int rowSize = (int)Mathf.Sqrt(tilesData.Count);
        tiles = new Dictionary<Vector2, Tile>();

        int index = 0;
        for (int x = 0; x < rowSize; x++)
        {
            for (int z = 0; z < rowSize; z++)
            {
                Tile tile = tilesList[index];
                tile.coordinate = new Vector2(x, z);
                Vector2 position = new Vector2(tile.position.x, tile.position.z);
                tiles.Add(position, tile);
                index++;
            }
        }
    }

    private List<Dictionary<Vector3, int>> groupVerticesByTiles(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        List<Dictionary<Vector3, int>> groupedVertices = new List<Dictionary<Vector3, int>>();
        Vector2 worldOffset = new Vector2(position.x, position.z);

        for (int i = 0; i < vertices.Length; i += 6)
        {
            if (i > 0)
            {
                Dictionary<Vector3, int> group = new Dictionary<Vector3, int>();

                for (int ix = i; ix >= i - 6; ix--)
                {
                    Vector3 worldPosition = new Vector3(vertices[ix].x + worldOffset.x, vertices[ix].y, vertices[ix].z + worldOffset.y);
                    if (!group.ContainsKey(worldPosition))
                    {
                        group.Add(worldPosition, ix);
                    }
                }

                // We remove all groups with only one coordinate because thats an invalid tile.
                // Why these exist i don't know.
                if (group.Count > 1)
                {
                    // We remove all the first coordinates in groups with more points than 4 becuase that's an invalid tile.
                    // I guess these exist because looping per 6 gives invalid groupings when reaching the tile at the end of each row.
                    // Easy fix is to remove the first coordinate from group because that's always the odd one out.
                    if (group.Count > 4)
                    {
                        group.Remove(group.First().Key);
                    }
                    groupedVertices.Add(group);
                }

            }
        }

        return groupedVertices;
    }

    private void debugGrid()
    {
        GameObject gridDebug = new GameObject("Grid");
        gridDebug.transform.parent = GameObject.Find("DebugContainer").transform;

        foreach (KeyValuePair<Vector2, Tile> tile in tiles)
        {
            GameObject tileObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tileObj.transform.parent = gridDebug.transform;
            tileObj.transform.position = tile.Value.position;
            tileObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
    }
}
