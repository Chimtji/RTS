using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trout.Utils
{
    public class Mouse : MonoBehaviour
    {

        public static Mouse Instance { get; private set; }

        [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
            {
                transform.position = raycastHit.point;
            }
        }

        public static Vector3 GetMouseWorldPosition3D() => Instance.GetMouseWorldPosition_Instance();

        private Vector3 GetMouseWorldPosition_Instance()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
            {
                return raycastHit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }

    }

}
