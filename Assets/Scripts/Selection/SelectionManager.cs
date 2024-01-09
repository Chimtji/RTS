using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public List<GameObject> selected = new List<GameObject>();

    public void SingleSelect(GameObject item)
    {
        DeselectAll();
        selected.Add(item);

        // Add selected visual logic here
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
        }
    }

    public void Deselect(GameObject item)
    {

    }

    public void DeselectAll()
    {
        selected.Clear();
    }
}