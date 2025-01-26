using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class PlatformPool : MonoBehaviour
{
    private Vector3 platformScale;
    [SerializeField]private Platform platformPrefab;
    [SerializeField]private int poolSize;
    private List<Platform> platforms = new List<Platform>();
    private Platform previousPlatform;
    private Platform currentEndPlatform;
    private bool isFirstParkour;
    void Awake()
    {
        platformScale = platformPrefab.transform.localScale;
        CreatePool();
    }

    private void Start() 
    {
        isFirstParkour = true;
        EventManager.Instance.OnSpawnMovingPlatform += SpawnPlatform;
        EventManager.Instance.OnSpawnStaticPlatforms += SpawnStaticPlatforms;
        EventManager.Instance.OnLevelRestart += OnLevelRestart;
        Invoke(nameof(SendPlatformScaleInfo), 0.1f);
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Platform platform = Instantiate(platformPrefab, transform);
            platform.InitialScale = platformScale;
            platform.CacheComponents();
            platform.gameObject.SetActive(false);
            platforms.Add(platform);
        }
    }
    //Find available platform
    private Platform FindAvailablePlatform()
    {
        var platform = platforms.Find(p => !p.gameObject.activeInHierarchy);
        if(platform == null)
        {
            platform = Instantiate(platformPrefab, transform);
            platform.InitialScale = platformScale;
            platform.CacheComponents();
            platform.gameObject.SetActive(false);
            platforms.Add(platform);
        }
        return platform;
    }

    //Broadcast platform scale to other scripts
    private void SendPlatformScaleInfo() 
    {
        EventManager.Instance.ONOnSendPlatformScaleInfo(platformScale);
    }

    //Spawn platform at requested position
    private void SpawnPlatform(float scaleX, float position, int spawnedPlatforms)
    {
        var platform = FindAvailablePlatform();
        var side = Random.Range(0, 2);
        platform.transform.position = new Vector3(side == 0 ? platformScale.x : -platformScale.x, 0, position);
        platform.transform.localScale = new Vector3(scaleX, platformScale.y, platformScale.z);
        platform.RandomizeColor();
        platform.gameObject.SetActive(true);
        platform.StartMoving(previousPlatform, platformScale.x,spawnedPlatforms);
        previousPlatform = platform;
    }

    //Spawn starting and ending platform
    private void SpawnStaticPlatforms(float startPoint, int parkourLength)
    {
        if(isFirstParkour)
        {
            isFirstParkour = false;
            SpawnStartPlatform(startPoint);
        }
        SpawnEndPlatform(startPoint + parkourLength * platformScale.z);
    }

    private void SpawnStartPlatform(float startPoint)
    {
        var platform = FindAvailablePlatform();
        platform.transform.position = Vector3.forward * startPoint;
        platform.transform.localScale = platformScale;
        platform.gameObject.SetActive(true);
        previousPlatform = platform;
    }

    private void SpawnEndPlatform(float endPoint)
    {
        var platform = FindAvailablePlatform();
        platform.transform.position = new Vector3(0, 0, endPoint);
        platform.transform.localScale = platformScale;
        platform.gameObject.SetActive(true);
        EventManager.Instance.ONOnAddPlatformToSpawnedList(platform);
        currentEndPlatform = platform;
    }

    //Reset parkour on level restart
    private void OnLevelRestart(bool isSuccess)
    {
        var removingOffset = isSuccess ? previousPlatform.transform.position.z - platformScale.z * 3f : 500f;
        ResetPlatforms(removingOffset);
        isFirstParkour = !isSuccess;
        if(!isSuccess) return;
        previousPlatform = currentEndPlatform;
    }

    //reset platforms and remove old platforms from scene for reuse
    private void ResetPlatforms(float removingOffset)
    {
        for(int i = 0; i < platforms.Count; i++)
        {
            if(platforms[i].transform.position.z > removingOffset) continue;
            if(!platforms[i].gameObject.activeInHierarchy) continue;
            platforms[i].gameObject.SetActive(false);
            platforms[i].transform.position = Vector3.zero;
            platforms[i].transform.localScale = platformScale;
        }
    }
    

    
}
