using UnityEngine;
using System.Collections;
using Trout.Utils;

public class TerrainChunkMesh
{
    public Mesh mesh;
    public TerrainChunkMesh(TerrainChunk chunk)
    {
        TerrainSettings settings = chunk.settings;
        int meshSize = settings.meshSize;

        float topLeftX = (meshSize - 1) / -2f;
        float topLeftZ = (meshSize - 1) / 2f;

        int verticesPerLine = meshSize / 1 + 1;

        FlatShadeMesh flatShadeMesh = new FlatShadeMesh(verticesPerLine);

        int[,] vertexIndicesMap = new int[meshSize, meshSize];
        int meshVertexIndex = 0;
        int borderVertexIndex = -1;

        for (int z = 0; z < meshSize; z++)
        {
            for (int x = 0; x < meshSize; x++)
            {
                bool isBorderVertex = z == 0 || z == meshSize - 1 || x == 0 || x == meshSize - 1;

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

        for (int z = 0; z < meshSize; z++)
        {
            for (int x = 0; x < meshSize; x++)
            {
                int vertexIndex = vertexIndicesMap[x, z];
                Vector2 percent = new Vector2(x / (float)meshSize, z / (float)meshSize);
                float height = chunk.heightMap.values[x, z];

                float coordX = (topLeftX + percent.x * meshSize) * settings.scale;
                float coordY = height;
                float coordZ = (topLeftZ - percent.y * meshSize) * settings.scale;
                Vector3 vertexPosition = new Vector3(coordX, coordY, coordZ);

                // if (chunk.edge != Edge.None)
                // {
                //     vertexPosition = GetEdgePosition(chunk.edge, x, z, meshSize, vertexPosition, chunk.edgeDepth);
                // }

                flatShadeMesh.AddVertex(vertexPosition, percent, vertexIndex);

                if (x < meshSize - 1 && z < meshSize - 1)
                {
                    int a = vertexIndicesMap[x, z];
                    int b = vertexIndicesMap[x + 1, z];
                    int c = vertexIndicesMap[x, z + 1];
                    int d = vertexIndicesMap[x + 1, z + 1];
                    flatShadeMesh.AddTriangle(a, d, c);
                    flatShadeMesh.AddTriangle(d, a, b);
                }
            }
        }

        flatShadeMesh.ProcessMesh();
        mesh = flatShadeMesh.CreateMesh();
    }

    static Vector3 GetEdgePosition(Edge edge, int x, int z, int mapSize, Vector3 position, float edgeHeight)
    {

        Vector3 modifiedPosition = new Vector3(position.x, position.y, position.z);
        if ((edge == Edge.Top || edge == Edge.TopLeft || edge == Edge.TopRight || edge == Edge.All) && IsLeftOrTopEdge(z, mapSize))
        {
            modifiedPosition.y = -edgeHeight;
            modifiedPosition.z = modifiedPosition.z - 1f;
        }
        if ((edge == Edge.Bottom || edge == Edge.BottomRight || edge == Edge.BottomLeft || edge == Edge.All) && IsRightOrBottomEdge(z, mapSize))
        {
            modifiedPosition.y = -edgeHeight;
            modifiedPosition.z = modifiedPosition.z + 1f;
        }
        if ((edge == Edge.Left || edge == Edge.TopLeft || edge == Edge.BottomLeft || edge == Edge.All) && IsLeftOrTopEdge(x, mapSize))
        {
            modifiedPosition.y = -edgeHeight;
            modifiedPosition.x = modifiedPosition.x + 1f;
        }
        if ((edge == Edge.Right || edge == Edge.TopRight || edge == Edge.BottomRight || edge == Edge.All) && IsRightOrBottomEdge(x, mapSize))
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