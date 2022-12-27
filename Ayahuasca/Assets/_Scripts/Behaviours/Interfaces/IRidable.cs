using UnityEngine;

namespace _Scripts.Behaviours.Interfaces
{
    public interface IRiddable
    {
        public void StartRiding(GameObject go, ref PlayerRiding ridingType);
        public void StopRiding(GameObject go);

        public string GetRideText();
    }
}