using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour {

    public static InventoryUI Instance { get; private set; }

    [SerializeField] private GameObject centerInventory;
    [SerializeField] private GameObject bottomInventory;

    [SerializeField] private List<InventoryText> InventoryTexts;

    void Awake() {
        Instance = this;

        SetSplitScreen(false);
    }

    public void SetSplitScreen(bool isActive)
    {
        centerInventory.SetActive(isActive);
        bottomInventory.SetActive(!isActive);
    }   

    public void SetValue(string uiName, int amount)
    {
        SetUIValue(centerInventory, uiName, amount);
        SetUIValue(bottomInventory, uiName, amount);
    }
    private void SetUIValue(GameObject go, string uiName, int amount)
    {
        foreach (var inventoryText in InventoryTexts)
        {
            if (inventoryText.title == uiName)
            {
                inventoryText.centerTextMesh.SetText(amount.ToString());
                inventoryText.bottomTextMesh.SetText(amount.ToString());
                break;
            }
        }
    } 
}

[Serializable]
public class InventoryText
{
    public string title;
    public TextMeshProUGUI centerTextMesh;
    public TextMeshProUGUI bottomTextMesh;
}