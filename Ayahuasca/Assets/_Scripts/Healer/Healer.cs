using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour, _Scripts.Behaviours.Interfaces.IInteractable {

    [SerializeField] private Transform interactionLocation;
    [SerializeField] private List<CureRecipeSO> listOfCures;

    public void Interact(Transform InInteractorTransform) {
        if(InInteractorTransform.TryGetComponent<PlayerInventory>(out PlayerInventory inventory)) {

            listOfCures.ForEach(cure => {
                if(cure.ContainsIngredients(inventory.CollectedItems)) {
                    int numberOfCures = cure.GetBriewAmount(inventory.CollectedItems);
                    StartCoroutine(Brew(cure, numberOfCures));
                }
            });
        }
    }

    public string GetInteractText() {
        return "Healer";
    }

    public Vector3 GetInteractLocation() {
        return interactionLocation.position;
    }

    private IEnumerator Brew(CureRecipeSO recipe, int amount) {
        yield return new WaitForSeconds(recipe.timeToBrewInSeconds);

        Debug.Log("Brewed " + amount + " " + recipe.name + " cures");

        yield return null;
    }
}
