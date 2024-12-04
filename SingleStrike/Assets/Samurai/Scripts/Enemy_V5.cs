using UnityEngine;
using System.Collections;

public class Enemy_V5 : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float waitTimeAtWaypoint = 2f;

    private Rigidbody rb;
    private Vector3 movement;
    private Transform targetPlayer;
    private bool playerInRange = false;
    private bool playerInAttackRange = false;
    private bool isPatrolling = true;
    private bool isWaiting = false;
    private bool isDead = false;

    private Animator animator;
    private AudioSource audioSource;

    public AudioClip deathSound;
    public Transform waypoint1;
    public Transform waypoint2;
    private Transform currentTargetWaypoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentTargetWaypoint = waypoint1;
        StartCoroutine(Patrol());
    }

    void Update()
    {
        if (isDead) return; // Prevent any action if the enemy is dead

        DetectPlayer();
        if (playerInRange)
        {
            CheckAttackRange();
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
                isPatrolling = false;

            }
            else
            {
                playerInRange = false;
                if (!isPatrolling) StartCoroutine(Patrol());
            }
        }
    }

    void CheckAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

        if (distanceToPlayer < attackRange)
        {
            if (!playerInAttackRange)
            {
                playerInAttackRange = true;
                rb.velocity = Vector3.zero; // Stop movement while attacking
                StartCoroutine(AttackPlayerRepeatedly());
            }
        }
        else
        {
            playerInAttackRange = false;
            StopCoroutine(AttackPlayerRepeatedly()); // Stop attacking when player moves away
            animator.SetBool("IsWalking", true); // Resume walking if needed
        }
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
        if (targetPlayer != null && !isWaiting)
        {
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            movement = new Vector3(direction.x, 0f, direction.z);
            rb.velocity = movement * moveSpeed;
            animator.SetBool("IsWalking", true);
        }
    }



    IEnumerator Patrol()
    {
        isPatrolling = true;

        while (isPatrolling && !playerInRange)
        {
            Vector3 direction = (currentTargetWaypoint.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            animator.SetBool("IsWalking", true);

            float distanceToWaypoint = Vector3.Distance(transform.position, currentTargetWaypoint.position);

            if (distanceToWaypoint < 0.5f)
            {
                // Idle at waypoint
                rb.velocity = Vector3.zero;
                animator.SetBool("IsWalking", false);
                isWaiting = true;
                yield return new WaitForSeconds(waitTimeAtWaypoint);
                isWaiting = false;

                // Switch to the next waypoint
                currentTargetWaypoint = currentTargetWaypoint == waypoint1 ? waypoint2 : waypoint1;
                FaceTarget(currentTargetWaypoint.position);
                Debug.Log("Enemy patrolling toward: " + currentTargetWaypoint.name);
            }

            yield return null; // Wait until the next frame
        }

        rb.velocity = Vector3.zero;
        animator.SetBool("IsWalking", false);
    }

    void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction.x != 0)
        {
            float rotationY = direction.x > 0 ? 90 : -90;
            transform.rotation = Quaternion.Euler(0, rotationY, 0);
        }
    }

    public void Die()
    {
        if (isDead) return; // Prevent multiple death calls

        isDead = true;
        rb.velocity = Vector3.zero; // Stop all movement
        animator.SetTrigger("Die");

        // Play death sound if available
        if (audioSource != null && deathSound != null)
        {
            audioSource.clip = deathSound;
            audioSource.Play();
        }

        // Disable enemy components after a short delay
        StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(2f); // Wait for death animation and/or sound
        Destroy(gameObject); // Remove the enemy from the scene
    }

    //IEnumerator DestroyAfterDeath()
    //{
    //    yield return new WaitForSeconds(deathSound != null ? deathSound.length : 2f);
    //    Destroy(gameObject);
    //}
}


