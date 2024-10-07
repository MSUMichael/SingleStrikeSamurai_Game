using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float jumpForce = 5f;
    public float crouchSpeed = 5f;
    public float crouchHeight = 0.5f;
    private float originalHeight;
    private bool isGrounded;
    private bool isCrouching;
    private Rigidbody rb;
    

    // Dash Variables
    public float dashSpeed = 50f;  // Force applied during dash
    public float dashDuration = 0.2f;  // How long the dash lasts
    public float dashCooldown = 2f;  // Cooldown between dashes
    private bool isDashing = false;
    private bool canDash = true;
    private float dashTime;
    private float lastMoveDirection = 1f;  // Keeps track of last direction on X-axis

    
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
        if (!isDashing)  // Normal movement when not dashing
        {
            float move = Input.GetAxis("Horizontal");

            if (move != 0)
            {
                lastMoveDirection = move;  // Update the last move direction when moving
            }

            // Apply force for movement along the X-axis
            Vector3 moveDirection = new Vector3(move * moveSpeed, rb.velocity.y, 0f);
            rb.velocity = moveDirection;  // Set the velocity directly for consistent movement

            if (move != 0)
            {
                //Debug.Log("Moving: " + (move > 0 ? "Right" : "Left"));
            }
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //Debug.Log("Jumping");
        if (soundManager != null) soundManager.PlayJumpSound();
    }

    private void Crouch()
    {
        transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
        moveSpeed = crouchSpeed;
        isCrouching = true;
        //Debug.Log("Crouching");
    }

    private void StandUp()
    {
        transform.localScale = new Vector3(transform.localScale.x, originalHeight, transform.localScale.z);
        moveSpeed = 15f;  // Reset move speed after crouching
        isCrouching = false;
        //Debug.Log("Standing up");
    }

    private void StartDash()
    {
        if (canDash)
        {
            isDashing = true;
            canDash = false;
            //Debug.Log("Player is dashing.");

            // Determine dash direction based on player input (X-axis or Y-axis)
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 dashDirection;

            if (verticalInput > 0)  // If the player is pressing up, dash upward
            {
                dashDirection = new Vector3(0f, 1f, 0f);
            }
            else  // Otherwise, dash along the X-axis
            {
                dashDirection = new Vector3(lastMoveDirection, 0f, 0f);
            }

            // Apply force for dashing using Rigidbody.AddForce()
            rb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

            // Start cooldown and stop dash after dash duration
            Invoke("StopDashing", dashDuration);
            Invoke("ResetDash", dashCooldown);

            if (soundManager != null) soundManager.PlayDashSound();
        }
    }

    private void StopDashing()
    {
        isDashing = false;
        //Debug.Log("Player stopped dashing.");
    }

    private void ResetDash()
    {
        canDash = true;
        //Debug.Log("Dash cooldown ended. Player can dash again.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Can Jump Again");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            //Debug.Log(isGrounded);
        }
    }
}
