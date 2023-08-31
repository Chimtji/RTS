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
    private GameObject construction;
    private GameObject building;


    private bool inBuildMode = false;

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();
        chunkMap = terrain.GetComponent<TerrainChunkMap>();
    }

    void Update()
    {
        if (inBuildMode && blueprint != null && blueprint.GetComponent<Blueprint>().isPlaceable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Build();
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

    public void Build()
    {
        construction = new GameObject("Construction");
        Vector3 buildPosition = blueprint.GetComponent<Blueprint>().buildPosition;
        construction.AddComponent<Construction>().Setup(pickedBuilding, buildPosition);
        Destroy(blueprint);
        ChangeBuildMode(false);
    }

    public void SelectObject(int index)
    {
        pickedBuilding = buildings[index];
        ChangeBuildMode(true);

        mousePosition = inputManager.GetMousePosition();

        if (blueprint == null)
        {
            blueprint = new GameObject("Blueprint");
            blueprint.AddComponent<Blueprint>().Setup(pickedBuilding, inputManager, chunkMap, blueprintGrid);
        }
        else
        {
            blueprint.GetComponent<Blueprint>().Replace(pickedBuilding);
        }

    }

    private void ChangeBuildMode(bool buildMode)
    {
        inBuildMode = buildMode;
        BuildGridMap buildGridMap = gameObject.GetComponent<BuildGridMap>();
        buildGridMap.SetBuildMode(inBuildMode);
    }
}

