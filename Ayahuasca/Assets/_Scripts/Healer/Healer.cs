using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Healer : MonoBehaviour, _Scripts.Behaviours.Interfaces.IInteractable {

    [SerializeField] private Transform interactionLocation;
    [SerializeField] private List<CureRecipeSO> listOfCures;

    private bool firstTimeInteraction = true;

    private int curesProduced = 0;

    public void Interact(Transform InInteractorTransform) {
        DiseaseSystem.Instance?.StopTimer();
        
        if(GetComponent<Dialog_Healer>()?.IsTalking ?? false) {
            return;
        }

        if(!firstTimeInteraction) {
            if(InInteractorTransform.TryGetComponent<PlayerInventory>(out PlayerInventory inventory)) {
                var messages = new List<string>();

                listOfCures.ForEach(cure => {
                    if(cure.ContainsIngredients(inventory.CollectedItems)) {
                        messages.Add("It will take me " + (Mathf.Round(cure.timeToBrewInSeconds / 60f)) + " minutes to brew " + cure.name + " cure");
                        int numberOfCures = cure.GetBriewAmount(inventory.CollectedItems);

                        if(cure.isPermanent) {
                            Debug.Log("Cure is permanent");

                            curesProduced += numberOfCures;

                            if(curesProduced > DiseaseSystem.Instance?.GetNumberOfAliveVillagers()) {
                                var diseaseSystem = DiseaseSystem.Instance;

                                if(diseaseSystem) {
                                    diseaseSystem.StopTimer();
                                }
                            }
                        }

                        StartCoroutine(Brew(cure, numberOfCures));
                    }
                });

                if(messages.Count > 0) {
                    messages.Insert(0, "I will start brewing the cures");
                    inventory.UpdateUI();
                } else {
                    messages.Add("You don't have enough ingredients to brew a cure");
                }

                GetComponent<Dialog_Healer>()?.Talk(messages.ToArray());
            }
        } else {
            firstTimeInteraction = false;

            GetComponent<Dialog_Healer>()?.Talk();
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

        if(recipe.isPermanent) {
            DiseaseSystem.Instance?.CureVillagers(amount);
        } else {
            DiseaseSystem.Instance?.RetardSickess(amount, recipe.cureEffectInSeconds);
        }
        
        yield return null;
    }
}
