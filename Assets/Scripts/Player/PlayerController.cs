using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;

    private void Awake() 
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    void Start()
    {
        Managers.EventManager.Instance.OnSetPlayerPath += SetPath;
    }

    private void SetPath(List<MovingPlatform> platforms)
    {
        playerMovement.StartMoving(platforms);
        playerAnimation.Move(true);
    }
}
