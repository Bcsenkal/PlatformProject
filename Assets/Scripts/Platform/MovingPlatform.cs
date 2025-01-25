using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Managers;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveTime = 1f;

    private float targetX;
    public Vector3 InitialScale { get; set; }
    private bool isMoving;
    private Tween moveTween;
    private MovingPlatform previousPlatform;

    private void Move()
    {
        targetX = transform.position.x < 0 ? 2f : -2f;
        moveTween = transform.DOMoveX(targetX, moveTime).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    public void StartMoving(MovingPlatform previous, bool isStartingPlatform)
    {
        
        previousPlatform = previous;
        isMoving = true;
        EventManager.Instance.ONOnChangeNextSpawn(transform.position.z + InitialScale.z);
        Managers.EventManager.Instance.OnMouseDown += StopMoving;
        Debug.Log(isStartingPlatform);
        if(isStartingPlatform)
        {
            transform.position = new Vector3(0, 0, 0);
            isMoving = false;
            return;
        }
        Move();
    }

    public void StopMoving(Vector2 args)
    {
        isMoving = false;
        moveTween.Kill();
        Managers.EventManager.Instance.ONOnPlatformStopMoving();
        Managers.EventManager.Instance.OnMouseDown -= StopMoving;
    }
}
