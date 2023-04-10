using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trout.Utils;

// This class holds all the logic about interacting with the Grid.

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private SGridItem gridItem;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    private Grid<GridCell> grid;
    private SGridItem.Dir dir = SGridItem.Dir.Down;
    private void Awake()
    {
        grid = new Grid<GridCell>(
            width,
            height,
            cellSize,
            Vector3.zero,
            (Grid<GridCell> g, int x, int z) => new GridCell(g, x, z)
        );
    }

    private void Build()
    {
        var (x, z) = grid.ToGridPosition(Mouse3D.GetMouseWorldPosition3D());
        List<Vector2Int> gridPositionList = gridItem.GetGridPositionList(new Vector2Int(x, z), dir);

        if (canBuild(gridPositionList))
        {
            Vector2Int rotationOffset = gridItem.GetRotationOffset(dir);
            Vector3 gridItemWorldPosition = grid.ToWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            Transform builtTransform = Instantiate(
                gridItem.prefab,
                gridItemWorldPosition,
                Quaternion.Euler(0, gridItem.GetRotationAngle(dir), 0)
            );

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridCell(gridPosition.x, gridPosition.y).Set(builtTransform);
            }
        }
        else
        {
            Debug.Log("Cannot build there!");
        }

    }

    private void Destroy()
    {

    }

    private bool canBuild(List<Vector2Int> gridPositionList)
    {
        bool positionIsFree = true;

        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (!grid.GetGridCell(gridPosition.x, gridPosition.y).isEmpty())
            {
                positionIsFree = false;
                break;
            }
        }

        return positionIsFree;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Build();

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = SGridItem.Rotate(dir);
            Debug.Log("direction: " + dir);
        }
    }

}
