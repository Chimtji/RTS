using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [Header("GUI")]
    public bool isGUIDragging = false;
    public Vector3 GUIDragStart;
    public Vector3 GUIDragEnd;
    public UnityEvent<Vector3, Vector3> onGUIDragDone = new UnityEvent<Vector3, Vector3>();
    private float GUIDragThreshold = 40f;

    [SerializeField]
    private LayerMask layerMask;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GUIDragStart = Input.mousePosition;
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
            isGUIDragging = false;
            GUIDragEnd = Input.mousePosition;
            onGUIDragDone.Invoke(GUIDragStart, GUIDragEnd);
        }
    }

    public Vector3 GUIPositionToWorld(Vector3 guiPos)
    {

        Ray ray = Camera.main.ScreenPointToRay(guiPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 5000.0f, layerMask))
        {
            return hit.point;
        }


        // @TODO: Fix this. should not be a vector zero
        return new Vector3(0, 0, 0);
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 9999, layerMask))
        {
            return hit.point;
        }

        // @TODO: Fix this. should not be a vector zero
        return new Vector3(0, 0, 0);
    }
}
