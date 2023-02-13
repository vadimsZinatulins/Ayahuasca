using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StatisticsUI : MonoBehaviour {
    [SerializeField] private List<GameObject> childs;

    [SerializeField] private TextMeshProUGUI playTime;
    [SerializeField] private TextMeshProUGUI survivalCount;
    [SerializeField] private TextMeshProUGUI deathCount;

    void Start() {
        var statistics = StatisticsForSystem.Instance;

        var playTimeSpan = TimeSpan.FromSeconds(statistics.EndTime - statistics.StartTime);

        playTime.SetText("Play time: " + string.Format("{0:00}:{1:00}", playTimeSpan.TotalMinutes, playTimeSpan.TotalSeconds));
        survivalCount.SetText("Survived: " + statistics.LiveCount);
        deathCount.SetText("Died: " + statistics.DeathCount);

        childs.ForEach(child => child.SetActive(statistics.ShowStatistics));

        StartCoroutine(DisableWindow());
    }

    IEnumerator DisableWindow() {
        yield return new WaitForSeconds(5);

        childs.ForEach(child => child.SetActive(false));
    }
}
