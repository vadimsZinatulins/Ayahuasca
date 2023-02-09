using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiseaseState", menuName = "Ayahuasca/Disease/DiseaseStateSO", order = 0)]
public class DiseaseStateSO : ScriptableObject {
    public string stateName;
    public float stateTimeInSeconds;
}
