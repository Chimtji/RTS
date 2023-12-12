using System.Collections.Generic;
using System.Numerics;

namespace Trout.Types
{
    public struct ChunkPosition
    {
        public float x;
        public float y;
        public Vector2 vector;

        public ChunkPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.vector = new Vector2(x, y);
        }
    }

    public class ChunkMap : Dictionary<ChunkPosition, TerrainChunk>
    {
    }
};