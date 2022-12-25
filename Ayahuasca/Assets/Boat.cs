using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Behaviours.Interfaces;
using PlayerBehaviours;
using Sirenix.OdinInspector;
using UnityEngine;

public class Boat : MonoBehaviour, IRiddable
{
    public Seat frontSeat, backSeat;

    public PlayerRiding StartRiding(GameObject go)
    {
        SetSeated(go.transform);
        return PlayerRiding.BOAT;
    }

    public void StopRiding(GameObject go)
    {
        RemoveFromBoat(go.transform);
    }

    public string GetRideText()
    {
        return "Ride boat";
    }

    private void SetSeated(Transform InTransform)
    {
        if (InTransform.TryGetComponent(out Player player))
        {
            int playerIndex = PlayersManager.Instance.GetPlayerIndex(player);
            // Isto vai ficar materlado, mas depois se lembrar de uma forma melhor de obrigar um jogador a ficar neste
            // muda-se
            switch (playerIndex)
            {
                case 0:
                    if (backSeat.currentObject == null)
                    {
                        backSeat.currentObject = player.gameObject;
                        player.transform.parent = backSeat.positionTransform;
                        player.transform.position = backSeat.positionTransform.position;
                        player.transform.rotation = backSeat.positionTransform.rotation;
                        player.GetComponent<CharacterController>().enabled = false;
                    }
                    else
                    {
                        Debug.LogError("Already occupied");
                        //RemoveFromBoat(backSeat);
                    }
                    break;
                    
                case 1:
                    if (frontSeat.currentObject == null)
                    {
                        frontSeat.currentObject = player.gameObject;
                        player.transform.parent = frontSeat.positionTransform;
                        player.transform.position = frontSeat.positionTransform.position;
                        player.transform.rotation = frontSeat.positionTransform.rotation;
                        player.GetComponent<CharacterController>().enabled = false;
                    }
                    else
                    {
                        Debug.LogError("Already occupied");
                        //RemoveFromBoat(backSeat);
                    }
                    break;
            }
        }
    }

    public void RemoveFromBoat(Transform player)
    {
        if (frontSeat.currentObject == player.transform)
        {
            RemoveFromBoat(frontSeat);
        }else if (backSeat.currentObject == player.transform)
        {
            RemoveFromBoat(backSeat);
        }
    }
    public void RemoveFromBoat(Seat seat)
    {
        GameObject go = seat.currentObject;
        // Change player animation state
        go.transform.parent = null;
        seat.currentObject = null;
    }
}

[Serializable]
public struct Seat
{
    public Transform positionTransform;
    public RowingSide seatLocation;
    [ReadOnly] public GameObject currentObject;
}

public enum RowingSide
{
    NONE,
    LEFT,
    RIGHT
}