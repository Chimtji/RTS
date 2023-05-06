using System;
using UnityEngine;
using CodeMonkey.Utils;


// This class holds all the logic about creating the grid and handling positions.

public class Grid<TGridCell>
{
    public event EventHandler<OnGridCellChangedEventArgs> OnGridCellChanged;
    public class OnGridCellChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridCell[,] gridCells;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridCell>, int, int, TGridCell> createGridCell)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridCells = new TGridCell[width, height];

        for (int x = 0; x < gridCells.GetLength(0); x++)
        {
            for (int z = 0; z < gridCells.GetLength(1); z++)
            {
                gridCells[x, z] = createGridCell(this, x, z);
            }
        }

        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridCells.GetLength(0); x++)
            {
                for (int z = 0; z < gridCells.GetLength(1); z++)
                {
                    debugTextArray[x, z] = UtilsClass.CreateWorldText(gridCells[x, z]?.ToString(), null, ToWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 15, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(ToWorldPosition(x, z), ToWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(ToWorldPosition(x, z), ToWorldPosition(x + 1, z), Color.white, 100f);
                }
            }
            Debug.DrawLine(ToWorldPosition(0, height), ToWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(ToWorldPosition(width, 0), ToWorldPosition(width, height), Color.white, 100f);

            OnGridCellChanged += (object sender, OnGridCellChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.z].text = gridCells[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }

    public void Refresh(TGridCell gridCell, int x, int z)
    {
        // Do some refresh logic in here to update all of grid
    }


    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector2Int ToGridPosition(Vector3 worldPosition)
    {

        int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        int z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
        Debug.Log((worldPosition - originPosition) / cellSize);
        return new Vector2Int(x, z);
    }

    public Vector3 ToWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }
    public Vector3 ToWorldPositionY(int x, int z)
    {
        return new Vector3(x, z, 0) * cellSize + originPosition;
    }

    public TGridCell GetGridCell(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridCells[x, z];
        }
        else
        {
            return default(TGridCell);
        }
    }

    public TGridCell GetGridCell(Vector3 worldPosition)
    {
        var (x, z) = ToGridPosition(worldPosition);
        return GetGridCell(x, z);
    }

    public void SetGridObject(int x, int z, TGridCell value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridCells[x, z] = value;
            TriggerGridCellChanged(x, z);
        }
    }

    public void TriggerGridCellChanged(int x, int z)
    {
        OnGridCellChanged?.Invoke(this, new OnGridCellChangedEventArgs { x = x, z = z });
    }

}
