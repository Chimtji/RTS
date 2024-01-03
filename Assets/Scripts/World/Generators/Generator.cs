using UnityEngine;
using System;
using System.Collections.Generic;
using Trout.Utils;

public abstract class Generator : MonoBehaviour
{
    public bool autoUpdate = true;

    // This syntax makes so all derived classes from Generator must have their own definition of this method
    public abstract void Generate();

    public virtual void OnValidate()
    {
        // if (autoUpdate && !Application.isPlaying)
        // {
        //     Regenerate();
        // }
    }

    public void OnValuesChange()
    {
        // Regenerate();
    }

    private void Regenerate()
    {
        if (!Application.isPlaying && gameObject)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                Generate();
            };
        }
    }
}