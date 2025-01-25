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
    private float platformLength;

    [SerializeField]private float moveSpeed;
    [SerializeField]private float rotationSpeed;

    private void Awake() 
    {
        playerController = GetComponent<PlayerController>();
    }
    

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            EventManager.Instance.ONOnLevelRestart(false);
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            EventManager.Instance.ONOnLevelRestart(true);
        }
        if(!isMoving) return;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        if(Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            pathIndex++;
            if(pathIndex >= path.Count)
            {
                EndReached();
                return;
            }
            targetPosition = new Vector3(path[pathIndex].transform.position.x, transform.position.y, path[pathIndex].transform.position.z);
            targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        }
    }

    public void StartMoving(List<Platform> platforms, bool isWinCondition)
    {
        isGonnaWin = isWinCondition;
        path = new List<Platform>(platforms);
        if(!isGonnaWin)
        {
            path.RemoveAt(path.Count - 1);
        }
        platformLength = path[0].transform.localScale.z;
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
        targetPosition = transform.position + Vector3.forward * platformLength;
        targetRotation = transform.rotation;
        transform.DOMove(targetPosition, 0.5f);
        yield return new WaitForSeconds(0.5f);
        EventManager.Instance.ONOnLevelEnd(false);
        playerController.EnablePhysics();
        yield return new WaitForSeconds(2f);
        
        yield return new WaitForSeconds(2f);
        playerController.DisablePhysics();
    }

    public void ResetPlayer(bool isSuccess)
    {
        var target = path[^1].transform.position;
        transform.position = new Vector3(target.x, transform.position.y, target.z);
        transform.rotation = Quaternion.identity;
        pathIndex = 0;
        // path.Clear();
    }
}
