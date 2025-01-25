using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Quaternion targetRotation;
    private Vector3 targetPosition;

    private List<MovingPlatform> path = new List<MovingPlatform>();
    private int pathIndex;
    private bool isMoving;

    [SerializeField]private float moveSpeed;
    [SerializeField]private float rotationSpeed;
    

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
            targetRotation = path[pathIndex].transform.rotation;
        }
    }

    public void StartMoving(List<MovingPlatform> platforms)
    {
        path = platforms;
        targetPosition = new Vector3(path[pathIndex].transform.position.x, transform.position.y, path[pathIndex].transform.position.z);
        isMoving = true;
    }

    private void EndReached()
    {
        isMoving = false;
        pathIndex = 0;
        path.Clear();

    }
}
