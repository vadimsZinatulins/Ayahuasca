using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiseaseSystem : MonoBehaviour {
    [Serializable] public struct Disease {
        public DiseaseStateSO diseaseState;
        public int initialNumberOfVillagers;
    }

    public class SickVillager {
        public DiseaseStateSO state;

        private float currentDiseaseTime;
        
        public bool IsAlive => state != null;

        public void Update(List<Disease> listOfDiseases) {
            if(IsAlive) {
                currentDiseaseTime += Time.deltaTime;

                if(currentDiseaseTime > state.stateTimeInSeconds) {
                    state = GetNextDiseaseState(listOfDiseases);
                    currentDiseaseTime = 0;
                }
            }
        }

        private DiseaseStateSO GetNextDiseaseState(List<Disease> listOfDiseases) {
            var currentDiseaseStateIndex = listOfDiseases.FindIndex(disease => disease.diseaseState == state);
            
            if(currentDiseaseStateIndex + 1 >= listOfDiseases.Count) {
                return null;
            }
   
            return listOfDiseases[currentDiseaseStateIndex + 1].diseaseState;
        }
    }

    [SerializeField] private int initialNumberOfHealthyVillagers = 90;
    [SerializeField] private int numberOfNewInfected = 1;
    [SerializeField] private float timeUntilNewInfected = 60;

    [SerializeField] private List<Disease> listOfDiseases;

    private List<SickVillager> listOfSickVillagers;
    private float timeSinceLastInfection = 0f;
    private int currentNumberOfVillagers;

    void Start() {
        listOfSickVillagers = new List<SickVillager>();

        currentNumberOfVillagers = initialNumberOfHealthyVillagers;

        listOfDiseases.ForEach(disease => AddSickVillagers(disease.initialNumberOfVillagers, disease.diseaseState));
    }

    void Update() {
        timeSinceLastInfection += Time.deltaTime;

        if(timeSinceLastInfection > timeUntilNewInfected) {
            int numOfNewInfected = Mathf.Min(numberOfNewInfected, currentNumberOfVillagers);
            currentNumberOfVillagers -= numOfNewInfected;

            AddSickVillagers(numOfNewInfected, listOfDiseases[0].diseaseState);

            timeSinceLastInfection = 0f;
        }

        listOfSickVillagers.ForEach(sickVillager => sickVillager.Update(listOfDiseases));
        listOfSickVillagers.RemoveAll(sickVillager => !sickVillager.IsAlive);
    }

    private void AddSickVillagers(int numberOfVillagers, DiseaseStateSO diseaseState) {
        for (int i = 0; i < numberOfVillagers; i++) {
            listOfSickVillagers.Add(new SickVillager { state = diseaseState });
        }
    }

    // Use this method to retrieve the number of sick villagers in a specific state
    public int GetNumberOfPeopleInState(string stateName) {
        return listOfSickVillagers.FindAll(villager => villager.state.stateName == stateName).Count;
    }

    // Use this method to retrieve the number of dead villagers
    public int GetNumberOfDeadVillagers() {
        int totalNumberOfVillagers = initialNumberOfHealthyVillagers;
        listOfDiseases.ForEach(disease => totalNumberOfVillagers += disease.initialNumberOfVillagers);

        int numberOfAliveVillagers = currentNumberOfVillagers + listOfSickVillagers.Count;

        return totalNumberOfVillagers - numberOfAliveVillagers;
    }
}
