using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class PlatformMovement : MonoBehaviour
{
    private Platform platform;
    //Smooth Movement
    private float elapsedTime;
    [SerializeField] private float moveTime;
    private float currentMoveTime;
    private const float minMoveTime = 0.4f;
    private float targetX;
    private float startX;

    private bool isMoving;
    private Platform previousPlatform;
    public void CacheComponents()
    {
        currentMoveTime = moveTime;
        platform = GetComponent<Platform>();
    }

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

    public void StartMoving(Platform previous, float platformScale, int spawnedPlatforms)
    {
        currentMoveTime -= spawnedPlatforms * 0.05f;
        currentMoveTime = Mathf.Clamp(currentMoveTime, minMoveTime, currentMoveTime);
        previousPlatform = previous;
        EventManager.Instance.ONOnChangeNextSpawn(transform.position.z + platform.InitialScale.z);
        EventManager.Instance.OnMouseDown += StopMoving;
        startX = transform.position.x;
        targetX = transform.position.x < 0 ? platformScale : -platformScale;
        isMoving = true;
    }

    private void StopMoving(Vector2 args)
    {
        if(!isMoving) return;
        isMoving = false;
        currentMoveTime = moveTime;
        EventManager.Instance.OnMouseDown -= StopMoving;
        if(previousPlatform == null)
        {
            EventManager.Instance.ONOnCallNextPlatform(transform.localScale.x);
            return;
        }
        var distance = transform.position.x - previousPlatform.transform.position.x;
        platform.SplitPlatform(distance);
    }
}
