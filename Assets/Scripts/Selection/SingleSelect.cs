using UnityEngine;

public class SingleSelect : MonoBehaviour
{
    public LayerMask selectable;
    public LayerMask ground;

    private SelectionManager selectionManager
    {
        get
        {
            return GetComponent<SelectionManager>();
        }
    }
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectable))
            {

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    selectionManager.AddSelect(hit.collider.gameObject);
                }
                else
                {
                    selectionManager.SingleSelect(hit.collider.gameObject);
                }

            }
            else
            {
                selectionManager.DeselectAll();
            }
        }
    }
}