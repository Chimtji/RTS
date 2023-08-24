using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Trout.Utils;

public class BuildManager : MonoBehaviour
{

    public GameObject terrain;
    public TBuilding[] buildings;

    [Header("Blueprint")]
    public Material blueprintGrid;

    private TerrainChunkMap chunkMap;
    private InputManager inputManager;
    private Vector3 mousePosition;

    private TBuilding pickedBuilding;

    private GameObject blueprint;


    private bool inBuildMode = false;

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();
        chunkMap = terrain.GetComponent<TerrainChunkMap>();
    }

    void Update()
    {
        if (inBuildMode && blueprint != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
        }

    }

    private void FixedUpdate()
    {
        if (inBuildMode)
        {
            Vector3 nextPosition = inputManager.GetMousePosition();

            if (mousePosition != nextPosition)
            {
                mousePosition = nextPosition;
            }
        }
    }

    public void PlaceObject()
    {
        inBuildMode = false;
    }

    public void SelectObject(int index)
    {
        pickedBuilding = buildings[index];
        inBuildMode = true;

        mousePosition = inputManager.GetMousePosition();

        // blueprint = Instantiate(pickedBuilding.visualBlueprint, mousePosition, Quaternion.identity);
        blueprint = new GameObject("Blueprint");
        blueprint.AddComponent<Blueprint>().Setup(pickedBuilding, inputManager, chunkMap, blueprintGrid);

        BuildGridMap buildGridMap = gameObject.GetComponent<BuildGridMap>();
        buildGridMap.SetBuildMode(inBuildMode);
    }
}

