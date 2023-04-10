using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trout.Utils;

public class Testing : MonoBehaviour
{

    [SerializeField] private TilemapVisual tilemapVisual;
    private Tilemap tilemap;

    private void Start()
    {
        tilemap = new Tilemap(10, 10, 10f, Vector3.zero);

        tilemap.SetTilemapVisual(tilemapVisual);

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Mouse3D.GetMouseWorldPosition3D();
            tilemap.SetTilemapSprite(mouseWorldPosition, Tilemap.TilemapObject.TilemapSprite.Ground);
        }
    }
}