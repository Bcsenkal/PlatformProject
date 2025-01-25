using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Managers;

public class MovingPlatform : MonoBehaviour
{
    private PlatformColorization platformColorization;
    public Vector3 InitialScale { get; set; }
    private bool isMoving;
    private MovingPlatform previousPlatform;

    //Smooth Movement
    private float elapsedTime;
    [SerializeField] private float moveTime;
    private const float minMoveTime = 0.4f;
    private float targetX;
    private float startX;

    public void CacheComponents()
    {
        platformColorization = GetComponent<PlatformColorization>();
        platformColorization.CacheComponents();
    }

    private void Update()
    {
        if(!isMoving) return;
        elapsedTime += Time.deltaTime;
        float newX = Easings.QuadEaseInOut(elapsedTime, startX, targetX - startX, moveTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        if(elapsedTime >= moveTime)
        {
            elapsedTime = 0;
            startX = targetX;
            targetX = 0 -targetX;
        }
    }

    public void StartMoving(MovingPlatform previous, float platformScale, int spawnedPlatforms)
    {
        moveTime -= spawnedPlatforms * 0.05f;
        moveTime = Mathf.Clamp(moveTime, minMoveTime, moveTime);
        previousPlatform = previous;
        EventManager.Instance.ONOnChangeNextSpawn(transform.position.z + InitialScale.z);
        Managers.EventManager.Instance.OnMouseDown += StopMoving;
        startX = transform.position.x;
        targetX = transform.position.x < 0 ? platformScale : -platformScale;
        isMoving = true;
    }

    private void StopMoving(Vector2 args)
    {
        if(!isMoving) return;
        isMoving = false;
        Managers.EventManager.Instance.OnMouseDown -= StopMoving;
        if(previousPlatform == null)
        {
            Managers.EventManager.Instance.ONOnCallNextPlatform(transform.localScale.x);
            return;
        }
        var distance = transform.position.x - previousPlatform.transform.position.x;
        SplitPlatform(distance);
    }

    private void SplitPlatform(float distance)
    {
        var isPerfect = Mathf.Abs(distance) <= 0.15f;
        float newScaleX = isPerfect ? previousPlatform.transform.localScale.x : previousPlatform.transform.localScale.x - Mathf.Abs(distance);
        newScaleX = Mathf.Clamp(newScaleX, 0, previousPlatform.transform.localScale.x);
        if(newScaleX == 0)
        {
            transform.localScale = Vector3.zero;
            Managers.EventManager.Instance.ONOnFailedPlacement();
            return;
        } 
        float newXPosition = isPerfect ? previousPlatform.transform.position.x : previousPlatform.transform.position.x + distance / 2;
        transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        EventManager.Instance.ONOnCallNextPlatform(transform.localScale.x);
        EventManager.Instance.ONOnPerfectPlacement(isPerfect);
        EventManager.Instance.ONOnAddPlatformToSpawnedList(this);
    }

    public void RandomizeColor()
    {
        platformColorization.RandomizeColor();
    }
}
