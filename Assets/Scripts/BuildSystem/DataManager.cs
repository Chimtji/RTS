using Trout.Utils;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public GameObject buildings;
    public GameObject units;

    public void ClearAll()
    {
        Utils.ClearChildren(buildings);
        Utils.ClearChildren(units);
    }
}