using System;
using System.Collections;
using System.Collections.Generic;
using PlayerBehaviours;
using UnityEngine;

public class InitGameLevel : MonoBehaviour
{
    [SerializeField] private Transform spawn1, spawn2;
    private void Start()
    {
        if (PlayersManager.Instance)
        {
            // Spawns each player at certain positions
            PlayersManager.Instance.ReturnPlayer(0).Spawn(spawn1.position,spawn1.rotation);
            PlayersManager.Instance.ReturnPlayer(1).Spawn(spawn2.position,spawn2.rotation);
        }
    }
}
