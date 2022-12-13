using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class N_DoubleToggle : MonoBehaviour
{
    public List<Toggle> Toggles;
    private Toggle CurrentToggle;

    [SerializeField] ToggleGroup ToggleGroup;
    public UnityEvent<Toggle> OnChangeToggle;

    private void Update()
    {
        if (ToggleGroup.GetFirstActiveToggle() != CurrentToggle)
        {
            CurrentToggle = ToggleGroup.GetFirstActiveToggle();
            OnChangeToggle?.Invoke(CurrentToggle);
        }
    }

    public Toggle GetCurrentToggle()
    {
        return CurrentToggle;
    }

    public void SetValue(int v)
    {
        bool foundAnyValue = false;
        foreach (var toggle in Toggles)
        {
            var textra = toggle.GetComponent<N_ToggleExtra>();
            if (textra != null)
            {
                if (textra.value == v)
                {
                    foundAnyValue = true;
                    toggle.Select();
                    break;
                }
            }
        }

        if (!foundAnyValue)
        {
            if (Toggles.Count > 0)
            {
                Toggles[0].Select();
            }
        }
        
    }
}