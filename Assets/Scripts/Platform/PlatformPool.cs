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
        EventManager.Instance.OnSpawnAnotherPlatform += FindAvailablePlatform;
        Invoke(nameof(SendPlatformScaleInfo), 0.1f);
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

    private void FindAvailablePlatform(int sideChoice, float position)
    {
        var platform = platforms.Find(p => !p.gameObject.activeInHierarchy);
        if(platform == null)
        {
            platform = Instantiate(platformPrefab, transform);
            platform.InitialScale = platformScale;
            platforms.Add(platform);
        }
        SpawnPlatform(platform, new Vector3(sideChoice == 0 ? -2f : 2f, 0, position));
    }

    private void SpawnPlatform(MovingPlatform platform, Vector3 position)
    {
        platform.transform.position = position;
        platform.gameObject.SetActive(true);
        var isStartingPlatform = position.z <= 0;
        platform.StartMoving(previousPlatform,isStartingPlatform);
        previousPlatform = platform;
    }

    private void SendPlatformScaleInfo()
    {
        EventManager.Instance.ONOnSendPlatformScaleInfo(platformScale.z);
    }
}
