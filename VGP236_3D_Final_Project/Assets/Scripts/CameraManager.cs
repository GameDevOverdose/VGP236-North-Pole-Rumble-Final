using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance = null;
    public static CameraManager Instance { get { return _instance; } }

    private  CinemachineCamera[] cinemachineCamerasInScene;

    //private int activatedPriority = 1;
    //private int deactivatedPriority = 0;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        cinemachineCamerasInScene = GameObject.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None)
                                    .OrderBy(c => c.transform.GetSiblingIndex()).ToArray();

        for (int i = 0; i < cinemachineCamerasInScene.Length; i++)
        {
            if(i == 0)
            {
                cinemachineCamerasInScene[i].gameObject.SetActive(true);
            }
            else
            {
                cinemachineCamerasInScene[i].gameObject.SetActive(false);
            }
        }
    }

    public void ActivateCamera(CinemachineCamera cameraToActivate)
    {
        foreach (CinemachineCamera cam in cinemachineCamerasInScene)
        {
            if (cam == cameraToActivate)
            {
                cam.gameObject.SetActive(true);
            }
            else
            {
                cam.gameObject.SetActive(false);
            }
        }
    }
}
