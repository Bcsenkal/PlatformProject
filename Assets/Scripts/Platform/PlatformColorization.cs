using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColorization : MonoBehaviour
{
    [SerializeField]private Color[] availableColors;
    private Renderer platformRenderer;
    public void CacheComponents()
    {
        platformRenderer = GetComponent<Renderer>();
    }

    public void RandomizeColor()
    {
        platformRenderer.material.color = availableColors[Random.Range(0, availableColors.Length)];
    }
}
