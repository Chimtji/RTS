using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class UnitData : ScriptableObject
{

    [Header("Attributes")]
    public string name;
    public string description;

    [Header("Visual")]
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
}
