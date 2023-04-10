using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap
{
    private Grid<TilemapObject> grid;
    public Tilemap(int width, int height, float cellSize, Vector3 originPosition)
    {

        grid = new Grid<TilemapObject>(
                   width,
                   height,
                   cellSize,
                   Vector3.zero,
                   (Grid<TilemapObject> g, int x, int z) => new TilemapObject(g, x, z)
               );
    }

    public void SetTilemapSprite(Vector3 worldPosition, TilemapObject.TilemapSprite tilemapSprite)
    {
        TilemapObject tilemapObject = grid.GetGridCell(worldPosition);
        if (tilemapObject != null)
        {
            tilemapObject.SetTilemapSprite(tilemapSprite);
        }
    }

    public void SetTilemapVisual(TilemapVisual tilemapVisual)
    {
        tilemapVisual.SetGrid(grid);
    }

    public class TilemapObject
    {
        public enum TilemapSprite
        {
            None,
            Ground
        }

        private Grid<TilemapObject> grid;
        private int x;
        private int z;
        private TilemapSprite tilemapSprite;

        public TilemapObject(Grid<TilemapObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void SetTilemapSprite(TilemapSprite tilemapSprite)
        {
            this.tilemapSprite = tilemapSprite;
            grid.TriggerGridCellChanged(x, z);
        }

        public override string ToString()
        {
            return tilemapSprite.ToString();
        }

        public TilemapSprite GetTilemapSprite()
        {
            return tilemapSprite;
        }
    }
}

