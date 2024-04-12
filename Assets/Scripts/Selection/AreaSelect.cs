using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AreaSelect : MonoBehaviour
{
    [SerializeField]
    private DataManager dataManager;
    [SerializeField]
    private RectTransform boxVisual;
    [SerializeField]
    private GameObject colliderContainer;


    /// <summary>
    /// The camera projection type: 
    /// true = orthographic
    /// false = perspective
    /// </summary>
    private bool orthographicCamera
    {
        get
        {
            return Camera.main.orthographic;
        }
    }



    private SelectionManager selectionManager
    {
        get
        {
            return GetComponent<SelectionManager>();
        }
    }
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool showVisual = false;
    private float dragThreshold = 20f;

    private List<GameObject> temporarySelection = new List<GameObject>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;

            showVisual = true;
            SetBoxScreenSize();

            if ((startPosition - Input.mousePosition).magnitude > dragThreshold)
            {
                if (!orthographicCamera)
                {
                    // DrawAreaInWorld();
                }
                else
                {
                    // do persepctive calculations instead
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Select();

            SetBoxScreenSize();
            showVisual = false;

            ResetTemporarySelection();
        }
    }

    private void OnDrawGizmos()
    {
        DrawAreaInWorld();
    }

    /// <summary>
    /// This draws the Area for an Orthograpic Camera
    /// </summary>
    private void DrawAreaInWorld()
    {
        Gizmos.matrix = Matrix4x4.identity;
        Vector2[] cornersScreen = CalculateBoxCorners();

        Vector3 topLeftScreen = cornersScreen[0];
        Vector3 topRightScreen = cornersScreen[1];
        Vector3 bottomLeftScreen = cornersScreen[2];
        float screenToWorldRelation = Vector2.Distance(topLeftScreen, bottomLeftScreen) / Vector2.Distance(topLeftScreen, topRightScreen);

        // Bottom Left Corner
        Ray rayLeft = Camera.main.ScreenPointToRay(cornersScreen[2]);
        if (Physics.Raycast(rayLeft, out RaycastHit hitLeft, 50000.0f, LayerMask.GetMask("Ground")))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayLeft.origin, rayLeft.direction * hitLeft.distance);
        }

        // Bottom Right Corner
        Ray rayRight = Camera.main.ScreenPointToRay(cornersScreen[3]);
        if (Physics.Raycast(rayRight, out RaycastHit hitRight, 50000.0f, LayerMask.GetMask("Ground")))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayRight.origin, rayRight.direction * hitRight.distance);
        }

        Vector3 bottomLeftWorld = hitLeft.point;
        Vector3 bottomRightWorld = hitRight.point;


        float width = Vector3.Distance(bottomLeftWorld, bottomRightWorld);
        float height = width * screenToWorldRelation;

        Vector3 rayDirection = Camera.main.transform.rotation * Quaternion.AngleAxis(-90, hitLeft.transform.right) * hitLeft.transform.forward;
        Ray leftSideRay = new Ray(bottomLeftWorld, rayDirection);
        Vector3 topLeftWorld = leftSideRay.GetPoint(height);

        Ray rightSideRay = new Ray(bottomRightWorld, rayDirection);
        Vector3 topRightWorld = rightSideRay.GetPoint(height);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(bottomLeftWorld, bottomRightWorld);
        Gizmos.DrawLine(bottomLeftWorld, topLeftWorld);
        Gizmos.DrawLine(bottomRightWorld, topRightWorld);
        Gizmos.DrawLine(topLeftWorld, topRightWorld);

        Vector3 boxCenter = (topLeftWorld + topRightWorld + bottomLeftWorld + bottomRightWorld) / 4;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Camera.main.transform.position, boxCenter);

        Vector3 direction = boxCenter - Camera.main.transform.position;
        Matrix4x4 rotation = Matrix4x4.TRS(boxCenter, Camera.main.transform.rotation, Camera.main.transform.lossyScale);
        Gizmos.matrix = rotation;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, 10f));
    }

    /// <summary>
    /// Adds all items in temporarySelect array to the selection manager ie. selection
    /// </summary>
    private void Select()
    {
        foreach (GameObject item in temporarySelection)
        {
            selectionManager.BoxSelect(item);
        }
    }

    /// <summary>
    /// Clears the temporary select array
    /// </summary>
    private void ResetTemporarySelection()
    {
        temporarySelection.Clear();
    }

    /// <summary>
    /// Sets the size of UI box image used for visualizing the area select.
    /// </summary>
    private void SetBoxScreenSize()
    {
        Vector2 boxStart = showVisual ? startPosition : Vector2.zero;
        Vector2 boxEnd = showVisual ? endPosition : Vector2.zero;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.position = boxCenter;
        boxVisual.sizeDelta = boxSize;
    }

    /// <summary>
    /// Calculates the corners of the selection box based on start and end position of mouse in screen space.
    /// </summary>
    private Vector2[] CalculateBoxCorners()
    {
        Vector3 bottomLeftPoint = Vector3.Min(startPosition, endPosition);
        Vector3 topRightPoint = Vector3.Max(startPosition, endPosition);

        Vector2 topLeft = new Vector2(bottomLeftPoint.x, topRightPoint.y);
        Vector2 topRight = new Vector2(topRightPoint.x, topRightPoint.y);
        Vector2 bottomLeft = new Vector2(bottomLeftPoint.x, bottomLeftPoint.y);
        Vector2 bottomRight = new Vector2(topRightPoint.x, bottomLeftPoint.y);

        Vector2[] corners =
        {
            topLeft,
            topRight,
            bottomLeft,
            bottomRight
        };

        return corners;
    }
}