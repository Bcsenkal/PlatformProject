using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class PlatformPool : MonoBehaviour
{
    [SerializeField]private Vector3 platformScale;
    [SerializeField]private MovingPlatform platformPrefab;
    [SerializeField]private int poolSize;
    private List<MovingPlatform> platforms = new List<MovingPlatform>();
    private MovingPlatform previousPlatform;
    void Awake()
    {
        platformScale = platformPrefab.transform.localScale;
        CreatePool();
    }

    private void Start() 
    {
        EventManager.Instance.OnSpawnMovingPlatform += SpawnPlatform;
        EventManager.Instance.OnSpawnStaticPlatforms += SpawnStaticPlatforms;
        Invoke(nameof(SendPlatformScaleInfo), 0.05f);
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            MovingPlatform platform = Instantiate(platformPrefab, transform);
            platform.InitialScale = platformScale;
            platform.gameObject.SetActive(false);
            platforms.Add(platform);
        }
    }

    private MovingPlatform FindAvailablePlatform()
    {
        var platform = platforms.Find(p => !p.gameObject.activeInHierarchy);
        if(platform == null)
        {
            platform = Instantiate(platformPrefab, transform);
            platform.InitialScale = platformScale;
            platforms.Add(platform);
        }
        return platform;
    }

    private void SpawnPlatform(float scaleX, float position, int spawnedPlatforms)
    {
        var platform = FindAvailablePlatform();
        var side = Random.Range(0, 2);
        platform.transform.position = new Vector3(side == 0 ? platformScale.x : -platformScale.x, 0, position);
        platform.transform.localScale = new Vector3(scaleX, platformScale.y, platformScale.z);
        platform.gameObject.SetActive(true);
        platform.StartMoving(previousPlatform, platformScale.x,spawnedPlatforms);
        previousPlatform = platform;
    }

    private void SendPlatformScaleInfo()
    {
        EventManager.Instance.ONOnSendPlatformScaleInfo(platformScale);
    }

    private void SpawnStaticPlatforms(int parkourLength)
    {
        var platform = FindAvailablePlatform();
        platform.transform.position = Vector3.zero;
        platform.gameObject.SetActive(true);
        previousPlatform = platform;
        platform = FindAvailablePlatform();
        platform.transform.position = new Vector3(0, 0, parkourLength * platformScale.z);
        platform.gameObject.SetActive(true);
        Managers.EventManager.Instance.ONOnAddPlatformToSpawnedList(platform);
    }
}
