// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Trout.Utils;
// using UnityEngine;
// using UnityEngine.UI;

// public class SelectionManager : MonoBehaviour
// {
//     [SerializeField]
//     private GameObject ui;
//     [SerializeField]
//     private Color selectColor = new Color(0, 1, 0, 0.3f);

//     private static Rect selectionBox;
//     private static Image selectionImage;

//     private Vector2 startPosition;
//     private Vector2 endPosition;

//     private List<ISelectable> selected = new List<ISelectable>();

//     public static event Action<List<ISelectable>> selectionChanged;

//     private bool drawSelection = false;

//     private void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             startPosition = Input.mousePosition;
//             ClearSelection();
//         }
//         if (Input.GetMouseButton(0))
//         {
//             endPosition = Input.mousePosition;
//             drawSelection = true;
//         }
//         if (Input.GetMouseButtonUp(0))
//         {
//             drawSelection = false;
//             Select();
//         }
//     }

//     private void OnGUI()
//     {

//         if (drawSelection)
//         {
//             selectionBox = GUIBox.GetScreenRect(startPosition, Input.mousePosition);
//             GUIBox.DrawScreenRect(selectionBox, new Color(0, 0, 0.95f, 0.25f));
//             GUIBox.DrawScreenRectBorder(selectionBox, 2, new Color(0.8f, 0.8f, 0.95f));
//         }

//     }

//     private Vector3[] OrientPoints(Vector3[] corners)
//     {
//         // Bottom Left -> Top Left -> Top Right -> Bottom Right
//         Vector3[] oriented = new Vector3[4];

//         if (startPosition.x < endPosition.x && startPosition.y < endPosition.y)
//         {
//             oriented[0] = corners[0]; // bottom left;
//             oriented[1] = corners[1]; // top left
//             oriented[2] = corners[2]; // top right
//             oriented[3] = corners[3]; // bottom right
//         }
//         else if (startPosition.x < endPosition.x && startPosition.y >= endPosition.y)
//         {
//             oriented[0] = corners[1]; // bottom left;
//             oriented[1] = corners[0]; // top left
//             oriented[2] = corners[2]; // top right
//             oriented[3] = corners[3]; // bottom right
//         }
//         else if (startPosition.x >= endPosition.x && startPosition.y >= endPosition.y)
//         {
//             oriented[0] = corners[3]; // bottom left;
//             oriented[1] = corners[2]; // top left
//             oriented[2] = corners[0]; // top right
//             oriented[3] = corners[1]; // bottom right
//         }
//         else
//         {
//             oriented[0] = corners[2]; // bottom left;
//             oriented[1] = corners[3]; // top left
//             oriented[2] = corners[1]; // top right
//             oriented[3] = corners[0]; // bottom right
//         }

//         return oriented;
//     }

//     private Quaternion CalculateRotation(Vector3[] orientedPoints)
//     {
//         Vector3 x = (orientedPoints[3] - orientedPoints[0]).normalized;
//         Vector3 y = new Vector3();
//         Vector3 z = Vector3.Cross(x, y);

//         Matrix4x4 rotationMatrix = new Matrix4x4(x, y, z, new Vector4(0, 0, 0, 1));

//         return rotationMatrix.rotation;
//     }

//     private Vector2[] GetSelectionBoxCorners()
//     {
//         // Min and Max to get 2 corners of rectangle regardless of drag direction.
//         var bottomLeft = Vector3.Min(startPosition, endPosition);
//         var topRight = Vector3.Max(startPosition, endPosition);

//         // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
//         Vector2[] corners =
//         {
//             new Vector2(bottomLeft.x, topRight.y),
//             new Vector2(topRight.x, topRight.y),
//             new Vector2(bottomLeft.x, bottomLeft.y),
//             new Vector2(topRight.x, bottomLeft.y)
//         };
//         return corners;
//     }

//     private Vector3[] GetSelectionBoxWorldCorners()
//     {
//         Vector2[] cornersSelectionBox = GetSelectionBoxCorners();
//         Vector3[] cornersWorld = new Vector3[4];
//         RaycastHit[] hits = new RaycastHit[4];

//         for (int i = 0; i < 4; i++)
//         {
//             Physics.Raycast(Camera.main.ScreenPointToRay(cornersSelectionBox[i]), out hits[i], 5000.0f, LayerMask.GetMask("Ground"));
//             cornersWorld[i] = hits[i].point;
//         }

//         return cornersWorld;
//     }

//     private void OnDrawGizmos()
//     {
//         Gizmos.color = Color.red;
//         if (Input.GetMouseButton(0))
//         {
//             Vector3[] corners = GetSelectionBoxWorldCorners();
//             Vector3[] sortedCornersByY = corners.OrderBy(corner => corner.y).ToArray();
//             Vector3 center = Vector3.zero;

//             corners = corners.Select(corner => new Vector3(corner.x, sortedCornersByY[0].y, corner.z)).ToArray();

//             foreach (Vector3 corner in corners)
//             {
//                 center += corner;
//                 Gizmos.DrawWireSphere(new Vector3(corner.x, 1f, corner.z), 0.5f);
//             }
//             center /= corners.Length;




//             Vector3[] oriented = OrientPoints(corners);
//             Quaternion rotation = CalculateRotation(oriented);

//             // Vector3 widthVector = (oriented[2] - oriented[1] + oriented[3] - oriented[0]) / 2;
//             // Vector3 heightVector = (oriented[1] - oriented[0] + oriented[2] - oriented[3]) / 2;
//             // Vector3 size = new Vector3(widthVector.magnitude, heightVector.magnitude, 99);

//             // float width = Vector3.Distance(oriented[1], oriented[2]);
//             // float height = Vector3.Distance(oriented[1], oriented[0]);

//             // Vector3 size = new Vector3(width, height, 10);

//             // Gizmos.matrix = Matrix4x4.TRS(center, rotation, size);
//             Gizmos.DrawWireCube(center, Vector3.one);

//             // Gizmos.DrawWireCube(oriented[0], Vector3.one);
//             Gizmos.DrawLine(oriented[0], oriented[1]);

//         }
//     }

//     private void ClearSelection()
//     {
//         foreach (var item in selected)
//         {
//             item.OnDeselect();
//         }
//         selected.Clear();
//         // selectionChanged.Invoke(selected);
//     }

//     private void Select()
//     {

//     }
// }