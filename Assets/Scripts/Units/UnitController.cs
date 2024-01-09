using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        // Temporary -- for testing purposes
        GameObject manager = GameObject.Find("Data Manager");
        Debug.Log(manager);
        DataManager data = manager.GetComponent<DataManager>();
        Debug.Log(data);

        data.AddUnit(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
