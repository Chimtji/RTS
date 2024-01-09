using UnityEngine;

public class UnitSelected : MonoBehaviour
{
    void Start()
    {
        transform.Find("Model").GetComponent<Renderer>().material.color = Color.red;
    }
    private void OnDestroy()
    {
        transform.Find("Model").GetComponent<Renderer>().material.color = Color.white;
    }
}