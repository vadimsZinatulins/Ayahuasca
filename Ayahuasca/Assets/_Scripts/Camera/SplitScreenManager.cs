using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        /*
        if (mainCamera)
        {
            mainCamera.gameObject.SetActive(isEnable);
        }
        if (cameraPlayerOne)
        {
            cameraPlayerOne.gameObject.SetActive(isEnable);
        }
        if (cameraPlayerTwo)
        {
            cameraPlayerTwo.gameObject.SetActive(isEnable);
        }
        gameObject.SetActive(isEnable);
        */
    }
}
