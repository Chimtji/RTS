using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectGenerator
{

    public void Generate(TerrainObject terrainObject)
    {
        // clear container
        // get chunk map
        // get possible spawn positions in chunk map
        // match possible spawn positions with object noise map
        // spawn
    }


    // public Vector2[] GetSpawnPositions(HeightMapSettings heightMapSettings){
    //     Dictionary<Vector2, ChunkData> chunks = GetChunks(heightMapSettings);

    //     foreach( KeyValuePair<Vector2, ChunkData> chunk in chunks){
    //         chunk.Value.size
    //     }

    // }

    // public Dictionary<Vector2, ChunkData> GetChunks(HeightMapSettings heightMapSettings)
    // {
    //     ChunkMap chunkMap = new ChunkMap(3, heightMapSettings);
    //     return chunkMap.CreateChunkList();
    // }
}