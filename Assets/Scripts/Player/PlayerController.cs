using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;
    private PlayerPhysics playerPhysics;

    private void Awake() 
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerPhysics = GetComponent<PlayerPhysics>();
    }

    void Start()
    {
        Managers.EventManager.Instance.OnSetPlayerPath += SetPath;
        Managers.EventManager.Instance.OnLevelEnd += OnLevelEnd;
        Managers.EventManager.Instance.OnLevelRestart += ResetPlayer;
    }

    private void SetPath(List<Platform> platforms,bool isWinCondition)
    {
        playerMovement.StartMoving(platforms,isWinCondition);
        playerAnimation.Move(true);
    }

    private void OnLevelEnd(bool isSuccess)
    {
        if(isSuccess)
        {
            Dance();
            Managers.EventManager.Instance.ONOnSwitchCameraState(CameraState.Orbital);
        }
        else
        {
            Managers.EventManager.Instance.ONOnSwitchCameraState(CameraState.Free);
        }
    }

    private void Dance()
    {
        playerAnimation.Move(false);
        playerAnimation.Dance(true);
    }

    //Enable physics on level end, would be way better with ragdoll tbh ^^
    public void EnablePhysics()
    {
        playerPhysics.EnablePhysics();
    }

    public void DisablePhysics()
    {
        playerPhysics.DisablePhysics();
    }


    private void ResetPlayer(bool isSuccess)
    {
        playerPhysics.DisablePhysics();
        playerAnimation.Dance(false);
        playerAnimation.Move(false);
        playerMovement.ResetPlayer(isSuccess);
    }
}
