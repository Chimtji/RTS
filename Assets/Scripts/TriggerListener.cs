using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerListener : MonoBehaviour
{
    public UnityEvent triggerEnterEvent = new UnityEvent();
    public UnityEvent triggerExitEvent = new UnityEvent();

    void OnTriggerEnter(Collider other)
    {
        if (triggerEnterEvent != null)
        {
            triggerEnterEvent.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerExitEvent != null)
        {
            triggerExitEvent.Invoke();
        }
    }
}
