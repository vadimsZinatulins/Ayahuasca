using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MultiTrigger : TriggerAction
{
    public string nextScene;
    
    public void ChangeScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
