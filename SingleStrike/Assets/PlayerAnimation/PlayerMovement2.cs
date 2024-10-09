using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float jumpForce = 5f;
    public float crouchSpeed = 5f;
    
    private float originalHeight;
    private bool isGrounded;
    private bool isCrouching;
    private Rigidbody rb;

    // Reference to AnimationStateController
    public AnimationStateController2 animController;

    // Dash Variables
    public float dashSpeed = 50f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashTime;
    private float lastMoveDirection = 1f;

    // Sound Reference
    public PlayerSoundManager soundManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalHeight = transform.localScale.y;
    }

    public void Update()
    {
        // Handle Crouching (Left Ctrl)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }

        // Handle Jumping (Space)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching)
        {
            Jump();
        }

        // Handle Dashing (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isCrouching)
        {
            StartDash();
        }
    }

    private void FixedUpdate()
    {
        // Handle Movement Input in FixedUpdate to apply forces properly
        Move();
    }

    private void Move()
    {
        if (!isDashing)
        {
            float move = Input.GetAxis("Horizontal");

            if (move != 0)
            {
                lastMoveDirection = move;
            }

            Vector3 moveDirection = new Vector3(move * moveSpeed, rb.velocity.y, 0f);
            rb.velocity = moveDirection;

            // Sync walking or running animations
            if (move != 0)
            {
                animController.SetWalkingState(true);  // Start walking
            }
            else
            {
                animController.SetWalkingState(false);  // Stop walking
            }
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animController.SetJumpingState(true);  // Trigger jumping animation
        if (soundManager != null) soundManager.PlayJumpSound();
    }

    private void Crouch()
    {
       
        moveSpeed = crouchSpeed;
        isCrouching = true;
        animController.SetCrouchingState(true);  // Trigger crouching animation
    }

    private void StandUp()
    {
        transform.localScale = new Vector3(transform.localScale.x, originalHeight, transform.localScale.z);
        moveSpeed = 15f;
        isCrouching = false;
        animController.SetCrouchingState(false);  // Reset crouching animation
    }

    private void StartDash()
    {
        if (canDash)
        {
            isDashing = true;
            canDash = false;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 dashDirection;

            if (verticalInput > 0)
            {
                dashDirection = new Vector3(0f, 1f, 0f);
            }
            else
            {
                dashDirection = new Vector3(lastMoveDirection, 0f, 0f);
            }

            rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);
            Invoke("StopDashing", dashDuration);
            Invoke("ResetDash", dashCooldown);

            if (soundManager != null) soundManager.PlayDashSound();
        }
    }

    private void StopDashing()
    {
        isDashing = false;
    }

    private void ResetDash()
    {
        canDash = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animController.SetJumpingState(false);  // Reset jump animation when grounded
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
