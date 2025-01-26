using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class ComboHandler : MonoBehaviour
{
    private int combo = 0;
    void Start()
    {
        EventManager.Instance.OnPlatformPlacement += OnPlatformPlacement;
        EventManager.Instance.OnLevelRestart += OnLevelRestart;

    }

    //On platform placement, play sound according to current perfect combo amount
    private void OnPlatformPlacement(bool isPerfect) 
    {
        combo = isPerfect ? combo + 1 : 0;
        AudioManager.Instance.PlayPlatformPopSfx(combo);
    }

    private void OnLevelRestart(bool isSuccess) 
    {
        combo = 0;
    }
}
