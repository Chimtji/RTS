using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Vector3 position;

    [SerializeField]
    private LayerMask layerMask;

    public Vector3 GetMousePosition()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 9999, layerMask))
        {
            position = hit.point;
        }

        return position;
    }
}
