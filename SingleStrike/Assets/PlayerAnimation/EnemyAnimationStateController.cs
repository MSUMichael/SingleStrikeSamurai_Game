using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    // Function to set walking animation state
    public void SetWalkingState(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    // Function to trigger attack animation using a trigger
    public void TriggerAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    // Function to trigger the weapon take-out animation using a trigger
    public void TriggerTakeOutWeaponAnimation()
    {
        animator.SetTrigger("TakeOutWeapon");
    }

    // Function to set attacking state (optional: only if you are using a bool for continuous attacking)
    public void SetAttackingState(bool isAttacking)
    {
        animator.SetBool("IsAttacking", isAttacking);
    }

    // Function to stop walking and reset to idle
    public void StopWalking()
    {
        animator.SetBool("IsWalking", false);
    }

    // Function to set the dying animation state
    public void SetDyingState(bool isDying)
    {
        animator.SetBool("IsDying", isDying);
    }

    // Optional: Function for dealing damage or effects when attack hits
    public void OnAttackHit()
    {
        // This could be called as an animation event when the attack hits
        Debug.Log("Enemy attack hit!");
    }
}
