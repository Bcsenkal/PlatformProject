using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Managers;

public class Platform : MonoBehaviour
{
    private PlatformColorization platformColorization;
    private PlatformMovement platformMovement;
    private PlatformHighlight platformHighlight;
    public Vector3 InitialScale { get; set; }
    private Platform previousPlatform;

    

    public void CacheComponents()
    {
        platformColorization = GetComponent<PlatformColorization>();
        platformMovement = GetComponent<PlatformMovement>();
        platformHighlight = GetComponent<PlatformHighlight>();
        platformColorization.CacheComponents();
        platformMovement.CacheComponents();
        platformHighlight.CacheComponents();
    }

    public void StartMoving(Platform previous, float platformScale, int spawnedPlatforms)
    {
        previousPlatform = previous;
        platformMovement.StartMoving(previous, platformScale, spawnedPlatforms);
    }

    //Calculate the placement distance from previously placed platform, rescale it and call for creating falling piece
    public void SplitPlatform(float distance)
    {
        var direction = distance < 0 ? -1 : 1;
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
        float fallingPartSize = transform.localScale.x - newScaleX;
        float platformEdge = transform.position.x + newScaleX / 2 * direction;
        float fallingPartXPosition = platformEdge + fallingPartSize / 2 * direction;
        transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        HighlightPlatform(isPerfect);
        CallEventsOnPlacement(isPerfect);
        previousPlatform = null;
        if(isPerfect) return;
        CreateFallingPart(fallingPartXPosition, fallingPartSize);
    }

    //Highlight platform on perfect placement
    private void HighlightPlatform(bool isPerfect)
    {
        platformHighlight.Highlight(isPerfect);
    }

    
    private void CallEventsOnPlacement(bool isPerfect)
    {
        EventManager.Instance.ONOnPlatformPlacement(isPerfect);
        EventManager.Instance.ONOnAddPlatformToSpawnedList(this);
        EventManager.Instance.ONOnCallNextPlatform(transform.localScale.x);
    }
    
    private void CreateFallingPart(float fallingPartXPosition, float fallingPartSize)
    {
        Managers.EventManager.Instance.ONOnCreateFallingPart(fallingPartXPosition, fallingPartSize,platformColorization.GetColor(),transform);
    }
    
    public void RandomizeColor()
    {
        platformColorization.RandomizeColor();
    }
}
