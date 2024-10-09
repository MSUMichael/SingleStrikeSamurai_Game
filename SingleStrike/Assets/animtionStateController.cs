using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    public PlayerMovement playerMovement;

    // Timer to track how long forward key has been pressed
    private float forwardPressTime = 0f;
    // Time threshold for running animation (2 seconds)
    private float timeToRun = 2f;

    // Flag to check if the player is in the air
    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("d");
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space); // Check for spacebar press

        // Handle Jumping
        if (jumpPressed && !isJumping)
        {
            // Trigger the jump animation
            isJumping = true;
            animator.SetBool("IsJumping", true);
        }

        // This would be linked to your game logic, where you detect if the player lands
        // For now, we'll just assume they land after releasing the spacebar.
        if (isJumping && Input.GetKeyUp(KeyCode.Space))
        {
            // Reset the jumping animation when spacebar is released (or player lands)
            isJumping = false;
            animator.SetBool("IsJumping", false);
        }

        if (shiftPressed)
        {
            // Crouch behavior
            animator.SetBool("IsCrouching", true);

            // If both Shift and D are pressed, allow crouched movement
            if (forwardPressed)
            {
                animator.SetBool("IsWalkingWhileCrouched", true);
            }
            else
            {
                animator.SetBool("IsWalkingWhileCrouched", false);
            }

            // Ensure other animations (walking, running) are disabled while crouching
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
        else
        {
            // Reset crouch and crouched walking animation
            animator.SetBool("IsCrouching", false);
            animator.SetBool("IsWalkingWhileCrouched", false);

            if (forwardPressed)
            {
                // Increment the timer by deltaTime (seconds)
                forwardPressTime += Time.deltaTime;

                // Check if player should start running based on time
                if (forwardPressTime >= timeToRun)
                {
                    animator.SetBool("IsRunning", true);
                }
                else
                {
                    animator.SetBool("IsWalking", true);
                    animator.SetBool("IsRunning", false);
                }
            }
            else
            {
                // Reset the timer and set both walking and running animations to false
                forwardPressTime = 0f;
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", false);
            }
        }
    }
}
