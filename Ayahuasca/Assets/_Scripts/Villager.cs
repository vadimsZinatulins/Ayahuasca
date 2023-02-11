using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour, _Scripts.Behaviours.Interfaces.IInteractable {

    [SerializeField] private Transform interactionTextLocation;

    public void Interact(Transform InInteractorTransform) {
        Debug.Log("Interact!");
        if(TryGetComponent<Dialog>(out Dialog dialog)) {
            dialog.Talk();
        }
    }

    public string GetInteractText() {
        return "Villager";
    }

    public Vector3 GetInteractLocation() {
        return interactionTextLocation.position;
    }
}
