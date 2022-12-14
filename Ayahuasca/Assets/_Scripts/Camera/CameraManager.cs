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
       CurrentPoolCameras.Add(camera);
       return camera;
    }

}
