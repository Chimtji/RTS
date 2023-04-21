using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public GameObject building;

    RaycastHit hit;
    Vector3 movePoint;

    // Start is called before the first frame update
    void Start()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000.0f))
        {
            transform.position = hit.point;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);



        if (Physics.Raycast(ray, out hit, 50000.0f))
        {
            transform.position = hit.point;
        }

        if (Input.GetMouseButton(0))
        {
            Instantiate(building, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
