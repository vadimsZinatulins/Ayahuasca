using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SplitScreenManager : MonoBehaviour
{
    public static SplitScreenManager Instance;
    
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void BindPlayerCamera(Transform player, Camera camera)
    {
        mainCamera.GetComponent<SplitScreenRenderer>().BindPlayerCamera(camera, player);
    }

    public void EnableSystem(bool isEnable)
    {
        mainCamera.GetComponent<SplitScreenRenderer>().EnableSystem(isEnable);
    }

    public bool GetIsSystemEnable()
    {
        return mainCamera.GetComponent<SplitScreenRenderer>().GetIsSystemEnable();
    }
}
