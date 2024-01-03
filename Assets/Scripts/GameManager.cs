using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameSettings settings;
    public GameObject terrainManager;
    public GameObject playerManager;
    public GameObject uiManager;
    public GameObject cameraManager;
    public GameObject buildManager;


    void Start()
    {
        terrainManager.GetComponent<TerrainManager>().Generate();
        playerManager.GetComponent<PlayerManager>().Generate();
    }
}
