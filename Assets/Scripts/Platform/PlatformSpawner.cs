using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    private Vector3 initialPlatformScale;
    [SerializeField]private int parkourLength;
    private int spawnedPlatformCount = 0;
    private List<Platform> spawnedPlatforms = new List<Platform>();
    private float nextSpawn;
    private float startPoint = 0f;
    void Start()
    {
        Managers.EventManager.Instance.OnCallNextPlatform += SpawnPlatform;
        Managers.EventManager.Instance.OnLevelStart += StartLevel;
        Managers.EventManager.Instance.OnSendPlatformScaleInfo += SetPlatformScale;
        Managers.EventManager.Instance.OnAddPlatformToSpawnedList += AddPlatformToList;
        Managers.EventManager.Instance.OnFailedPlacement += SendPlatformsToPlayer;
        Managers.EventManager.Instance.OnLevelRestart += RestartLevel;
    }

    void StartLevel()
    {
        nextSpawn += initialPlatformScale.z;
        SpawnPlatform(initialPlatformScale.x);
    }

    void SpawnPlatform(float platformScale)
    {
        if(spawnedPlatformCount >= parkourLength - 1)
        {
            Managers.EventManager.Instance.ONOnSetPlayerPath(spawnedPlatforms, true);
            return;
        } 
        Managers.EventManager.Instance.ONOnSpawnMovingPlatform(platformScale, nextSpawn, spawnedPlatformCount);
        nextSpawn += initialPlatformScale.z;
        spawnedPlatformCount++;
    }

    private void SetPlatformScale(Vector3 scale)
    { 
        initialPlatformScale = scale;
        SpawnStaticPlatforms();
    }

    private void SpawnStaticPlatforms()
    {
        if(parkourLength == 0)
        {
            parkourLength = 10;
        }
        Managers.EventManager.Instance.ONOnSpawnStaticPlatforms(startPoint,parkourLength);
    }

    private void AddPlatformToList(Platform platform)
    {
        if(spawnedPlatforms.Count == 0)
        {
            spawnedPlatforms.Add(platform);
            return;
        }
        spawnedPlatforms.Insert(spawnedPlatforms.Count-1,platform);
    }

    private void SendPlatformsToPlayer()
    {
        Managers.EventManager.Instance.ONOnSetPlayerPath(spawnedPlatforms, false);
    }

    private void RestartLevel(bool isSuccess)
    {
        startPoint = spawnedPlatforms[spawnedPlatforms.Count - 1].transform.position.z;
        spawnedPlatforms.Clear();
        nextSpawn = startPoint;
        spawnedPlatformCount = 0;
        Invoke(nameof(SpawnStaticPlatforms),0.05f);
    }
    
}
