using System.Collections.Generic;
using Trout.Utils;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    [SerializeField]
    private GameObject buildingsContainer;
    [SerializeField]
    private GameObject unitsContainer;

    public List<GameObject> buildings;
    public List<GameObject> units;

    public void AddBuilding(GameObject building)
    {
        buildings.Add(building);
        building.transform.SetParent(buildingsContainer.transform);
    }
    public void AddUnit(GameObject unit)
    {
        units.Add(unit);
        unit.transform.SetParent(unitsContainer.transform);
    }

    public void ClearAll()
    {
        // units.Clear();
        buildings.Clear();

        Utils.ClearChildren(buildingsContainer);
        // Utils.ClearChildren(unitsContainer);
    }
}