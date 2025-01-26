using UnityEngine;
using Cinemachine;
using Managers;
public enum CameraState
{
    Orbital,
    Follow,
    Free
}

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera orbitalCamera;
    private CinemachineVirtualCamera followCamera;
    private OrbitalCamera orbitalCameraControl;
    private Transform player;

    private void Awake() 
    {
        CacheComponents();
    }

    void Start()
    {
        EventManager.Instance.OnSwitchCameraState += SwitchCameraState;
        EventManager.Instance.OnLevelRestart += ResetCameraState;
    }

    private void CacheComponents()
    {
        orbitalCamera = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        followCamera = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
        orbitalCameraControl = orbitalCamera.GetComponent<OrbitalCamera>();
        player = followCamera.m_Follow;
    }
    
    private void SwitchCameraState(CameraState state)
    {
        switch (state)
        {
            case CameraState.Orbital:
                SwitchToOrbitalCamera();
                break;
            case CameraState.Follow:
                SwitchToFollowCamera();
                break;
            case CameraState.Free:
                SwitchToFreeCamera();
                break;
        }
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

    // Unfollow the camera target while player is falling so the camera won't fall with player
    private void SwitchToFreeCamera()
    {
        followCamera.m_Follow = null;
    }

    // Assign player again on restart
    private void ResetCameraState(bool isSuccess)
    {
        followCamera.m_Follow = player;
        SwitchToFollowCamera();
    }

}
