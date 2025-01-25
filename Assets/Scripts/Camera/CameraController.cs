using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera orbitalCamera;
    private CinemachineVirtualCamera followCamera;
    private CinemachineVirtualCamera freeCamera;
    private OrbitalCamera orbitalCameraControl;

    private void Awake() 
    {
        orbitalCamera = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        followCamera = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
        freeCamera = transform.GetChild(2).GetComponent<CinemachineVirtualCamera>();
        orbitalCameraControl = orbitalCamera.GetComponent<OrbitalCamera>();
        Managers.EventManager.Instance.OnLevelRestart += OnLevelRestart;
    }

    void Start()
    {
        Managers.EventManager.Instance.OnSwitchToOrbitalCamera += SwitchToOrbitalCamera;
        Managers.EventManager.Instance.OnSwitchToFollowCamera += SwitchToFollowCamera;
        Managers.EventManager.Instance.OnSwitchToFreeCamera += SwitchToFreeCamera;
    }

    private void SwitchToOrbitalCamera()
    {
        orbitalCamera.Priority = 10;
        followCamera.Priority = 0;
        freeCamera.Priority = 0;
        orbitalCameraControl.Enable();
    } 
    private void SwitchToFollowCamera()
    {
        orbitalCamera.Priority = 0;
        followCamera.Priority = 10;
        freeCamera.Priority = 0;
    }
    private void SwitchToFreeCamera()
    {
        orbitalCamera.Priority = 0;
        followCamera.Priority = 0;
        freeCamera.transform.position = followCamera.transform.position;
        freeCamera.transform.rotation = followCamera.transform.rotation;
        freeCamera.Priority = 10;
    }

    private void OnLevelRestart(bool isSuccess)
    {
        SwitchToFollowCamera();
    }

    // Update is called once per frame
}
