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
        private Player _player1, _player2;
        [SerializeField] private Player PlayerPrefab;
        private void Awake()
        {
            _player1 = Instantiate(PlayerPrefab, transform);
            _player2 = Instantiate(PlayerPrefab, transform);
        }
    }
}
