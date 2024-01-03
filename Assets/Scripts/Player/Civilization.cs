using UnityEngine;

[CreateAssetMenu()]
public class Civilization : ScriptableObject
{
    [Header("General")]
    public string name;

    [Header("Buildings")]
    public BuildingData headquarter;

};