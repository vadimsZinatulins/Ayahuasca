using QFSW.QC;
using UnityEngine;

namespace DefaultNamespace.Utility
{
    public static class UtilityCommands
    {
        [Command("utility_SetTimeScale", MonoTargetType.Single)]
        public static void SetTimeScale(float value)
        {
            Time.timeScale = value;
        }
        [Command("utility_SetFpsTarget", MonoTargetType.Single)]
        public static void SetFpsMax(int value)
        {
            Application.targetFrameRate = value;
        }
        
    }
}