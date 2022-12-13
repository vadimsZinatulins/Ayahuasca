using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Must be inside of a system instance (dont destroy on load parent)
public class N_SettingsManager : MonoBehaviour
{
    //Game Settings
    public const string MOUSE_STRING = "MouseSens";
    public const string VERTICAL_STRING = "InvertHorizontal";
    public const string HORIZONTAL_STRING = "InvertVertical";
    
    //Video Settings
    public const string VSYNC_STRING = "Vsync";
    public const string QUALITY_STRING = "Quality";
    public const string FPS_STRING = "FPS";
    public const string RESOLUTION_STRING = "Resolution";
    public static List<Resolution> Resolutions { get; private set; }
    //Audio Settings


    private void Awake()
    {
        Resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToList();
#if UNITY_EDITOR
        Resolutions = Screen.resolutions.ToList();
#endif
        //PlayerPrefs.DeleteKey(RESOLUTION_STRING);
    }

    //Load Functions
    
    
    //Update Functions
    public static void UpdateGameSettings()
    {
        
    }
    public static void UpdateVideoSettings()
    {
        QualitySettings.vSyncCount = PlayerPrefs.GetInt(VSYNC_STRING,0);
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(QUALITY_STRING,2));
        Application.targetFrameRate = PlayerPrefs.GetInt(FPS_STRING,60);
        var index = PlayerPrefs.GetInt(RESOLUTION_STRING, Resolutions.Count - 1);
        Screen.SetResolution(Resolutions[index].width,Resolutions[index].height,true,PlayerPrefs.GetInt(FPS_STRING,60));
    }
    public static void UpdateAudioSettings()
    {
        
    }
}
