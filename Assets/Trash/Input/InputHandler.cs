using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    [Header("Mouse")]
    public UnityEvent leftMouseClicked = new UnityEvent();
    public UnityEvent rightMouseClicked = new UnityEvent();
    public UnityEvent<Dictionary<int, GameObject>> selection = new UnityEvent<Dictionary<int, GameObject>>();

    [Header("GUI")]
    public bool isGUIDragging = false;
    public Vector3 GUIDragStart;
    public Vector3 GUIDragEnd;
    public UnityEvent<Vector3, Vector3> onGUIDragDone = new UnityEvent<Vector3, Vector3>();
    private float GUIDragThreshold = 20f;
    private BoxSelection boxSelection;

    private void Awake()
    {
        boxSelection = gameObject.AddComponent<BoxSelection>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GUIDragStart = Input.mousePosition;
            boxSelection.startPosition = GUIDragStart;
            leftMouseClicked.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseClicked.Invoke();
        }
        if (Input.GetMouseButton(0))
        {
            if ((GUIDragStart - Input.mousePosition).magnitude > GUIDragThreshold)
            {
                isGUIDragging = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            GUIDragEnd = Input.mousePosition;
            onGUIDragDone.Invoke(GUIDragStart, GUIDragEnd);

            boxSelection.Create(GUIDragStart, GUIDragEnd);
            selection.Invoke(boxSelection.items);

            isGUIDragging = false;
            boxSelection.show = false;
        }
    }

    private void OnGUI()
    {
        if (isGUIDragging)
        {
            boxSelection.show = true;
        }
    }

    public Vector3 GUIPositionToWorld(Vector3 guiPos)
    {

        Ray ray = Camera.main.ScreenPointToRay(guiPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 5000.0f))
        {
            return hit.point;
        }


        // @TODO: Fix this. should not be a vector zero
        return new Vector3(0, 0, 0);
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 9999))
        {
            return hit.point;
        }

        // @TODO: Fix this. should not be a vector zero
        return new Vector3(0, 0, 0);
    }

}
