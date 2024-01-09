using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerListener : MonoBehaviour
{
    public UnityEvent<Collider> triggerEnterEvent = new UnityEvent<Collider>();
    public UnityEvent<Collider> triggerExitEvent = new UnityEvent<Collider>();
    public UnityEvent<List<Collider>> triggerAllEvent = new UnityEvent<List<Collider>>();
    private List<Collider> colliders = new List<Collider>();
    private float wait = 0.1f;
    private float lastEnter;
    private bool enableAllTrigger = false;

    private void Awake()
    {
        lastEnter = Time.time;
    }

    private void Update()
    {
        if (enableAllTrigger && Time.time >= lastEnter + wait)
        {
            enableAllTrigger = false;
            if (triggerAllEvent != null)
            {
                triggerAllEvent.Invoke(colliders);
                colliders.Clear();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        enableAllTrigger = true;
        colliders.Add(other);

        if (triggerEnterEvent != null)
        {

            triggerEnterEvent.Invoke(other);
        }

        lastEnter = Time.time;
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerExitEvent != null)
        {
            triggerExitEvent.Invoke(other);
        }
    }
}
