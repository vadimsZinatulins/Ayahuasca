using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class N_WorldPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactText;
    private Transform lookTarget;

    public void SetTarget(Transform InLookTarget)
    {
        lookTarget = InLookTarget;
    }
    public void SetText(Camera InCamera,Vector3 InPos,string InNewText)
    {
        if (InCamera != null)
        {
            Vector3 screenPos = InCamera.WorldToScreenPoint(InPos);
            interactText.rectTransform.position = screenPos;
        }
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
