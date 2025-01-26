using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera orbitalCamera;
    private CinemachineVirtualCamera followCamera;
    private OrbitalCamera orbitalCameraControl;
    private Transform player;

    private void Awake() 
    {
        orbitalCamera = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        followCamera = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
        orbitalCameraControl = orbitalCamera.GetComponent<OrbitalCamera>();
        player = followCamera.m_Follow;
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
        orbitalCameraControl.Enable();
    } 
    private void SwitchToFollowCamera()
    {
        orbitalCamera.Priority = 0;
        followCamera.Priority = 10;
    }
    private void SwitchToFreeCamera()
    {
        followCamera.m_Follow = null;
    }

    private void OnLevelRestart(bool isSuccess)
    {
        followCamera.m_Follow = player;
        SwitchToFollowCamera();
    }

    // Update is called once per frame
}
