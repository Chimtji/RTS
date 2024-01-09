using Trout.Utils;
using UnityEngine;
using System.Collections;

public class UnitGlobalSelection : MonoBehaviour
{
    private InputHandler inputHandler;

    private UnitSelectedDictionary selectedTable;
    // private RaycastHit hit;
    private MeshCollider selectionBox;
    private Mesh selectionMesh;

    /// <summary>
    /// Corners of the 2D selection box;
    /// </summary>
    private Vector2[] corners;
    private Vector3[] verts;

    void Awake()
    {
        // inputHandler = gameObject.AddComponent<InputHandler>();
        // inputHandler.leftMouseClicked.AddListener(() => Debug.Log("clicked"));
    }

    // void Start()
    // {
    //     selectedTable = GetComponent<UnitSelectedDictionary>();
    // }
    void Update()
    {
        // if (inputHandler.isGUIDragging)
        // {
        //     verts = new Vector3[4];
        //     corners = GetBoundingBox(inputHandler.GUIDragStart, inputHandler.GUIDragEnd);

        //     for (int i = 0; i < corners.Length; i++)
        //     {
        //         Ray ray = Camera.main.ScreenPointToRay(corners[i]);

        //         if (Physics.Raycast(ray, out RaycastHit hit, 50000.0f, LayerMask.GetMask("Ground")))
        //         {
        //             verts[i] = new Vector3(hit.point.x, 0, hit.point.z);
        //             Debug.DrawLine(Camera.main.ScreenToWorldPoint(corners[i]), hit.point, Color.red, 1.0f);
        //         }
        //     }

        //     selectionMesh = GenerateSelectionMesh(verts);
        //     selectionBox = gameObject.AddComponent<MeshCollider>();
        //     selectionBox.sharedMesh = selectionMesh;
        //     selectionBox.convex = true;
        //     selectionBox.isTrigger = true;

        //     if (!Input.GetKey(KeyCode.LeftShift))
        //     {
        //         selectedTable.RemoveAll();
        //     }

        //     Destroy(selectionBox, 0.02f);
        // }

        // if (Input.GetMouseButtonUp(0))
        // {
        //     if (dragSelect == false)
        //     {
        //         Ray ray = Camera.main.ScreenPointToRay(p1);

        //         if (Physics.Raycast(ray, out RaycastHit hit, 50000.0f, LayerMask.GetMask("Unit")))
        //         {
        //             if (Input.GetKey(KeyCode.LeftShift))
        //             {
        //                 selectedTable.Add(hit.transform.gameObject);
        //             }
        //             else
        //             {
        //                 selectedTable.RemoveAll();
        //                 selectedTable.Add(hit.transform.gameObject);
        //             }
        //         }
        //         else
        //         {
        //             if (!Input.GetKey(KeyCode.LeftShift))
        //             {
        //                 selectedTable.RemoveAll();

        //             }
        //         }
        //     }
        //     dragSelect = false;
        // }
    }

    // private void OnGUI()
    // {
    //     if (inputHandler.isGUIDragging)
    //     {
    //         var rect = GUIMesh.GetScreenRect(inputHandler.GUIDragStart, Input.mousePosition);
    //         GUIMesh.DrawScreenRect(rect, new Color(0, 0, 0.95f, 0.25f));
    //         GUIMesh.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
    //     }
    // }

    // private Vector2[] GetBoundingBox(Vector2 p1, Vector2 p2)
    // {
    //     // Min and Max to get 2 corners of rectangle regardless of drag direction.
    //     var bottomLeft = Vector3.Min(p1, p2);
    //     var topRight = Vector3.Max(p1, p2);

    //     // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
    //     Vector2[] corners =
    //     {
    //         new Vector2(bottomLeft.x, topRight.y),
    //         new Vector2(topRight.x, topRight.y),
    //         new Vector2(bottomLeft.x, bottomLeft.y),
    //         new Vector2(topRight.x, bottomLeft.y)
    //     };
    //     return corners;
    // }

    // private Mesh GenerateSelectionMesh(Vector3[] corners)
    // {
    //     Vector3[] verts = new Vector3[8];
    //     int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

    //     for (int i = 0; i < 4; i++)
    //     {
    //         verts[i] = corners[i];
    //     }

    //     for (int j = 4; j < 8; j++)
    //     {
    //         verts[j] = corners[j - 4] + Vector3.up * 100.0f;
    //     }

    //     Mesh selectionMesh = new Mesh();
    //     selectionMesh.vertices = verts;
    //     selectionMesh.triangles = tris;

    //     return selectionMesh;
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     selectedTable.Add(other.gameObject);
    // }
}