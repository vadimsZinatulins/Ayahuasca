using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatisticsForSystem : MonoBehaviour {
    public static StatisticsForSystem Instance { get; private set; }

    public float StartTime { get; set; }
    public float EndTime { get; set; }
    public int DeathCount { get; set; }
    public int LiveCount { get; set; }

    public bool ShowStatistics { get; set; } = false;

    void Awake() {
        Debug.Log("Awake Called");

        Instance = this;
    }

    void Start() {
        Debug.Log("Start Called");
    }

    public void LoadScene(string scene) {
        ShowStatistics = true;

        SceneManager.LoadScene(scene);
    }

}
