using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu()]
public class GameSettings : UpdatableData
{
    [Header("General")]

    [Range(2, 8)]
    public int amountOfPlayers;
    public TerrainSettings mapSettings;

    [Header("Player")]
    public int playerNumber;
    public Player player;

};