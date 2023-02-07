using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventListener : MonoBehaviour
{
    [SerializeField] private PlayerBehaviours.PlayerCharacter playerCharacter;

    public void DisableWalk() {
        playerCharacter.WalkingEnabled = false;
    }

    public void EnableWalk() {
        playerCharacter.WalkingEnabled = true;
    }
}
