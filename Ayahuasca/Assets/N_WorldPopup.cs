using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class N_WorldPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactText;
    private Transform lookTarget;

    public void SetTarget(Transform InLookTarget)
    {
        lookTarget = InLookTarget;
    }
    public void SetText(Vector3 InPos,string InNewText)
    {
        transform.position = InPos;
        interactText.text = InNewText;
    }

    private void Update()
    {
        if (lookTarget)
        {
            transform.LookAt(lookTarget);
        }
    }
}
