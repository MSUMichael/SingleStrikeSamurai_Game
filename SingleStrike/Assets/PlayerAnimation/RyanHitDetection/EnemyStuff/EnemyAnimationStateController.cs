using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Written by ryan reisdorf

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

    // Function to trigger the dying animation using a trigger
    public void TriggerDyingAnimation()
    {
        animator.SetTrigger("Death");  // Use a trigger for the dying animation
    }

    // Optional: Function for dealing damage or effects when attack hits
    public void OnAttackHit()
    {
        Debug.Log("Enemy attack hit!");
    }

    public void TriggerBlockAnimation()
    {
        animator.SetTrigger("Block");
    }

}