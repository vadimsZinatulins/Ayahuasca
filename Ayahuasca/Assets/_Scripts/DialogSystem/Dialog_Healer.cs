using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog_Healer : Dialog {
    public void Talk(string[] sentences) {
        this.sentences = sentences;

        Talk();
    }
}
