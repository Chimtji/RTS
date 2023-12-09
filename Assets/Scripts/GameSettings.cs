using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu()]
public class GameSettings : UpdatableData
{
    [Header("General")]
    [Header("Players")]
    public PlayerAttributes[] players;

};