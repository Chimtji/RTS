using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TBuilding", menuName = "Structures/TBuilding")]
public class TBuilding : ScriptableObject
{

    [Header("Attributes")]
    public string name;
    public string description;

    [Header("Structure")]
    public GameObject visual;
    public int width;
    public int height;

    [Header("Construction")]
    public GameObject visualConstruction;

    [Header("Blueprint")]
    public GameObject visualBlueprint;
    public GameObject visualBlueprintError;

    [Header("Stats")]
    public int health;

    [Header("Economy")]
    public int cost;

    [Header("UI")]
    public GameObject buttons;
}
