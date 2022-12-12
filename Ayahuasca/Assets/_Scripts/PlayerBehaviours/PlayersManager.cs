using System;
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
        private Player _player1, _player2;
        [SerializeField] private Player PlayerPrefab;

        [Header("Player Binds")] 
        public PlayerBindKeys player1Binds;
        public PlayerBindKeys player2Binds;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Instance = this;
            _player1 = Instantiate(PlayerPrefab, transform);
            _player1.transform.name = "Player1";
            
            _player2 = Instantiate(PlayerPrefab, transform);
            _player2.transform.name = "Player2";
        }

        private void Start()
        {
            _player1.BindInputs(player1Binds);
            _player2.BindInputs(player2Binds);
        }

        public Player ReturnPlayerOne()
        {
            return _player1;
        }
        public Player ReturnPlayerTwo()
        {
            return _player2;
        }
        public void ReturnPlayers(out Player player1, out Player player2)
        {
            player1 = _player1;
            player2 = _player2;
        }
    }
}
