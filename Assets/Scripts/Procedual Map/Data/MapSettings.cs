using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MapSettings : UpdatableData
{
    // TODO: Make Enum in Tshirt sizes or based on player number
    [Range(1, 4)]
    public int size;

    public int GetTotalNumOfChunks()
    {
        return size * size;
    }
}
