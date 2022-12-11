using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_GOExtraUtilities : MonoBehaviour
{
    public bool dontDestroyOnLoad=false;
    public static List<string> GO_DOL_Names = new List<string>();
    private void Awake()
    {
        if (dontDestroyOnLoad && !GO_DOL_Names.Contains(transform.name))
        {
            GO_DOL_Names.Add(transform.name);
            DontDestroyOnLoad(transform);
            Debug.Log($"Added this [{transform.name}] to the Dont Destroy On Load GO!");
        }
    }
}
