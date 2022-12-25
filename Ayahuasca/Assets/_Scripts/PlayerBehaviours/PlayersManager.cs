using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBehaviours
{
    /// <summary>
    /// This Manager will have reference to both players controllers. The main use of this class
    /// is to be able to manage saving data, manage control input assignments and other generic things.
    /// </summary>
    public class PlayersManager : MonoBehaviour
    {
        public static PlayersManager Instance;
        public CameraManager CameraManager;
        [SerializeField] private List<PlayerInstanceData> PlayerData;
        private List<Player> _players = new List<Player>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;

            //Spawns the player's. This are not the characters, only the controller instances
            foreach (var pData in PlayerData)
            {
                var _player = Instantiate(pData.Prefab, transform);
                _player.transform.name = pData.PlayerName;
                if (_player.TryGetComponent(out Player player))
                {
                    player.BindInputs(pData.Binds);
                    _players.Add(player);
                }
            }
        }
        
        public Player ReturnPlayer(int index)
        {
            return _players[index];
        }
        
        public Player ReturnPlayer(string playerName)
        {
            return _players.Find(d => d.transform.name == playerName);
        }

        public List<Player> ReturnPlayers()
        {
            return _players;
        }

        public int GetPlayerIndex(Player player)
        {
            return _players.IndexOf(player);
        }
    }

    [Serializable]
    public struct PlayerInstanceData
    {
        public string PlayerName;
        public GameObject Prefab;
        public PlayerBindKeys Binds;
    }
}