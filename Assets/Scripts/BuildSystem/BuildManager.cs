using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("Managers")]
    /// <summary>
    /// The Game Manager object in the scene
    /// </summary>
    public GameObject gameManager;

    /// <summary>
    /// The Terrain Manager object in the scene
    /// </summary>
    public GameObject terrainManager;

    /// <summary>
    /// The Data Manager object in the scene
    /// </summary>
    public GameObject dataManager;

    /// <summary>
    /// The Unit Manager object in the scene
    /// </summary>
    public GameObject unitManager;

    /// <summary>
    /// The UI Manager object in the scene
    /// </summary>
    public GameObject uiManager;

    [Header("Blueprint")]
    /// <summary>
    /// The shader used for the ground grid of the blueprint
    /// </summary>
    public Material blueprintGridMaterial;

    /// <summary>
    /// The shader that's applied on building model when it's in the blueprint state
    /// </summary>
    public Material blueprintMaterial;

    /// <summary>
    /// The bool to toggle build mode on/off
    /// </summary>
    public bool inBuildMode = false;

    /// <summary>
    /// The current position of the cursor;
    /// </summary>
    private Vector3 mousePosition;

    /// <summary>
    /// The building attributes of the current / selected building
    /// </summary>
    private BuildingData buildingData;

    /// <summary>
    /// The current / selected building gameobject
    /// </summary>
    private GameObject building;

    /// <summary>
    /// The player this client is controlling
    /// </summary>
    private Player player
    {
        get
        {
            return gameManager.GetComponent<GameManager>().settings.player;
        }
    }

    /// <summary>
    /// The manager for reading the input of mouse and keyboard
    /// </summary>
    private InputManager input
    {
        get
        {
            return gameObject.GetComponent<InputManager>();
        }
    }

    /// <summary>
    /// The chunkmap of the generated terrain
    /// </summary>
    private TerrainManager chunkMap
    {
        get
        {
            return terrainManager.GetComponent<TerrainManager>();
        }
    }

    /// <summary>
    /// The blueprint component attached to the building object
    /// </summary>
    private BlueprintController blueprint
    {
        get
        {
            return building.GetComponent<BlueprintController>();
        }
    }

    void Update()
    {
        if (inBuildMode)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Deselect();
            }
        }

    }

    private void FixedUpdate()
    {
        if (inBuildMode)
        {
            Vector3 nextPosition = input.GetMousePosition();

            if (mousePosition != nextPosition)
            {
                mousePosition = nextPosition;
            }
        }
    }

    public void CreateBuilding(BuildingData buildingData, Vector3 position)
    {
        this.buildingData = buildingData;
        CreateBuildingObject();

        building.transform.position = position;
        GameObject visual = Instantiate(buildingData.visual, Vector3.zero, Quaternion.identity, building.transform);
        visual.transform.localPosition = new Vector3(0, 0, 0);
        visual.name = "prefab";

        building.AddComponent<BuildingController>();

        building = null;

    }

    public void Select(BuildingData buildingData)
    {
        this.buildingData = buildingData;
        mousePosition = input.GetMousePosition();

        Destroy(building);
        building = null; // Is this bad?

        CreateBuildingObject();

        building.AddComponent<BlueprintController>();
        ChangeBuildMode(true);
    }

    public void StartConstruction()
    {
        blueprint.TearDown();
        building.AddComponent<ConstructionController>();
        ChangeBuildMode(false);
        building = null;
    }

    private void CreateBuildingObject()
    {
        if (building != null)
        {
            return;
        }

        GameObject buildingObject = new GameObject(buildingData.name);
        dataManager.GetComponent<DataManager>().AddBuilding(buildingObject);

        BuildingShared buildingShared = buildingObject.AddComponent<BuildingShared>();

        buildingShared.owner = player;
        buildingShared.attributes = buildingData;
        buildingShared.input = input;
        buildingShared.terrain = chunkMap;
        buildingShared.blueprintGridMaterial = blueprintGridMaterial;
        buildingShared.blueprintMaterial = blueprintMaterial;
        buildingShared.builder = this;
        buildingShared.trainer = unitManager.GetComponent<UnitManager>();
        buildingShared.uiActionsContainer = uiManager.GetComponent<UIManager>().actions;
        buildingShared.uiInformationContainer = uiManager.GetComponent<UIManager>().information;

        building = buildingObject;
    }

    private void Deselect()
    {
        ChangeBuildMode(false);
        Destroy(building);
    }

    private void ChangeBuildMode(bool buildMode)
    {
        inBuildMode = buildMode;
        BuildGridMap buildGridMap = gameObject.GetComponent<BuildGridMap>();
        buildGridMap.SetBuildMode(inBuildMode);
    }
}

