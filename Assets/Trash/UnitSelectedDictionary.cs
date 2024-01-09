

using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedDictionary : MonoBehaviour
{
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    public void Add(GameObject unit)
    {
        int id = unit.GetInstanceID();

        if (!selectedTable.ContainsKey(id))
        {
            selectedTable.Add(id, unit);
            unit.AddComponent<UnitSelected>();

        }
    }

    public void Remove(int id)
    {
        Destroy(selectedTable[id].GetComponent<UnitSelected>());
        selectedTable.Remove(id);
    }

    public void RemoveAll()
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedTable)
        {
            if (pair.Value != null)
            {
                Destroy(selectedTable[pair.Key].GetComponent<UnitSelected>());
            }
        }
        selectedTable.Clear();
    }
}