using UnityEngine;

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
        
        /// <summary>
        /// ATENTION: If possible, use one of the other ones. Player Info is important
        /// </summary>
        public void Spawn()
        {
            _character = Instantiate(CharacterPrefab, Vector3.zero, Quaternion.identity);
        }
        
        public void Spawn( PlayerInfo info)
        {
            _character = Instantiate(CharacterPrefab, Vector3.zero, Quaternion.identity);
            //Load here the data from the PlayerInfo
        }
        
        public void Spawn( PlayerInfo info, Vector3 spawnPos)
        {
            _character = Instantiate(CharacterPrefab, spawnPos, Quaternion.identity);
            //Load here the data from the PlayerInfo
        }
        
        public void Spawn( PlayerInfo info, Vector3 spawnPos, Quaternion spawnRot)
        {
            _character = Instantiate(CharacterPrefab, spawnPos, spawnRot);
            //Load here the data from the PlayerInfo
        }
    }

    /// <summary>
    /// Stores the unique data from each player, maybe its not necessary but just in case there's save files for each character
    /// </summary>
    public struct PlayerInfo
    {
        
    }
}