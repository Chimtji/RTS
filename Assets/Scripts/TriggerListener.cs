using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerListener : MonoBehaviour
{
    public UnityEvent<Collider> triggerEnterEvent = new UnityEvent<Collider>();
    public UnityEvent<Collider> triggerExitEvent = new UnityEvent<Collider>();
    public LayerMask layerMask;

    void OnTriggerEnter(Collider other)
    {
        if (triggerEnterEvent != null)
        {
            if (other.gameObject.layer == layerMask.value)
            {
                Debug.Log("Enter: " + other.gameObject.transform.position);
                triggerEnterEvent.Invoke(other);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("exit: " + other);

        if (triggerExitEvent != null)
        {
            triggerExitEvent.Invoke(other);
        }
    }
}
