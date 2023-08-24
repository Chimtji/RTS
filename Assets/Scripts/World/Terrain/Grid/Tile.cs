using UnityEngine;
using Trout.Utils;
using System.Linq;
using System.Collections.Generic;

public class Tile
{
    /// <summary>
    /// The size of the cell
    /// </summary>
    public float size;

    /// <summary>
    /// The Local position of this tile in the local grid
    /// </summary>
    public Vector2 coordinate;

    /// <summary>
    /// A List of the positions of the 4 corners of the tile. Positions are in world Position.
    /// </summary>
    public List<Vector3> corners = new List<Vector3>();

    /// <summary>
    /// The world position of the top left corner of the tile
    /// </summary>
    public Vector3 topLeftCorner
    {
        get { return corners[1]; }
    }

    /// <summary>
    /// The world position of the top right corner of the tile
    /// </summary>
    public Vector3 topRightCorner
    {
        get { return corners[0]; }
    }

    /// <summary>
    /// The world position of the bottom left corner of the tile
    /// </summary>
    public Vector3 bottomLeftCorner
    {
        get { return corners[3]; }
    }

    /// <summary>
    /// The world position of the bottom right corner of the tile
    /// </summary>
    public Vector3 bottomRightCorner
    {
        get { return corners[2]; }
    }

    /// <summary>
    /// The position of the center of the cell with the y being height of the middle of the cell. Position given in world position.
    /// </summary>
    public Vector3 position
    {
        get
        {
            float[] cornersX = { corners[0].x, corners[1].x, corners[2].x, corners[3].x };
            float[] cornersY = { corners[0].y, corners[1].y, corners[2].y, corners[3].y };
            float[] cornersZ = { corners[0].z, corners[1].z, corners[2].z, corners[3].z };

            float xAverage = Utils.GetAverage(cornersX);
            float yAverage = Utils.GetAverage(cornersY);
            float zAverage = Utils.GetAverage(cornersZ);

            return new Vector3(xAverage, yAverage, zAverage);
        }
    }

    public Tile(float size, List<Vector3> corners)
    {
        this.corners = corners;
        this.size = size;
    }
}
