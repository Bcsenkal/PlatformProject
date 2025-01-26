using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class PlatformMovement : MonoBehaviour
{
    private const float MIN_MOVE_TIME = 0.4f;
    private const float MOVE_TIME_REDUCTION_PER_PLATFORM = 0.025f;
    private Platform platform;
    //Smooth Movement
    private float elapsedTime;
    [SerializeField] private float moveTime;
    private float currentMoveTime;
    
    private float targetX;
    private float startX;

    private bool isMoving;
    private Platform previousPlatform;
    public void CacheComponents()
    {
        currentMoveTime = moveTime;
        platform = GetComponent<Platform>();
    }

    //smoothly move to left and right with easing function
    private void Update()
    {
        if(!isMoving) return;
        elapsedTime += Time.deltaTime;
        float newX = Easings.QuadEaseInOut(elapsedTime, startX, targetX - startX, currentMoveTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        if(elapsedTime >= currentMoveTime)
        {
            elapsedTime = 0;
            startX = targetX;
            targetX = 0 -targetX;
        }
    }

    //Start moving and subscrive to mouse down(touch) event
    public void StartMoving(Platform previous, float platformScale, int spawnedPlatforms)
    {
        CalculateMoveTime(spawnedPlatforms);
        previousPlatform = previous;
        EventManager.Instance.ONOnChangeNextSpawn(transform.position.z + platform.InitialScale.z);
        EventManager.Instance.OnMouseDown += StopMoving;
        startX = transform.position.x;
        targetX = transform.position.x < 0 ? platformScale : -platformScale;
        isMoving = true;
    }

    private float CalculateMoveTime(int spawnedPlatforms)
    {
        currentMoveTime = moveTime - spawnedPlatforms * MOVE_TIME_REDUCTION_PER_PLATFORM;
        currentMoveTime = Mathf.Clamp(currentMoveTime, MIN_MOVE_TIME, currentMoveTime);
        return currentMoveTime;
    }

    //Stop Moving On Mouse(touch) Input and unsunscribe from event
    private void StopMoving(Vector2 args)
    {
        if(!isMoving) return;
        isMoving = false;
        currentMoveTime = moveTime;
        EventManager.Instance.OnMouseDown -= StopMoving;
        SplitPlatform();
    }

    private void SplitPlatform()
    {
        var distance = transform.position.x - previousPlatform.transform.position.x;
        platform.SplitPlatform(distance);
    }
}
