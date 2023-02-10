using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewCureRecipe", menuName = "Ayahuasca/Cure/CureRecipe")]
public class CureRecipeSO : ScriptableObject {

    [Serializable]    
    public class Ingerdient {
        public _Scripts.Behaviours.CollectableData collectableData;
        public int necessaryGrams;
    }
    
    public List<Ingerdient> listOfIngredients;
    public float cureEffectInSeconds = 5.0f;
    public float timeToBrewInSeconds = 2.0f;
    public bool isPermanent = false;

    public bool ContainsIngredients(List<PlayerInventory.CollectedItem> collectedItems) {
        var collectedIngredients = collectedItems.FindAll(item => 
            listOfIngredients.Find(ingredient =>
                item.collectableData == ingredient.collectableData && item.grams >= ingredient.necessaryGrams
            ) != null
        );

        return collectedIngredients.Count >= listOfIngredients.Count;
    }

    public int GetBriewAmount(List<PlayerInventory.CollectedItem> collectedItems) {
        int amount = 0;
        while(ContainsIngredients(collectedItems)) {
            collectedItems.ForEach(item => {
                var matchingIngredient = listOfIngredients.Find(ingredient => item.collectableData == ingredient.collectableData);

                if(matchingIngredient != null) {
                    item.grams -= matchingIngredient.necessaryGrams;
                }
            });

            amount++;
        }

        return amount;
    }
}
