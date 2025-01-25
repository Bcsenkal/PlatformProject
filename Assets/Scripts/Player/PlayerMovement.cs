using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;

    private Quaternion targetRotation;
    private Vector3 targetPosition;

    private List<MovingPlatform> path = new List<MovingPlatform>();
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

    public void StartMoving(List<MovingPlatform> platforms, bool isWinCondition)
    {
        isGonnaWin = isWinCondition;
        path = platforms;
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
            Managers.EventManager.Instance.ONOnLevelEnd(true);
        }
        else
        {
            StartCoroutine(Fall());
        }
        pathIndex = 0;
        path.Clear();
    }

    IEnumerator Fall()
    {
        targetPosition = transform.position + Vector3.forward * platformLength;
        targetRotation = transform.rotation;
        transform.DOMove(targetPosition, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Managers.EventManager.Instance.ONOnLevelEnd(false);
        playerController.EnablePhysics();
        yield return new WaitForSeconds(2f);
        
        yield return new WaitForSeconds(2f);
        playerController.DisablePhysics();
    }
}
