using System.Collections.Generic;
using Trout.Utils;
using UnityEngine;
public class BoxSelection : MonoBehaviour
{
    public bool show;
    public Dictionary<int, GameObject> items = new Dictionary<int, GameObject>();
    public Vector3 startPosition;
    private Vector3 endPosition;
    private GameObject container;
    private new MeshCollider collider;

    private void Awake()
    {
        gameObject.AddComponent<Collider>();

        container = new GameObject("Collider");
        container.transform.SetParent(gameObject.transform);

        TriggerListener trigger = container.AddComponent<TriggerListener>();
        // trigger.triggerEnterEvent.AddListener(collider => Add(collider.gameObject));
        trigger.triggerAllEvent.AddListener(colliders => Add(colliders));
    }
    private void OnGUI()
    {
        if (show)
        {
            var rect = GUIBox.GetScreenRect(startPosition, Input.mousePosition);
            GUIBox.DrawScreenRect(rect, new Color(0, 0, 0.95f, 0.25f));
            GUIBox.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    public void Create(Vector2 start, Vector3 end)
    {
        collider = container.AddComponent<MeshCollider>();
        collider.convex = true;
        collider.isTrigger = true;

        startPosition = start;
        endPosition = end;
        Vector3[] verts = new Vector3[4];
        Vector2[] corners = CreateBoundingBox();

        for (int i = 0; i < corners.Length; i++)
        {
            Ray ray = Camera.main.ScreenPointToRay(corners[i]);

            if (Physics.Raycast(ray, out RaycastHit hit, 50000.0f, LayerMask.GetMask("Ground")))
            {
                // @TODO: Use the lowest point of the 4 corners as y
                verts[i] = hit.point;

            }
        }

        Mesh selectionMesh = GenerateSelectionMesh(verts);
        collider.sharedMesh = selectionMesh;
        RemoveAll();
    }
    public void Delete()
    {
        Destroy(collider);
    }
    public void Add(List<Collider> colliders)
    {
        foreach (Collider collider in colliders)
        {
            int id = collider.gameObject.GetInstanceID();

            if (!items.ContainsKey(id))
            {
                items.Add(id, collider.gameObject);
                collider.gameObject.AddComponent<UnitSelected>();

            }
        }
    }

    public void Remove(int id)
    {
        Destroy(items[id].GetComponent<UnitSelected>());
        items.Remove(id);
    }

    public void RemoveAll()
    {
        foreach (KeyValuePair<int, GameObject> pair in items)
        {
            if (pair.Value != null)
            {
                Destroy(items[pair.Key].GetComponent<UnitSelected>());
            }
        }
        items.Clear();
    }
    private Vector2[] CreateBoundingBox()
    {
        // Min and Max to get 2 corners of rectangle regardless of drag direction.
        var bottomLeft = Vector3.Min(startPosition, endPosition);
        var topRight = Vector3.Max(startPosition, endPosition);

        // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
        Vector2[] corners =
        {
            new Vector2(bottomLeft.x, topRight.y),
            new Vector2(topRight.x, topRight.y),
            new Vector2(bottomLeft.x, bottomLeft.y),
            new Vector2(topRight.x, bottomLeft.y)
        };
        return corners;
    }

    private Mesh GenerateSelectionMesh(Vector3[] corners)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + Vector3.up * 100.0f;
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }
}