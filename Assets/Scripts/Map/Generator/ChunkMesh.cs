using UnityEngine;
using System.Collections;

public class ChunkMesh
{
    public Mesh mesh;
    public ChunkMesh(Chunk chunk)
    {
        int borderedSize = chunk.heightMap.values.GetLength(0);
        int meshSize = borderedSize - 2;
        int meshSizeUnsimplified = borderedSize - 2;

        float topLeftX = (meshSizeUnsimplified - 1) / -2f;
        float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

        int verticesPerLine = (meshSize - 1) / 1 + 1;

        FlatShadeMesh flatShadeMesh = new FlatShadeMesh(verticesPerLine);

        int[,] vertexIndicesMap = new int[borderedSize, borderedSize];
        int meshVertexIndex = 0;
        int borderVertexIndex = -1;

        for (int z = 0; z < borderedSize; z++)
        {
            for (int x = 0; x < borderedSize; x++)
            {
                bool isBorderVertex = z == 0 || z == borderedSize - 1 || x == 0 || x == borderedSize - 1;

                if (isBorderVertex)
                {
                    vertexIndicesMap[x, z] = borderVertexIndex;
                    borderVertexIndex--;
                }
                else
                {
                    vertexIndicesMap[x, z] = meshVertexIndex;
                    meshVertexIndex++;
                }
            }
        }

        for (int z = 0; z < borderedSize; z++)
        {
            for (int x = 0; x < borderedSize; x++)
            {
                int vertexIndex = vertexIndicesMap[x, z];
                Vector2 percent = new Vector2((x - 1) / (float)meshSize, (z - 1) / (float)meshSize);
                float height = chunk.heightMap.values[x, z];

                float coordX = (topLeftX + percent.x * meshSizeUnsimplified) * chunk.scale;
                float coordY = height;
                float coordZ = (topLeftZ - percent.y * meshSizeUnsimplified) * chunk.scale;
                Vector3 vertexPosition = new Vector3(coordX, coordY, coordZ);

                if (chunk.edgeType != Edge.None)
                {
                    vertexPosition = GetEdgePosition(chunk.edgeType, x, z, borderedSize, vertexPosition, chunk.edgeDepth);
                }

                flatShadeMesh.AddVertex(vertexPosition, percent, vertexIndex);

                if (x < borderedSize - 1 && z < borderedSize - 1)
                {
                    int a = vertexIndicesMap[x, z];
                    int b = vertexIndicesMap[x + 1, z];
                    int c = vertexIndicesMap[x, z + 1];
                    int d = vertexIndicesMap[x + 1, z + 1];
                    flatShadeMesh.AddTriangle(a, d, c);
                    flatShadeMesh.AddTriangle(d, a, b);
                }

                vertexIndex++;
            }
        }

        flatShadeMesh.ProcessMesh();
        mesh = flatShadeMesh.CreateMesh();
    }

    static Vector3 GetEdgePosition(Edge edge, int x, int z, int mapSize, Vector3 position, float edgeHeight)
    {

        Vector3 modifiedPosition = new Vector3(position.x, position.y, position.z);
        if ((edge == Edge.Top || edge == Edge.TopLeft || edge == Edge.TopRight) && IsLeftOrTopEdge(z, mapSize))
        {
            modifiedPosition.y = -edgeHeight;
            modifiedPosition.z = modifiedPosition.z - 1f;
        }
        if ((edge == Edge.Bottom || edge == Edge.BottomRight || edge == Edge.BottomLeft) && IsRightOrBottomEdge(z, mapSize))
        {
            modifiedPosition.y = -edgeHeight;
            modifiedPosition.z = modifiedPosition.z + 1f;
        }
        if ((edge == Edge.Left || edge == Edge.TopLeft || edge == Edge.BottomLeft) && IsLeftOrTopEdge(x, mapSize))
        {
            modifiedPosition.y = -edgeHeight;
            modifiedPosition.x = modifiedPosition.x + 1f;
        }
        if ((edge == Edge.Right || edge == Edge.TopRight || edge == Edge.BottomRight) && IsRightOrBottomEdge(x, mapSize))
        {
            modifiedPosition.y = -edgeHeight;
            modifiedPosition.x = modifiedPosition.x - 1f;
        }
        return modifiedPosition;
    }

    static bool IsLeftOrTopEdge(int index, int mapSize)
    {
        if (index < 2)
        {
            return true;
        }

        return false;
    }
    static bool IsRightOrBottomEdge(int index, int mapSize)
    {
        if (index > mapSize - 3)
        {
            return true;
        }

        return false;
    }

}
