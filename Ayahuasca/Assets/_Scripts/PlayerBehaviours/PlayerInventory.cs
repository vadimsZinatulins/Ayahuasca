using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    [System.Serializable]
    public class CollectedItem {
        public _Scripts.Behaviours.CollectableData collectableData;
        public int grams;
    }

    private static List<CollectedItem> collectables;

    public List<CollectedItem> CollectedItems => collectables;

    void Awake() {
        collectables = new List<CollectedItem>();
    }

    public void Add(_Scripts.Behaviours.CollectableData collectableData) {
        CollectedItem ca = collectables.Find(collectable => collectable.collectableData == collectableData);

        int gramsCollected = Random.Range(collectableData.minContainingGrams, collectableData.maxContainingGrams);

        if(ca == null) {
            collectables.Add(new CollectedItem { 
                collectableData = collectableData,
                grams = gramsCollected
            });
        } else {
            ca.grams += gramsCollected;
        }

        UpdateUI();
    }

    public void UpdateUI() {
        collectables.ForEach(collectable => InventoryUI.Instance?.SetValue(collectable.collectableData.herbName, collectable.grams));
    }
}
