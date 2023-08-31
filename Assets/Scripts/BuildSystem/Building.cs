using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public TBuilding attributes;

    private GameObject ui;

    public void Setup(TBuilding attributes)
    {
        this.attributes = attributes;

        CreateUI();

    }

    private void CreateUI()
    {
        ui = new GameObject("UI");
        ui.transform.parent = transform;
        Canvas canvas = ui.AddComponent<Canvas>();
        CanvasScaler canvasScaler = ui.AddComponent<CanvasScaler>();
        GraphicRaycaster graphicRaycaster = ui.AddComponent<GraphicRaycaster>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

    }
}
