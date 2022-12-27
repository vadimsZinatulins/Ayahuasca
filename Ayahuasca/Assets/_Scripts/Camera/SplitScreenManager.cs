using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    public static SplitScreenManager Instance;
    
    [SerializeField]
    private Vector3 cameraOffset;

    [SerializeField]
    private float cameraSplitDistance;

    private GameObject playerOne;
    private GameObject playerTwo;
    [SerializeField] private Camera mainCamera;
    private Camera cameraPlayerOne;
    private Camera cameraPlayerTwo;
    private GameObject mask;
    private bool isSplitScreenActive;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    void Initialize()
    {
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = Screen.height / 2;
        mainCamera.cullingMask = (1 << LayerMask.NameToLayer("Split Screen")) | (1 << LayerMask.NameToLayer("UI"));

        CreateRenderTexture(Screen.width, Screen.height, cameraPlayerOne);
        CreateRenderTexture(Screen.width, Screen.height, cameraPlayerTwo);

        CreateRenderQuad("Player One Render Quad", 0, cameraPlayerOne.targetTexture);
        CreateRenderQuad("Player Two Render Quad", 1, cameraPlayerTwo.targetTexture);
        CreateMask();
    }

    void Update()
    {
        if(playerOne == null || playerTwo == null) 
        {
            return;
        }
        
        var midPoint = (playerOne.transform.position + playerTwo.transform.position) / 2f;
        var distanceFromMidPoint = Vector3.Distance(midPoint, playerOne.transform.position);

        CheckIfSplitScreenIsRequired(distanceFromMidPoint);

        if(isSplitScreenActive)
        {
            var cameraOffset = (midPoint - playerOne.transform.position).normalized * cameraSplitDistance;

            UpdateCameraPosition(playerOne, cameraPlayerOne, cameraOffset);
            UpdateCameraPosition(playerTwo, cameraPlayerTwo, -cameraOffset);
            
            UpdateMaskPositionAndOrientation();
        } 
        else 
        {
            cameraPlayerOne.transform.position = cameraPlayerTwo.transform.position = midPoint + cameraOffset;
        }
    }

    public void SetupPlayerCamera(GameObject player, Camera camera)
    {
        if(playerOne == null)
        {
            playerOne = player;
            cameraPlayerOne = camera;
        }
        else if(playerTwo == null)
        {
            playerTwo = player;
            cameraPlayerTwo = camera;
        }

        SetupCamera(player, camera);

        if(playerOne != null && playerTwo != null)
        {
            Initialize();
        }
    }

    private void CreateRenderTexture(int width, int height, Camera camera)
    {
        if(camera.targetTexture != null) 
        {
            camera.targetTexture.Release();
        }

        camera.targetTexture = new RenderTexture(width, height, 0, RenderTextureFormat.Default);
    }

    private void SetupCamera(GameObject player, Camera camera)
    {
        camera.cullingMask = ~(1 << LayerMask.NameToLayer("Split Screen"));
        camera.transform.position = player.transform.position + cameraOffset;
        camera.transform.LookAt(player.transform.position, Vector3.up);
    }

    private void CreateRenderQuad(string name, int stencilValue, RenderTexture texture)
    {
        var renderQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        renderQuad.name = name;
        renderQuad.layer = LayerMask.NameToLayer("Split Screen");
        renderQuad.transform.parent = transform;
        renderQuad.transform.localPosition = Vector3.forward * GetComponent<Camera>().nearClipPlane * 2f;
        renderQuad.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        // renderQuad.transform.position = transform.position + transform.forward * GetComponent<Camera>().nearClipPlane * 2f;
        // renderQuad.transform.rotation = transform.rotation;
        
        renderQuad.transform.localScale = new Vector3(Screen.width, Screen.height, 1);

        Destroy(renderQuad.GetComponent<MeshCollider>());

        var renderQuadMaterial = renderQuad.GetComponent<Renderer>().material;
        renderQuadMaterial.shader = Shader.Find("Custom/CustomStencilShader");
        renderQuadMaterial.mainTexture = texture;
        renderQuadMaterial.SetInt("_StencilRef", stencilValue);
    }

    private void CreateMask()
    {
        var stencilValue = 1;

        mask = new GameObject("Mask");
        mask.layer = LayerMask.NameToLayer("Split Screen");
        mask.transform.parent = transform;
        mask.transform.localPosition = Vector3.forward * GetComponent<Camera>().nearClipPlane * 2f;
        mask.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        // mask.transform.position = transform.position + transform.forward * GetComponent<Camera>().nearClipPlane * 2f;
        // mask.transform.rotation = transform.rotation;
        mask.transform.localScale = new Vector3(1.5f, 1f, 1f);
        
        var maskChildQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        maskChildQuad.name = "Stencil Quad";
        maskChildQuad.layer = LayerMask.NameToLayer("Split Screen");
        maskChildQuad.transform.parent = mask.transform;
        maskChildQuad.transform.localPosition = Vector3.forward * GetComponent<Camera>().nearClipPlane * 2f;
        maskChildQuad.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        // maskChildQuad.transform.position = transform.position + transform.forward * GetComponent<Camera>().nearClipPlane * 2f;
        // maskChildQuad.transform.rotation = transform.rotation;
        maskChildQuad.transform.localScale = new Vector3(Screen.width, Screen.height, 1);

        Destroy(maskChildQuad.GetComponent<MeshCollider>());

        var maskChildQuadMaterial = maskChildQuad.GetComponent<Renderer>().material;
        maskChildQuadMaterial.shader = Shader.Find("Custom/OverrideStencil");
        maskChildQuadMaterial.SetInt("_StencilRef", stencilValue);
    }

    private void UpdateCameraPosition(GameObject player, Camera camera, Vector3 offset) 
    {
        camera.transform.position = player.transform.position + offset + cameraOffset;
    }

    private void UpdateMaskPositionAndOrientation()
    {
        var normal = (playerOne.transform.position - playerTwo.transform.position).normalized;
        var angle = Mathf.Atan2(normal.x, normal.z) * Mathf.Rad2Deg;

        mask.transform.localRotation = Quaternion.Euler(-Vector3.forward * angle);

        var shift = mask.transform.up * (float)Screen.height / 2f;
        mask.transform.position = transform.position + shift + transform.forward * GetComponent<Camera>().nearClipPlane * 1.9f;
    }

    private void CheckIfSplitScreenIsRequired(float distanceFromMidPoint)
    {
        if(distanceFromMidPoint >= cameraSplitDistance && !isSplitScreenActive) 
        {
            cameraPlayerTwo.gameObject.SetActive(true);
            mask.SetActive(true);
            isSplitScreenActive = true;
        } 
        else if(distanceFromMidPoint < cameraSplitDistance && isSplitScreenActive) 
        {
            cameraPlayerTwo.gameObject.SetActive(false);
            mask.SetActive(false);
            isSplitScreenActive = false;
        }
    }

    public void EnableSystem(bool isEnable)
    {
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
    }
}
