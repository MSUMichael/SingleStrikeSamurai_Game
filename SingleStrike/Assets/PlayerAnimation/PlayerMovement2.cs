using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 5f;
    public float crouchSpeed = 2f;
    
    private float originalHeight;
    private bool isGrounded;
    public bool isCrouching;
    private Rigidbody rb;

    // Reference to AnimationStateController
    public AnimationStateController2 animController;

   
    public float sprintSpeed = 6f;  // Sprint speed
    private bool isSprinting = false;  // Tracks if the player is sprinting

    // Sound Reference
    public PlayerSoundManager soundManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalHeight = transform.localScale.y;
    }

    public void Update()
    {
        // Debug logs for input detection
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Crouch key pressed");
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Debug.Log("Crouch key released");
            StandUp();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching)
        {
            Debug.Log("Jump key pressed");
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching)
        {
            Debug.Log("Sprint key pressed");
            StartSprinting();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("Sprint key released");
            StopSprinting();
        }
    }

    private void FixedUpdate()
    {
        // Handle Movement Input in FixedUpdate to apply forces properly
        Move();
    }

    private void Move()
    {
        float move = Input.GetAxis("Horizontal");

        if (move != 0)
        {
            Vector3 moveDirection = new Vector3(move * (isSprinting ? sprintSpeed : moveSpeed), rb.velocity.y, 0f);
            rb.velocity = moveDirection;

            // Sync walking or running animations
            animController.SetWalkingState(true);  // Start walking
        }
        else
        {
            animController.SetWalkingState(false);  // Stop walking
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
        moveSpeed = 2;
        isCrouching = false;
        animController.SetCrouchingState(false);  // Reset crouching animation
    }

    private void StartSprinting()
    {
        isSprinting = true;
        Debug.Log("Started Sprinting");
        animController.SetRunningState(true);
    }

    private void StopSprinting()
    {
        isSprinting = false;
        Debug.Log("Stopped Sprinting");
        animController.SetRunningState(false);

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
