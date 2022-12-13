using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class N_VideoMenu : N_Menu
{
    [SerializeField] private TMP_Dropdown DropResolution;
    [SerializeField] private TMP_Dropdown DropQuality;
    [SerializeField] private TMP_Dropdown DropFPS;
    [SerializeField] private N_DoubleToggle DToggleVsync;
    [SerializeField] private List<int> FpsList;

    //#TODO: Guardar os valores no save file e dar load no inicio
    public void Start()
    {
        //Dar load aos valores depois
        #region FPS

        DropFPS.ClearOptions();
        List<string> fpsStrings = new List<string>();
        foreach (var fps in FpsList)
        {
            fpsStrings.Add(fps.ToString());
        }
        DropFPS.AddOptions(fpsStrings);
        

        DropFPS.SetValueWithoutNotify(FpsList.First
        (
            d=> d == PlayerPrefs.GetInt(N_SettingsManager.FPS_STRING,FpsList[FpsList.Count-1]))
        );
        ChangeFPS(PlayerPrefs.GetInt(N_SettingsManager.FPS_STRING,FpsList[FpsList.Count-1]));
        DropFPS.onValueChanged.AddListener(ChangeFPS);
        #endregion

        #region Quality

        ChangeQualitySettings(PlayerPrefs.GetInt(N_SettingsManager.QUALITY_STRING,3));
        DropQuality.onValueChanged.AddListener(ChangeQualitySettings);

        #endregion
        
        #region Resolution


        if (N_SettingsManager.Resolutions.Count > 0)
        {
            // Set all resolutions
            List<string> ResolutionsStrings = new List<string>();
            foreach (var res in N_SettingsManager.Resolutions)
            {
                ResolutionsStrings.Add($"{res.width} x {res.height}");
            }
            
            DropResolution.ClearOptions();
            DropResolution.AddOptions(ResolutionsStrings);
            
            // Select the current resolution
            int selectedResolution = PlayerPrefs.GetInt(N_SettingsManager.RESOLUTION_STRING, N_SettingsManager.Resolutions.IndexOf(Screen.currentResolution));
            DropResolution.SetValueWithoutNotify(selectedResolution);
            ChangeScreenResolution(selectedResolution);
            DropResolution.onValueChanged.AddListener(ChangeScreenResolution);
        }
        #endregion

        #region Vsync

        //Missing set from player prefs
        DToggleVsync.OnChangeToggle.AddListener(ChangeVsync);

        #endregion
    }

    public void ChangeVsync(Toggle toggle)
    {
        N_ToggleExtra toggleExtra = toggle.GetComponent<N_ToggleExtra>();
        if(toggleExtra)
        {
            PlayerPrefs.SetInt(N_SettingsManager.VSYNC_STRING,toggleExtra.value);
            N_SettingsManager.UpdateVideoSettings();
        }
    }
    public void ChangeQualitySettings(int index)
    {
        if (index != PlayerPrefs.GetInt(N_SettingsManager.QUALITY_STRING,-1))
        {
            PlayerPrefs.SetInt(N_SettingsManager.QUALITY_STRING,index);
            N_SettingsManager.UpdateVideoSettings();
        }
    }
    public void ChangeFPS(int fps)
    {
        if (fps != PlayerPrefs.GetInt(N_SettingsManager.FPS_STRING,-1))
        {
            PlayerPrefs.SetInt(N_SettingsManager.FPS_STRING,fps);
            N_SettingsManager.UpdateVideoSettings();
        }
    }
    public void ChangeScreenResolution(int index)
    {
        PlayerPrefs.SetInt(N_SettingsManager.RESOLUTION_STRING,index);
        N_SettingsManager.UpdateVideoSettings();
    }
}
