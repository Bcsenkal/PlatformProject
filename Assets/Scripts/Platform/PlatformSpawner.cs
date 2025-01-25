using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    private float platformLength;
    private float nextSpawn;
    void Start()
    {
        Managers.EventManager.Instance.OnSendPlatformScaleInfo += SetPlatformLength;
        Managers.EventManager.Instance.OnPlatformStopMoving += SpawnPlatform;
        
    }

    void SpawnPlatform()
    {
        var sideChoice = Random.Range(0, 2);
        Managers.EventManager.Instance.ONOnSpawnAnotherPlatform(sideChoice, nextSpawn);
        nextSpawn += platformLength;
    }

    private void SetPlatformLength(float length)
    {
        platformLength = length;
        SpawnPlatform();
    }
    
}
