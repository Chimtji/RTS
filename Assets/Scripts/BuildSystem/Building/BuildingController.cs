using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour, IPointerClickHandler
{
    public GameObject building;
    public TBuilding attributes;
    public GameObject ui;

    public void TrainUnit(TBuilding unit)
    {
        Debug.Log("building:" + unit.name);
    }

    public void OnPointerClick(PointerEventData data)
    {
        ui.SetActive(true);
        Debug.Log("clicked");
    }


}
