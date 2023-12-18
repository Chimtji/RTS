using UnityEngine;

[CreateAssetMenu()]
public class Player : ScriptableObject
{
    [Header("General")]
    public string id;

    [Header("Diplomacy")]
    public string nickname;
    public Color color;
    public int team;

    [Header("Civilization")]
    public Civilization civilization;
}