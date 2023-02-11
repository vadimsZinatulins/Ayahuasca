using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_Healer : Dialog {
    public void Talk(string[] sentences) {
        this.sentences = sentences;

        Talk();
    }

    public void Talk() {
        this.index = 0;
        this.showMessage = true;
        
        InventoryUI.Instance?.gameObject.SetActive(false);
        StartCoroutine(Type());
    }
}
