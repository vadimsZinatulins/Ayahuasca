using System;
using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    [SerializeField] bool mouseLockedAtStart = true;

    private void Start()
    {
        if (mouseLockedAtStart)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    [Command("load-level", MonoTargetType.All)]
    public static void LoadLevelCommand(string name)
    {
        SceneManager.LoadScene(name);
        N_Menu.ResetValues();
    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
        N_Menu.ResetValues();
    }

    public void ChangeMenu(N_Menu TargetMenu)
    {
        N_Menu.ChangeMenu(TargetMenu);
    }

    public void TravelBack()
    {
        N_Menu.TravelBack();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
