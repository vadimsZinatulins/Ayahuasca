using UnityEngine;

namespace _Scripts.Behaviours
{
    [CreateAssetMenu(fileName = "NewCollectableData", menuName = "Ayahuasca/Collectables/CollectableData", order = 0)]
    public class CollectableData : ScriptableObject
    {
        public string herbName;
        public Sprite Icon;
    }
}