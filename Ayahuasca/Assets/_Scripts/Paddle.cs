using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {
    
    public Transform Target { get; set; }

    void Update() {
        if(Target != null) {
            transform.LookAt(Target, Vector3.up);
        }
    }
}
