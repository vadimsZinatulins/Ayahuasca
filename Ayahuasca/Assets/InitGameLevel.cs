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
            PlayersManager.Instance.ReturnPlayerOne().Spawn(spawn1.position,spawn1.rotation);
            PlayersManager.Instance.ReturnPlayerTwo().Spawn(spawn2.position,spawn2.rotation);
        }
    }
}
