using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Managers;


public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;

    private Quaternion targetRotation;
    private Vector3 targetPosition;

    private List<Platform> path = new List<Platform>();
    private int pathIndex;
    private bool isMoving;
    private bool isGonnaWin;
    private Vector3 platformSize;

    [SerializeField]private float moveSpeed;
    [SerializeField]private float rotationSpeed;

    private void Awake() 
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        EventManager.Instance.OnSendPlatformScaleInfo += SetPlatformSize;
    }

    private void Update() 
    {
        if(!isMoving) return;
        Move();
        CheckStopDistance();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void CheckStopDistance()
    {
        if(Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            pathIndex++;
            if(pathIndex >= path.Count)
            {
                EndReached();
                return;
            }
            ChangeDestination();
        }
    }

    private void ChangeDestination()
    {
        targetPosition = new Vector3(path[pathIndex].transform.position.x, transform.position.y, path[pathIndex].transform.position.z);
        targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
    }

    //set path and start moving along it
    public void StartMoving(List<Platform> platforms, bool isWinCondition)
    {
        isGonnaWin = isWinCondition;
        path = new List<Platform>(platforms);
        if(!isGonnaWin)
        {
            path.RemoveAt(path.Count - 1);
        }
        targetPosition = new Vector3(path[pathIndex].transform.position.x, transform.position.y, path[pathIndex].transform.position.z);
        targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        isMoving = true;
    }

    
    private void EndReached()
    {
        isMoving = false;
        if(isGonnaWin)
        {
            EventManager.Instance.ONOnLevelEnd(true);
        }
        else
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        targetPosition = transform.position + Vector3.forward * platformSize.z;
        targetRotation = transform.rotation;
        transform.DOMove(targetPosition, 0.75f);
        yield return new WaitForSeconds(0.75f);
        EventManager.Instance.ONOnLevelEnd(false);
        playerController.EnablePhysics();
        yield return new WaitForSeconds(2f);
        
        yield return new WaitForSeconds(2f);
        playerController.DisablePhysics();
    }

    public void ResetPlayer(bool isSuccess)
    {
        var target = path[^1].transform.position;
        transform.position = isSuccess ? new Vector3(target.x, transform.position.y, target.z) : Vector3.up * platformSize.y / 2;
        transform.rotation = Quaternion.identity;
        pathIndex = 0;
        // path.Clear();
    }

    // Getting platform size information to place the player properly without using magic numbers on restart
    private void SetPlatformSize(Vector3 scale)
    {
        platformSize = scale;
    }
}
