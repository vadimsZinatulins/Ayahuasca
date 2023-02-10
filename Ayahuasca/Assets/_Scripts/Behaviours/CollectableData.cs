using UnityEngine;

namespace _Scripts.Behaviours
{
    [CreateAssetMenu(fileName = "NewCollectableData", menuName = "Ayahuasca/Collectables/CollectableData", order = 0)]
    public class CollectableData : ScriptableObject
    {
        public string herbName;
        public Sprite Icon;

        public int minContainingGrams = 1;
        public int maxContainingGrams = 5;
    }
}