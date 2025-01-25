using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    private Vector3 initialPlatformScale;
    [SerializeField]private int parkourLength;
    private int spawnedPlatformCount = 0;
    private List<MovingPlatform> spawnedPlatforms = new List<MovingPlatform>();
    private float nextSpawn;
    void Start()
    {

        Managers.EventManager.Instance.OnSendPlatformScaleInfo += SetPlatformScale;
        Managers.EventManager.Instance.OnCallNextPlatform += SpawnPlatform;
        Managers.EventManager.Instance.OnLevelStart += StartLevel;
        Managers.EventManager.Instance.OnAddPlatformToSpawnedList += AddPlatformToList;
        Managers.EventManager.Instance.OnFailedPlacement += SendPlatformsToPlayer;
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
            //win condition
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
        Managers.EventManager.Instance.ONOnSpawnStaticPlatforms(parkourLength);
    }

    private void AddPlatformToList(MovingPlatform platform)
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
    
}
