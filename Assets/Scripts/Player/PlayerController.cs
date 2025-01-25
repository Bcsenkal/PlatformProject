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
        Managers.EventManager.Instance.OnLevelRestart += OnLevelRestart;
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
            playerAnimation.Move(false);
            playerAnimation.Dance(true);
            Managers.EventManager.Instance.ONOnSwitchToOrbitalCamera();
        }
        else
        {
            Managers.EventManager.Instance.ONOnSwitchToFreeCamera();
        }
    }

    public void EnablePhysics()
    {
        playerPhysics.EnablePhysics();
    }

    public void DisablePhysics()
    {
        playerPhysics.DisablePhysics();
    }

    private void OnLevelRestart(bool isSuccess)
    {
        playerPhysics.DisablePhysics();
        playerAnimation.Dance(false);
        playerAnimation.Move(false);
        playerMovement.ResetPlayer(isSuccess);
    }
}
