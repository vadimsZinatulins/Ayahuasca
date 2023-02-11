using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour {

    public static InventoryUI Instance { get; private set; }

    [SerializeField] private GameObject centerInventory;
    [SerializeField] private GameObject bottomInventory;

    void Awake() {
        Instance = this;

        SetSplitScreen(false);
    }

    public void SetSplitScreen(bool isActive) {
        centerInventory.SetActive(isActive);
        bottomInventory.SetActive(!isActive);
    }   

    public void SetValue(string herb, int amount) {
        SetHerbValue(centerInventory, herb, amount);
        SetHerbValue(bottomInventory, herb, amount);
    }
    private void SetHerbValue(GameObject go, string herb, int amount) {
        go.transform.Find(herb + " Text")?.GetComponent<TextMeshProUGUI>()?.SetText(herb + ": " + amount);
    } 
}
