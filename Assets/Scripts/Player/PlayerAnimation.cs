using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private readonly int runHash = Animator.StringToHash("isRunning");
    private readonly int danceHash = Animator.StringToHash("isDancing");
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    public void Move(bool isMoving)
    {
        anim.SetBool(runHash, isMoving);
    }

    public void Dance(bool isDancing)
    {
        anim.SetBool(danceHash, isDancing);
    }
}
