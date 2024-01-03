using UnityEngine;
using UnityEngine.EventSystems;

public class TerrainController : MonoBehaviour, IPointerClickHandler
{
    private GameObject uiActions
    {
        get
        {
            return gameObject.GetComponent<TerrainShared>().uiManager.actions;
        }
    }
    private GameObject uiInformation
    {
        get
        {
            return gameObject.GetComponent<TerrainShared>().uiManager.information;
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

        foreach (Transform building in uiActions.transform)
        {
            building.gameObject.SetActive(false);
        }

        foreach (Transform building in uiInformation.transform)
        {
            building.gameObject.SetActive(false);
        }
    }
}