using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] ObjectPool SoloCameraPool;
    private List<Camera> CurrentPoolCameras = new List<Camera>();

    public Camera GetSoloCamera()
    {
       GameObject cameraGO = SoloCameraPool.GetObject();
       Camera camera = cameraGO.GetComponent<Camera>();
       
       Destroy(camera.GetComponent<AudioListener>());
       
       camera.gameObject.SetActive(false);

       CurrentPoolCameras.Add(camera);
       
       return camera;
    }

}
