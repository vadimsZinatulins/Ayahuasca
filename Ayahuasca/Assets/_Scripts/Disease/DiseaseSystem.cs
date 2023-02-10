using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiseaseSystem : MonoBehaviour {
    [Serializable] public class Disease {
        public DiseaseStateSO diseaseState;
        public int initialNumberOfVillagers;
    }

    [Serializable] public class SickVillager {
        // public DiseaseStateSO state;

        public float timeToDie;
    }

    public static DiseaseSystem Instance { get; private set; }

    [SerializeField] private int initialNumberOfHealthyVillagers = 90;
    [SerializeField] private int numberOfNewInfected = 1;
    [SerializeField] private float timeUntilNewInfected = 60;

    [SerializeField] private List<Disease> listOfDiseases;

    private List<SickVillager> listOfSickVillagers;
    private List<float> sicknessRedardants;

    private float timeSinceLastInfection = 0f;
    private int currentNumberOfVillagers;
    private int curedVillagers;

    private float timeToDie;

    void Awake() {
        if(Instance != null) {
            Debug.LogError("There are two Disease Systems...");
        }

        Instance = this;

        listOfSickVillagers = new List<SickVillager>();
        sicknessRedardants = new List<float>();

        listOfDiseases.ForEach(disease => AddSickVillagers(disease.initialNumberOfVillagers, disease.diseaseState));

        timeToDie = 0;
        listOfDiseases.ForEach(disease => timeToDie += disease.diseaseState.stateTimeInSeconds);

        curedVillagers = 0;
        currentNumberOfVillagers = initialNumberOfHealthyVillagers;
    }

    void Update() {
        timeSinceLastInfection += Time.deltaTime;

        if(timeSinceLastInfection > timeUntilNewInfected) {
            int numOfNewInfected = Mathf.Min(numberOfNewInfected, currentNumberOfVillagers);
            currentNumberOfVillagers -= numOfNewInfected;

            AddSickVillagers(numOfNewInfected, listOfDiseases[0].diseaseState);
            ApplyStoredRetardants();

            timeSinceLastInfection = 0f;
        }

        listOfSickVillagers.ForEach(sickVillager => sickVillager.timeToDie -= Time.deltaTime);
        listOfSickVillagers.RemoveAll(sickVillager => sickVillager.timeToDie <= 0f);
    }

    private bool TryGetDiseaseInterval(string diseaseName, out float minTime, out float maxTime) {
        minTime = maxTime = 0f;

        var desiredDisease = listOfDiseases.Find(disease => disease.diseaseState.stateName == diseaseName);
        if(desiredDisease == null) {
            return false;
        }
        
        float accumulatedTime = 0;
        var diseaseIndex = listOfDiseases.IndexOf(desiredDisease);
        listOfDiseases.Take(diseaseIndex).ToList().ForEach(disease => accumulatedTime += disease.diseaseState.stateTimeInSeconds);

        minTime = accumulatedTime;
        maxTime = accumulatedTime + desiredDisease.diseaseState.stateTimeInSeconds;
        return true;
    }

    private void AddSickVillagers(int numberOfVillagers, DiseaseStateSO diseaseState) {
        if(TryGetDiseaseInterval(diseaseState.stateName, out float minTime, out float maxTime)) {
            for (int i = 0; i < numberOfVillagers; i++) {
                listOfSickVillagers.Add(new SickVillager { timeToDie = maxTime });
            }
        }
    }

    // Use this method to retrieve the number of sick villagers in a specific state
    public int GetNumberOfPeopleInState(string stateName) {
        if(TryGetDiseaseInterval(stateName, out float minTime, out float maxTime)) {
            return listOfSickVillagers.FindAll(sickVillager => sickVillager.timeToDie >= minTime && sickVillager.timeToDie <= maxTime).Count;
        }
        return 0;
    }

    // Use this method to retrieve the number of dead villagers
    public int GetNumberOfDeadVillagers() {
        int totalNumberOfVillagers = initialNumberOfHealthyVillagers;
        listOfDiseases.ForEach(disease => totalNumberOfVillagers += disease.initialNumberOfVillagers);

        int numberOfAliveVillagers = currentNumberOfVillagers + listOfSickVillagers.Count;

        return totalNumberOfVillagers - numberOfAliveVillagers;
    }

    // Use this method to cure villagers. The argument specifies the amount of villagers to cure
    public void CureVillagers(int amount) {
        var curedVilagersList = listOfSickVillagers
            .OrderBy(sickVillager => sickVillager.timeToDie)
            .Take(amount)
            .ToList();

        curedVillagers += curedVilagersList.Count;

        curedVilagersList.ForEach(curedVillager => listOfSickVillagers.Remove(curedVillager));
    }

    // Use this method to retard sickess. The argument specifies the amount of villagers that will retard the sickness and by which time
    public void RetardSickess(int amount, float time) {
        int halfOfRetardants = amount / 2;
        int numToConsume = Math.Min(listOfSickVillagers.Count, halfOfRetardants);
        int numToStore = amount - numToConsume;

        // Apply half of the retardants
        listOfSickVillagers
            .OrderBy(sickVillager => sickVillager.timeToDie)
            .Take(numToConsume)
            .ToList()
            .ForEach(sickVillager => sickVillager.timeToDie += time);

        // Store the remainning retardants
        sicknessRedardants.AddRange(Enumerable.Range(0, numToStore).Select(x => time).ToList());
    }

    private void ApplyStoredRetardants() {
        int numRetardantsToApply = Math.Min(listOfSickVillagers.Count, sicknessRedardants.Count);

        // Apply half of the retardants
        int index = 0;
        listOfSickVillagers
            .OrderBy(sickVillager => sickVillager.timeToDie)
            .Take(numRetardantsToApply)
            .ToList()
            .ForEach(sickVillager => sickVillager.timeToDie += sicknessRedardants[index++]);

        // Remove the used retardants
        int remainingRetardants = sicknessRedardants.Count - numRetardantsToApply;
        sicknessRedardants = sicknessRedardants.TakeLast(remainingRetardants).ToList();
    }
}
