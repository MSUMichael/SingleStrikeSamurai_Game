using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    public Animator playerAnim; // Reference to the Animator
    public AnimationStateController2 animController; // Reference to the animation controller

    // Update is called once per frame
    void Update()
    {
        // Check for attack inputs
        if (Input.GetMouseButtonDown(0)) // Left click for over attack
        {
            PlayerOverAttack();
        }
        else if (Input.GetMouseButtonDown(1)) // Right click for under attack
        {
            PlayerUnderAttack();
        }
    }

    private void PlayerOverAttack()
    {
        // Trigger the over attack animation
        animController.TriggerOverAttack(); // Custom method to trigger the animation
        Debug.Log("Performed Over Attack");
        // You can add more logic for attack detection, such as enabling hitboxes here
    }

    private void PlayerUnderAttack()
    {
        // Trigger the under attack animation
        animController.TriggerUnderAttack(); // Custom method to trigger the animation
        Debug.Log("Performed Under Attack");
        // You can add more logic for attack detection, such as enabling hitboxes here
    }
}
