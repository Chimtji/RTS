using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameSettings : UpdatableData
{

    public PlayerSettings[] players;

    [System.Serializable]
    public class PlayerSettings
    {
        public string name;
        public Color color;
        public int team;
    }
}