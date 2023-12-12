using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameSettings settings;
    public GameObject terrainManager;
    public GameObject playersManager;


    void Start()
    {
        terrainManager.GetComponent<TerrainManager>().Generate();
        playersManager.GetComponent<PlayersManager>().Generate();
    }
}
