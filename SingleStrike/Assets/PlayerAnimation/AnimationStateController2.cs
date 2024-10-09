using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController2 : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWalkingState(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    public void SetRunningState(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }

    public void SetCrouchingState(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    public void SetJumpingState(bool isJumping)
    {
        animator.SetBool("IsJumping", isJumping);
    }
}
