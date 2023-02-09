using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour {

    [Serializable]
    private class CollectableAmount {
        public _Scripts.Behaviours.CollectableData collectableData;
        public int amount;
    }

    private List<CollectableAmount> collectables;

    void Awake() {
        collectables = new List<CollectableAmount>();
    }

    public void Add(_Scripts.Behaviours.CollectableData collectableData) {
        CollectableAmount ca = collectables.Find(collectable => collectable.collectableData == collectableData);

        if(ca == null) {
            collectables.Add(new CollectableAmount { 
                collectableData = collectableData,
                amount = 1
            });
        } else {
            ca.amount++;
        }
    }
}
