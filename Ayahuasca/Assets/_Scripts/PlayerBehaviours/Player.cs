using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerBehaviours
{
    /// <summary>
    /// This class is gonna hold the player controller instance. This will save this player stats, the character they are controlling
    /// and other things relative to just one player.
    /// </summary>
    public class Player : MonoBehaviour
    {
        public PlayerCharacter CharacterPrefab;
        private PlayerCharacter _character;
        private PlayerInfo _playerInfo;
        private PlayerBindKeys binds;

        public void BindInputs(PlayerBindKeys bind)
        {
            binds = bind;
        }

        private void SetInputsToCharacter()
        {
            _character.SetInputs(binds.MoveAction.action.ReadValue<Vector2>());
        }

        private void Update()
        {
            SetInputsToCharacter();
        }
        //░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
        //                                Utility                                  
        //░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░

        /// <summary>
        /// ATENTION: If possible, use the one with player info. Player Info is important
        /// </summary>
        public PlayerCharacter Spawn()
        {
            return Spawn(Vector3.zero);
        }
        public PlayerCharacter Spawn(Vector3 spawnPos)
        {
            return Spawn(spawnPos, Quaternion.identity);
        }
        public PlayerCharacter Spawn(Vector3 spawnPos,Quaternion spawnRot)
        {
            _character = Instantiate(CharacterPrefab, spawnPos, spawnRot);
            _character.SetOwner(this);
            _character.SetCamera();
            return _character;
        }
        public PlayerCharacter Spawn(Vector3 spawnPos, Quaternion spawnRot,PlayerInfo info)
        {
            _character = Instantiate(CharacterPrefab, spawnPos, spawnRot);
            _character.SetOwner(this);
            _character.SetCamera();

            return _character;
            //Load here the data from the PlayerInfo
        }

        //░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
        //                                Utility                                  
        //░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
        
    }

    /// <summary>
    /// Stores the unique data from each player, maybe its not necessary but just in case there's save files for each character
    /// </summary>
    public struct PlayerInfo
    {
        
    }
}