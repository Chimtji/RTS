using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour, IPointerClickHandler
{
    public BuildingData attributes;
    public GameObject uiActions;
    public GameObject uiActionsContainer;
    public BuildManager builder;
    public Player owner;

    void Awake()
    {
        BuildingShared shared = gameObject.GetComponent<BuildingShared>();
        this.owner = shared.owner;
        this.attributes = shared.attributes;
        this.uiActionsContainer = shared.uiActionsContainer;
        this.builder = shared.builder;
        CreateUiActions();
    }

    // @TODO: Create validation, so you can't build whatever building sent to this method, but only the ones
    // that are available in attributes.
    public void Build(BuildingData data)
    {
        builder.Select(data);
    }

    public void OnPointerClick(PointerEventData data)
    {
        foreach (Transform child in uiActionsContainer.transform)
        {
            child.gameObject.SetActive(false);
        }

        uiActions.SetActive(true);
    }

    private void CreateUiActions()
    {
        GameObject ui = new GameObject(attributes.name);
        ui.transform.SetParent(uiActionsContainer.transform);
        Canvas canvas = ui.AddComponent<Canvas>();
        ui.AddComponent<CanvasScaler>();
        ui.AddComponent<GraphicRaycaster>();
        ui.SetActive(false);

        RectTransform uiRect = ui.GetComponent<RectTransform>();
        uiRect.localScale = new Vector3(1, 1, 1);
        uiRect.anchorMin = new Vector2(0, 0);
        uiRect.anchorMax = new Vector2(1, 1);
        uiRect.pivot = new Vector2(0.5f, 0.5f);
        uiRect.position = new Vector3(0, 0, 0);
        uiRect.sizeDelta = new Vector3(0, 0, 0);
        uiRect.anchoredPosition = new Vector2(0, 0);

        GameObject layout = new GameObject("Layout");
        layout.AddComponent<CanvasRenderer>();
        GridLayoutGroup gridLayout = layout.AddComponent<GridLayoutGroup>();
        layout.transform.SetParent(ui.transform);

        RectTransform layoutRect = layout.GetComponent<RectTransform>();
        layoutRect.anchorMin = new Vector2(0, 0);
        layoutRect.anchorMax = new Vector2(1, 1);
        layoutRect.pivot = new Vector2(0.5f, 0.5f);
        layoutRect.position = new Vector3(0, 0, 0);
        layoutRect.sizeDelta = new Vector3(0, 0, 0);
        layoutRect.anchoredPosition = new Vector2(0, 0);
        layoutRect.localScale = new Vector3(1, 1, 1);
        gridLayout.cellSize = new Vector2(50, 50);
        gridLayout.padding.top = 5;
        gridLayout.padding.bottom = 5;
        gridLayout.padding.left = 5;
        gridLayout.padding.right = 5;
        gridLayout.spacing = new Vector2(5, 5);

        foreach (BuildingData buildingData in attributes.buildActions)
        {
            GameObject button = new GameObject(buildingData.name);
            button.transform.SetParent(layout.transform);
            button.AddComponent<CanvasRenderer>();
            button.AddComponent<Image>();
            Button buttonComp = button.AddComponent<Button>();
            button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            buttonComp.onClick.AddListener(() => Build(buildingData));
        }
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        uiActions = ui;
    }


}
