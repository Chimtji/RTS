using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Players/Player")]
public class PlayerAttributes : ScriptableObject
{
    [Header("General")]
    public string id;

    [Header("Spawn")]
    [SerializeField]
    public SpawnLocation spawnLocation;

    [Header("Diplomacy")]
    public string nickname;
    public Color color;
    public int team;
}