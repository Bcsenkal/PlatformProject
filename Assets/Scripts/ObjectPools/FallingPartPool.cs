using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class FallingPartPool : MonoBehaviour
{
    [SerializeField]private FallingPlatformPart fallingPartPrefab;
    [SerializeField]private int poolSize;
    private List<FallingPlatformPart> parts = new List<FallingPlatformPart>();

    private void Awake() 
    {
        CreatePool();
    }

    private void Start()
    {
        EventManager.Instance.OnCreateFallingPart += CreateFallingPart;
        EventManager.Instance.OnAddFallingPartToPool += AddPartToPool;
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            FallingPlatformPart part = Instantiate(fallingPartPrefab, transform);
            part.CacheComponents();
            part.gameObject.SetActive(false);
            parts.Add(part);
        }
    }

    private void AddPartToPool(FallingPlatformPart part)
    {
        part.gameObject.SetActive(false);
        parts.Add(part);
    }

    private FallingPlatformPart FindAvailablePart()
    {
        var part = parts.Find(p => !p.gameObject.activeInHierarchy);
        if(part == null)
        {
            part = Instantiate(fallingPartPrefab, transform);
            part.CacheComponents();
            part.gameObject.SetActive(false);
            parts.Add(part);
        }
        return part;
    }

    private void CreateFallingPart(float fallingPartXPosition, float fallingPartSize,Color color,Transform sourcePlatform)
    {
        var part = FindAvailablePart();
        part.transform.position = new Vector3(fallingPartXPosition, sourcePlatform.position.y, sourcePlatform.position.z);
        part.transform.localScale = new Vector3(fallingPartSize, sourcePlatform.localScale.y, sourcePlatform.localScale.z);
        part.gameObject.SetActive(true);
        var direction = sourcePlatform.position.x < fallingPartXPosition ? 1 : -1;
        part.Activate(color,direction);
    }


}
