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

    public void SplitPlatform(float distance)
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
        platformHighlight.Highlight(isPerfect);
        EventManager.Instance.ONOnCallNextPlatform(transform.localScale.x);
        EventManager.Instance.ONOnPerfectPlacement(isPerfect);
        EventManager.Instance.ONOnAddPlatformToSpawnedList(this);
        previousPlatform = null;
    }

    public void RandomizeColor()
    {
        platformColorization.RandomizeColor();
    }
}
