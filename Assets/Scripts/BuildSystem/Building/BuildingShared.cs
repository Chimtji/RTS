using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingShared : MonoBehaviour
{
    public BuildManager builder;
    public Player owner;
    public BuildingData attributes;
    public InputManager input;
    public TerrainManager terrain;
    public Material blueprintGridMaterial;
    public Material blueprintMaterial;

    public GameObject uiActionsContainer;
    public GameObject uiInformationContainer;
    public Vector3 buildPosition;
}
