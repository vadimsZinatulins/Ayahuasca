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
            SubscribeInputEvents();
        }

        //░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
        //                             Subscribing events / Functions                     
        //░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░

        private void SubscribeInputEvents()
        {
            binds.JumpAction.action.performed += OnJump;
            binds.InteractAction.action.performed += OnInteract;
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            if (_character != null)
            {
                _character.OnJump();
            }
        }

        private void OnInteract(InputAction.CallbackContext obj)
        {
            if (_character != null)
            {
                _character.OnInteract();
            }
        }

        public void SetCharacterInput()
        {
            if (_character)
            {
                _character.SetMovementInput(binds.MoveAction.action.ReadValue<Vector2>());
            }
        }

        private void Update()
        {
            SetCharacterInput();
        }

        //░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
        //                             Subscribing events / Functions                     
        //░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░


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

        public PlayerCharacter Spawn(Vector3 spawnPos, Quaternion spawnRot)
        {
            _character = Instantiate(CharacterPrefab, spawnPos, spawnRot);
            _character.SetOwner(this);
            _character.SetCamera();
            return _character;
        }

        public PlayerCharacter Spawn(Vector3 spawnPos, Quaternion spawnRot, PlayerInfo info, PlayerRiding riding)
        {
            _character = Instantiate(CharacterPrefab, spawnPos, spawnRot);
            _character.SetOwner(this);
            _character.SetCamera();

            return _character;
            //Load here the data from the PlayerInfo
        }

        /// <summary>
        /// Despawns the character from the world
        /// </summary>
        /// <returns></returns>
        public bool DeSpawnCharacter()
        {
            //this could be a pool, but to be simple, lets destroy
            if (_character)
            {
                Destroy(_character.gameObject);
                _character = null;
                return true;
            }
            else
            {
                return false;
            }
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