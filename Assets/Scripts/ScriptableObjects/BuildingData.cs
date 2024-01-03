using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Building", menuName = "Structures/Building")]
public class BuildingData : ScriptableObject
{

    [Header("Attributes")]
    public string name;
    public string description;

    [Header("Structure")]
    public GameObject visual;
    public int width;
    public int height;

    [Header("Stats")]
    public int health;

    [Header("Economy")]
    public int cost;

    [Header("UI")]
    public Sprite image;

    public string tooltip;

    [Header("Actions")]
    public BuildingData[] buildActions;
}
