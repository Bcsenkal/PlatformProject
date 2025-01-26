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
        Managers.EventManager.Instance.OnLevelStart += SpawnFirstPlatform;
        Managers.EventManager.Instance.OnSendPlatformScaleInfo += SetPlatformScale;
        Managers.EventManager.Instance.OnAddPlatformToSpawnedList += AddPlatformToList;
        Managers.EventManager.Instance.OnFailedPlacement += SendPlatformsToPlayer;
        Managers.EventManager.Instance.OnLevelRestart += ResetParkour; 
    }

    void SpawnFirstPlatform()
    {
        nextSpawn += initialPlatformScale.z;
        SpawnPlatform(initialPlatformScale.x);
    }

    //if spawned platform count is equal to parkour length, send parkour information to player to start parkour, else spawn a new platform in front of current one
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

    //sets the initial platform scale to use different sized platforms
    private void SetPlatformScale(Vector3 scale)
    {
        initialPlatformScale = scale;
        SpawnStaticPlatforms();
    }

    //spawn starting and ending platform
    private void SpawnStaticPlatforms()
    {
        if(parkourLength == 0)
        {
            parkourLength = 10;
        }
        Managers.EventManager.Instance.ONOnSpawnStaticPlatforms(startPoint,parkourLength);
    }

    //add spawned platform to list
    private void AddPlatformToList(Platform platform)
    {
        if(spawnedPlatforms.Count == 0)
        {
            spawnedPlatforms.Add(platform);
            return;
        }
        spawnedPlatforms.Insert(spawnedPlatforms.Count-1,platform);
    }

    //On failed placement, send current platforms to player with false bool which indicates game is going to fail
    private void SendPlatformsToPlayer()
    {
        Managers.EventManager.Instance.ONOnSetPlayerPath(spawnedPlatforms, false);
    }

    //Resets parkour
    private void ResetParkour(bool isSuccess)
    {
        startPoint = isSuccess ? spawnedPlatforms[spawnedPlatforms.Count - 1].transform.position.z : 0f;
        spawnedPlatforms.Clear();
        nextSpawn = startPoint;
        spawnedPlatformCount = 0;
        Invoke(nameof(SpawnStaticPlatforms),0.05f);
    }
    
}
