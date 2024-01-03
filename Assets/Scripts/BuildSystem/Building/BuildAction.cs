using UnityEngine;

[CreateAssetMenu()]
public class BuildAction : ScriptableObject
{
    public enum BuildType
    {
        Unit, Technology, Structure
    }

    public string name;

    // This should handle all 3 types and not just TBuilding
    public BuildingData product;

    public Sprite image;

    public string description;
}