using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trout.Utils;

// This class holds all the logic about each grid Item, but not what is placed in the GridCell.

public class GridCell
{
    private Grid<GridCell> grid;
    private int x;
    private int z;
    private Transform transform;

    public GridCell(Grid<GridCell> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }

    public void Set(Transform transform)
    {
        this.transform = transform;
        grid.Refresh(this, x, z);
    }

    public void Clear()
    {
        transform = null;
    }

    public bool isEmpty()
    {
        return transform == null;
    }

    // ------ Is this still relevant? --------
    // public override string ToString()
    // {
    //     return x + ", " + z + "\n" + transform;
    // }
}