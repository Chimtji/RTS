using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public List<GameObject> selected = new List<GameObject>();

    public void SingleSelect(GameObject item)
    {
        DeselectAll();
        selected.Add(item);
        AddSelectionVisual(item);

    }
    public void AddSelect(GameObject item)
    {
        if (!selected.Contains(item))
        {
            selected.Add(item);
        }
        else
        {
            selected.Remove(item);
        }
    }
    public void BoxSelect(GameObject item)
    {
        if (!selected.Contains(item))
        {
            selected.Add(item);
            AddSelectionVisual(item);

        }
    }

    public void Deselect(GameObject item)
    {
        selected.Remove(item);
        RemoveSelectionVisual(item);
    }

    public void DeselectAll()
    {
        foreach (GameObject item in selected)
        {
            RemoveSelectionVisual(item);
        }
        selected.Clear();
    }

    private void AddSelectionVisual(GameObject item)
    {
        // Testing
        item.transform.Find("Model").GetComponent<Renderer>().material.color = Color.red;
    }
    private void RemoveSelectionVisual(GameObject item)
    {
        // Testing
        item.transform.Find("Model").GetComponent<Renderer>().material.color = Color.white;
    }
}