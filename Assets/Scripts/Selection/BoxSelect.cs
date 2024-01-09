using UnityEngine;

public class BoxSelect : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    private DataManager dataManager;

    [SerializeField]
    private RectTransform boxVisual;
    private SelectionManager selectionManager
    {
        get
        {
            return GetComponent<SelectionManager>();
        }
    }

    private Rect selectionBox;
    private Vector2 startPosition;
    private Vector2 endPosition;

    void Awake()
    {
        cam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            selectionBox = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Select();
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    private void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
        boxVisual.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        if (Input.mousePosition.x < startPosition.x)
        {
            // Dragging Left
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;

        }
        else
        {
            // Dragging Right
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < startPosition.y)
        {
            // Dragging Down
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            // Dragging Up
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    private void Select()
    {
        foreach (GameObject item in dataManager.GetComponent<DataManager>().units)
        {
            if (selectionBox.Contains(cam.WorldToScreenPoint(item.transform.position)))
            {
                selectionManager.BoxSelect(item);
            }
        }
    }
}