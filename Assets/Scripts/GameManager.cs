using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerAttributes[] playersAttributes;
    void Start()
    {
        foreach (PlayerAttributes playerAttributes in playersAttributes)
        {
            Player player = new Player(playerAttributes);
        }
    }
}
