using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private bool isGrounded;
    private Rigidbody rb;

    // Animator reference
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();  // Get the Animator component
    }

    void Update()
    {
        // Handle Movement Input
        float move = Input.GetAxis("Horizontal");
        animator.SetFloat("MoveSpeed", Mathf.Abs(move));  // Set MoveSpeed in Animator for walking/running

        // Move the character
        Vector3 movement = new Vector3(move * moveSpeed, rb.velocity.y, 0f);
        rb.velocity = movement;

        // Handle Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Handle Attacking
        if (Input.GetMouseButtonDown(0))  // Left-click to attack
        {
            Attack();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("IsJumping", true);  // Trigger jump animation
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");  // Trigger the attack animation
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false);  // Stop jump animation when grounded
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
