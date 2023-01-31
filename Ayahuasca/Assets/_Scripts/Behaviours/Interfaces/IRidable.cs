using UnityEngine;

namespace _Scripts.Behaviours.Interfaces
{
    public interface IRiddable
    {
        public void StartRiding(GameObject go, ref PlayerRiding ridingType, out RowingSide side);
        public bool StopRiding(GameObject go);

        public string GetRideText();
    }
}