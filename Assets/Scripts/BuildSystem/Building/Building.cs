using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UI;

public class Building
{
    public GameObject visual;
    public TBuilding attributes;

    private GameObject ui;

    public Building(TBuilding attributes, Vector3 position)
    {
        this.attributes = attributes;

        Setup(position);
    }

    public void Setup(Vector3 position)
    {
        visual = Object.Instantiate(attributes.visual, position, Quaternion.identity);
        GameObject container = GameObject.Find("Data/Buildings");
        visual.transform.SetParent(container.transform);

        CreateUI();

        BuildingController controller = visual.AddComponent<BuildingController>();
        controller.ui = ui;
    }

    private void CreateUI()
    {
        GameObject container = GameObject.Find("UI/Layout/Actions");
        ui = new GameObject(attributes.name);
        ui.transform.SetParent(container.transform);
        Canvas canvas = ui.AddComponent<Canvas>();
        ui.AddComponent<CanvasScaler>();
        ui.AddComponent<GraphicRaycaster>();
        ui.SetActive(false);

        RectTransform uiRect = ui.GetComponent<RectTransform>();
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
        gridLayout.cellSize = new Vector2(50, 50);
        gridLayout.padding.top = 5;
        gridLayout.padding.bottom = 5;
        gridLayout.padding.left = 5;
        gridLayout.padding.right = 5;
        gridLayout.spacing = new Vector2(5, 5);

        GameObject buttonA = new GameObject("ButtonA");
        buttonA.transform.SetParent(layout.transform);
        buttonA.AddComponent<CanvasRenderer>();
        buttonA.AddComponent<Image>();
        buttonA.AddComponent<Button>();

        GameObject buttonB = new GameObject("ButtonB");
        buttonB.transform.SetParent(layout.transform);
        buttonB.AddComponent<CanvasRenderer>();
        buttonB.AddComponent<Image>();
        buttonB.AddComponent<Button>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
}
