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
        Debug.Log("Walking state set to: " + isWalking);
    }

    public void SetRunningState(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
        Debug.Log("Running state set to: " + isRunning);
    }

    public void SetCrouchingState(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
        Debug.Log("Crouching state set to: " + isCrouching);
    }

    public void SetJumpingState(bool isJumping)
    {
        animator.SetBool("IsJumping", isJumping);
        Debug.Log("Jumping state set to: " + isJumping);
    }
}
