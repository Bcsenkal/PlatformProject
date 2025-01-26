using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Managers;

public class TapToStart : MonoBehaviour
{
    private Tween popTween;
    private bool isStarted;
    void Start()
    {
        StartPopping(0f);
        Managers.EventManager.Instance.OnLevelRestart += OnLevelRestart;
        Managers.EventManager.Instance.OnMouseDown += StartLevel;
    }
    
    private void StartLevel(Vector2 args)
    {
        if(isStarted) return;
        popTween.Kill();
        transform.localScale = Vector3.zero;
        EventManager.Instance.ONOnLevelStart();
        isStarted = true;
    }

    private void OnLevelRestart(bool isSuccess)
    {
        StartPopping(1f);
    }

    private void StartPopping(float delay)
    {
        Invoke(nameof(EnableInput),delay);
        Pop(delay + 0.05f);
    }

    private void EnableInput()
    {
        isStarted = false;
        transform.localScale = Vector3.one;
    }
    
    private void Pop(float delay)
    {
        popTween = transform.DOScale(Vector3.one * 1.1f, 0.2f).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
    }
}
