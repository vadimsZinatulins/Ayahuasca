using UnityEngine;

namespace _Scripts.Behaviours
{
    public class Collectable : MonoBehaviour, _Scripts.Behaviours.Interfaces.IInteractable
    {
        [SerializeField] private CollectableData collectableData;
        [SerializeField] private Transform interactionLocation;

        public Vector3 GetInteractLocation()
        {
            return interactionLocation.position;
        }

        public string GetInteractText()
        {
            return collectableData.herbName;
        }

        public void Interact(Transform InInteractorTransform)
        {
            // Trigger "Lifting" if any of childs has Animator Component
            InInteractorTransform.gameObject.GetComponentInChildren<Animator>()?.SetTrigger("Lifting");

            Destroy(this.gameObject);
        }
    }
}