using System.Collections.Generic;
using Trout.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerrainController : MonoBehaviour, IPointerClickHandler
{
    private List<GameObject> uiSelectedContainers
    {
        get
        {
            string parentContainer = "UI/Layout";
            string[] containerNames = { parentContainer + "/Actions", parentContainer + "/Information" };
            List<GameObject> containers = new List<GameObject>();

            foreach (string name in containerNames)
            {
                GameObject container = GameObject.Find(name);
                containers.Add(container);
            }

            return containers;
        }
    }

    /// <summary>
    /// Triggers whenever the terrain is clicked
    /// </summary>
    /// <param name="data">The data from the click event</param>
    public void OnPointerClick(PointerEventData data)
    {

        if (data.button == PointerEventData.InputButton.Left)
        {
            UnselectUi();
        }
    }

    /// <summary>
    /// When unselecting anything this hides all selected ui, so the ui are clean and "reset"
    /// </summary>
    private void UnselectUi()
    {
        foreach (GameObject container in uiSelectedContainers)
        {
            int childCount = container.transform.childCount;

            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject child = container.transform.GetChild(i).gameObject;
                child.SetActive(false);
            }
        }
    }
}