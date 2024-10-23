using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationManager : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayIdleAnimation()
    {
        ResetAllAnimations();
        animator.SetBool("isIdle", true);
    }

    public void PlayPatrolAnimation()
    {
        ResetAllAnimations();
        animator.SetBool("isPatrolling", true);
    }

    public void PlayChaseAnimation()
    {
        ResetAllAnimations();
        animator.SetBool("isChasing", true);
    }

    public void PlayAttackAnimation()
    {
        ResetAllAnimations();
        animator.SetBool("isAttacking", true);
    }

    private void ResetAllAnimations()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isPatrolling", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", false);
    }
}
