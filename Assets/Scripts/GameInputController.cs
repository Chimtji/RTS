using System.Collections.Generic;
using UnityEngine;

public class GameInputController : MonoBehaviour
{
    private InputHandler InputHandler
    {
        get
        {
            return gameObject.GetComponent<InputHandler>();
        }
    }
    private Vector3 startPosition;

    private void Awake()
    {
        // InputHandler.selection.AddListener((items) =>
        // {
        //     Debug.Log("Consumer: " + items.Count);
        // });
    }

    private void Update()
    {

    }

    private void Select(Vector3 start, Vector3 end)
    {

    }
}