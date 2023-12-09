using UnityEngine;

[System.Serializable]
public class PlayerAttributes
{
    [Header("General")]
    public string id;
    [Header("Diplomacy")]
    public string nickname;
    public Color color;
    public int team;
}