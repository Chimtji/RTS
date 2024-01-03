using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator
{
    Material material;
    float height;
    Mesh mesh;

    GameObject container;

    int stackSize = 20;
    float offset;
    Matrix4x4 matrix;
    int layer;

    public CloudGenerator(Material material, float height, Mesh mesh, GameObject container)
    {
        this.material = material;
        this.height = height;
        this.mesh = mesh;
        this.container = container;

        container.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        this.material.SetFloat("_Cloud_Height", height);

        Generate();
    }

    public void Generate()
    {
        offset = height / stackSize / 2f;
        Vector3 startPosition = container.transform.position + (Vector3.up * (offset * stackSize / 2f));

        for (int i = 0; i < stackSize; i++)
        {
            matrix = Matrix4x4.TRS(startPosition - (Vector3.up * offset * i), container.transform.rotation, container.transform.localScale);
            Graphics.DrawMesh(mesh, matrix, material, layer, container.GetComponent<Camera>(), 0, null, true, false, false);
        }
    }
}
