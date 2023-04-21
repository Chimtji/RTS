using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VegetationGenerator))]
public class VegetationGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VegetationGenerator vegetationGenerator = (VegetationGenerator)target;

        if (DrawDefaultInspector())
        {
            // if (vegetationGenerator.autoUpdate)
            // {
            //     vegetationGenerator.GenerateInEditor();
            // }
        };

        if (GUILayout.Button("Generate"))
        {
            vegetationGenerator.Generate();
        }
    }
}
