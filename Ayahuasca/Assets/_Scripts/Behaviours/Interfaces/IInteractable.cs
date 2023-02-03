using UnityEngine;

namespace _Scripts.Behaviours.Interfaces
{
    public interface IInteractable
    {
        void Interact(Transform InInteractorTransform);
        string GetInteractText();
        Vector3 GetInteractLocation();
    }
}