using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOExtraUtilities : MonoBehaviour
{
    public bool dontDestroyOnLoad=false;
    public static List<string> GO_DOL_Names = new List<string>();
    private void Awake()
    {
        if (dontDestroyOnLoad)
        {
            foreach (var goDol in GO_DOL_Names)
            {
                if (goDol == transform.name)
                {
                    Destroy(gameObject);
                }    
            }
            GO_DOL_Names.Add(transform.name);
            DontDestroyOnLoad(transform);
            Debug.Log($"Added this [{transform.name}] to the Dont Destroy On Load GO!");
        }
    }
}
