// Written by Viacheslav Sotov
using UnityEngine;
using System.Collections;

public class Enemy_V4 : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;

    private Rigidbody rb;
    private Vector3 movement;
    private Transform targetPlayer;
    private bool playerInRange = false;
    private bool playerInAttackRange = false;
    private bool isDead = false;

    private Animator animator;
    private AudioSource audioSource;

    public AudioClip deathSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead) return; 

        DetectPlayer();
        if (playerInRange)
        {
            CheckAttackRange();
        }
        else
        {
            Idle();
        }
    }

    void DetectPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            targetPlayer = playerObject.transform;
            float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

            if (distanceToPlayer < detectionRange)
            {
                playerInRange = true;
                FollowPlayer();
            }
            else
            {
                playerInRange = false;
            }
        }
    }

    void CheckAttackRange()
    {
        if (targetPlayer == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

        if (distanceToPlayer < attackRange && IsFacingPlayer())
        {
            if (!playerInAttackRange)
            {
                playerInAttackRange = true;
                rb.velocity = Vector3.zero; 
                StartCoroutine(AttackPlayerRepeatedly());
            }
        }
        else
        {
            playerInAttackRange = false;
            StopCoroutine(AttackPlayerRepeatedly()); // Stop attacking when player moves away
            animator.SetBool("IsWalking", true); 
        }
    }

    bool IsFacingPlayer()
    {
        if (targetPlayer == null) return false;

        Vector3 directionToPlayer = (targetPlayer.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < 45f;
    }

    void FacePlayer()
    {
        if (targetPlayer == null) return;

        Vector3 directionToPlayer = (targetPlayer.position - transform.position).normalized;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    IEnumerator AttackPlayerRepeatedly()
    {
        while (playerInAttackRange)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(1f); // Cooldown between attacks
        }
    }

    void FollowPlayer()
    {
        if (targetPlayer != null)
        {
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            movement = new Vector3(direction.x, 0f, direction.z);
            rb.velocity = movement * moveSpeed;
            animator.SetBool("IsWalking", true);
        }
    }

    void Idle()
    {
        rb.velocity = Vector3.zero;
        animator.SetBool("IsWalking", false); 
    }

    public void Die()
    {
        if (isDead) return; 

        isDead = true;
        rb.velocity = Vector3.zero; 
        animator.SetTrigger("Die");

        
        if (audioSource != null && deathSound != null)
        {
            audioSource.clip = deathSound;
            audioSource.Play();
        }

        
        StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(2f); // Wait for death animation and/or sound
        Destroy(gameObject); 
    }
}
