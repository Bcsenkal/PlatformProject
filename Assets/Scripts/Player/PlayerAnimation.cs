using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private int runHash = Animator.StringToHash("isRunning");
    private int danceHash = Animator.StringToHash("isDancing");
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Move(bool isMoving)
    {
        anim.SetBool(runHash, isMoving);
    }

    public void Dance(bool isDancing)
    {
        anim.SetBool(danceHash, isDancing);
    }
}
