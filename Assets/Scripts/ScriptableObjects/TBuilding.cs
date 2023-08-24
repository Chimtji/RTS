using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TBuilding", menuName = "Structures/TBuilding")]
public class TBuilding : ScriptableObject
{

    [Header("Attributes")]
    public string name;
    public string description;
    public int health;
    public int cost;

    [Header("Structure")]
    public GameObject visual;
    public int width;
    public int height;

    [Header("Construction")]
    public GameObject visualConstruction;

    [Header("Blueprint")]
    public GameObject visualBlueprint;
    public GameObject visualBlueprintError;
}
