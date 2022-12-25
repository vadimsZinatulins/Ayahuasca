using UnityEngine;

namespace _Scripts.Behaviours.Interfaces
{
    public interface IRiddable
    {
        public PlayerRiding StartRiding(GameObject go);
        public void StopRiding(GameObject go);

        public string GetRideText();
    }
}