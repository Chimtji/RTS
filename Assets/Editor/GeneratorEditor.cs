using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Generator), true)]
public class GeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Generator generator = (Generator)target;

        if (GUILayout.Button("Generate"))
        {
            generator.Generate();
        }
    }
}

