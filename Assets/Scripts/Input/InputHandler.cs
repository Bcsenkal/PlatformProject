using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class InputHandler : MonoBehaviour
{
    private bool isStarted;
    private void Start()
    {
        EventManager.Instance.OnMouseDown += StartLevel;
    }

    private void StartLevel(Vector2 position)
    {
        if (!isStarted)
        {
            isStarted = true;
            EventManager.Instance.ONOnLevelStart();
        }
    }
}
