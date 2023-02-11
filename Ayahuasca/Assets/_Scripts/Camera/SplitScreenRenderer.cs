using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenRenderer : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float splitDistance;

    [SerializeField] private Material material;
    
    private Transform playerOne;
    private Camera cameraPlayerOne;

    private Transform playerTwo;
    private Camera cameraPlayerTwo;

    [SerializeField] bool isSplitScreenActive;

    private int cachedScreenWidth;
    private int cachedScreenHeight;

    public Material Material => material;
    public bool IsSplitScreenActive => isSplitScreenActive;
    void Start()
    {
        material = new Material(Shader.Find("Custom/StencilGeom"));
    }

    void LateUpdate()
    {
        if(!playerOne || !playerTwo)
        {
            return;
        }

        if(cachedScreenWidth != Screen.width || cachedScreenHeight != Screen.height)
        {
            UpdateResolution(Screen.width, Screen.height);
        }

        var midPoint = (playerOne.transform.position + playerTwo.transform.position) / 2f;
        var playerPosDif = (playerOne.transform.position - playerTwo.transform.position).normalized;

        transform.position = midPoint + offset;
        transform.LookAt(midPoint);

        cameraPlayerOne.transform.position = playerOne.transform.position - playerPosDif * splitDistance / 2 + offset;
        cameraPlayerTwo.transform.position = playerTwo.transform.position + playerPosDif * splitDistance / 2 + offset;

        cameraPlayerOne.transform.rotation = cameraPlayerTwo.transform.rotation = transform.rotation;

        UpdateSplitScreenState();
        if(isSplitScreenActive)
        {
            var localSpace = transform.InverseTransformVector(playerPosDif);
            material.SetFloat("_CutDirectionX", localSpace.x);
            material.SetFloat("_CutDirectionY", localSpace.z);
        }
    }

    public void BindPlayerCamera(Camera camera, Transform player)
    {
        if(!playerOne)
        {
            cameraPlayerOne = camera;
            playerOne = player;
        }
        else if(!playerTwo)
        {
            cameraPlayerTwo = camera;
            playerTwo = player;
        }

        if(playerOne && playerTwo)
        {
            UpdateResolution(Screen.width, Screen.height);
        }
    }

    public void UpdateResolution(int width, int height)
    {
        cachedScreenWidth = width;
        cachedScreenHeight = height;

        SetupCamera(width, height, cameraPlayerOne, "_RenderTextureCamOne");
        SetupCamera(width, height, cameraPlayerTwo, "_RenderTextureCamTwo");
    }

    private void SetupCamera(int width, int height, Camera camera, string textureTarget)
    {
        if(camera.targetTexture) 
        {
            camera.targetTexture.Release();
        }

        camera.targetTexture = new RenderTexture(width, height, 0, RenderTextureFormat.Default);
        material.SetTexture(textureTarget, camera.targetTexture);
    }

    private void UpdateSplitScreenState()
    {
        var distance = Vector3.Distance(playerOne.transform.position, playerTwo.transform.position);
        if(distance > splitDistance && !isSplitScreenActive)
        {
            cameraPlayerOne.gameObject.SetActive(true);
            cameraPlayerTwo.gameObject.SetActive(true);
            isSplitScreenActive = true;

            InventoryUI.Instance?.SetSplitScreen(isSplitScreenActive);
        } 
        else if(distance < splitDistance && isSplitScreenActive)
        {
            cameraPlayerOne.gameObject.SetActive(false);
            cameraPlayerTwo.gameObject.SetActive(false);
            isSplitScreenActive = false;
            
            InventoryUI.Instance?.SetSplitScreen(isSplitScreenActive);
        }
    }

    public void EnableSystem(bool isEnable)
    {
        this.enabled = isEnable;
        cameraPlayerOne.gameObject.SetActive(isEnable);
        cameraPlayerTwo.gameObject.SetActive(isEnable);
    }

    public bool GetIsSystemEnable()
    {
        return this.enabled && cameraPlayerOne.gameObject.activeSelf && cameraPlayerTwo.gameObject.activeSelf;
    }
}
