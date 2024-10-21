using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectionRange = 5f;
    public float crouchDetectionRange = 2f;
    public float attackRange = 2f;
    public int health = 100;
    public float fieldOfViewAngle = 90f;

    private Rigidbody rb;
    private Vector3 movement;
    private Transform targetPlayer;
    private bool playerInRange = false;
    private bool playerInAttackRange = false;
    private bool inRange = false;
    private bool weaponTakenOut = false;
    private bool isWaiting = false;
    public bool isDead = false;  // Flag to track if the enemy is dead

    public EnemyAnimationController enemyAnimationController;
    public PlayerMovement2 playerMovement;
    private BoxCollider boxCollider;  // Reference to the BoxCollider
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();  // Get the BoxCollider component
        animator = GetComponent<Animator>();  // Get the Animator component
        Debug.Log("Enemy initialized.");

        // Automatically find the player and assign the playerMovement reference
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerMovement = playerObject.GetComponent<PlayerMovement2>();
        }
        else
        {
            Debug.LogError("No player object found with tag 'Player'");
        }
    }

    void Update()
    {
        if (isDead) return;  // Prevent any further updates if the enemy is dead

        DetectPlayer();
        CheckAttackRange();
    }

    void FixedUpdate()
    {
        if (isDead) return;  // Prevent any movement if the enemy is dead

        if (targetPlayer != null && !inRange && !isWaiting)
        {
            MoveEnemy();
        }
        else
        {
            rb.velocity = Vector3.zero;
            enemyAnimationController.SetWalkingState(false);
        }
    }

    // Detect the player, only if the enemy is not dead
    void DetectPlayer()
    {
        if (isDead) return;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            float currentDetectionRange = playerMovement.isCrouching ? crouchDetectionRange : detectionRange;

            if (distanceToPlayer < currentDetectionRange && IsPlayerInFront(playerObject.transform))
            {
                targetPlayer = playerObject.transform;

                if (!playerInRange)
                {
                    playerInRange = true;

                    if (!weaponTakenOut)
                    {
                        Debug.Log("Playing weapon take-out animation.");
                        enemyAnimationController.TriggerTakeOutWeaponAnimation();
                        weaponTakenOut = true;
                        StartCoroutine(WaitBeforeMoving());
                    }
                }

                Vector3 direction = (targetPlayer.position - transform.position).normalized;
                movement = new Vector3(direction.x, 0f, direction.z);
            }
            else if (playerInRange)
            {
                targetPlayer = null;
                movement = Vector3.zero;
                playerInRange = false;
            }
        }
        else
        {
            if (playerInRange)
            {
                targetPlayer = null;
                movement = Vector3.zero;
                playerInRange = false;
                weaponTakenOut = false;
            }
        }
    }

    void MoveEnemy()
    {
        if (movement != Vector3.zero)
        {
            rb.velocity = movement * moveSpeed;
            enemyAnimationController.SetWalkingState(true);
        }
    }

    void CheckAttackRange()
    {
        if (isDead) return;  // Prevent attacking if the enemy is dead

        if (targetPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

            if (distanceToPlayer < attackRange)
            {
                if (!playerInAttackRange)
                {
                    playerInAttackRange = true;
                    inRange = true;
                    enemyAnimationController.TriggerAttackAnimation();
                }
            }
            else
            {
                if (playerInAttackRange)
                {
                    playerInAttackRange = false;
                    inRange = false;
                }
            }
        }
    }

    // Deal damage to the enemy
    public void TakeDamage(int damage)
    {
        if (isDead) return;  // Prevent taking damage if already dead

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    // Trigger enemy death behavior
    public void Die()
    {
        if (isDead) return;  // Ensure Die() is called only once
        isDead = true;

        // Play death animation
        enemyAnimationController.TriggerDyingAnimation();
        animator.SetTrigger("Death");

        // Shorten the BoxCollider to simulate the enemy collapsing
        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }

        // Stop movement and physics interaction
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        // Disable this script to stop further behavior
        this.enabled = false;

        Debug.Log(gameObject.name + " has died.");
    }

    // Check if the player is in front of the enemy
    bool IsPlayerInFront(Transform playerTransform)
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
        float clampedDotProduct = Mathf.Clamp(dotProduct, -1f, 1f);
        float angleToPlayer = Mathf.Acos(clampedDotProduct) * Mathf.Rad2Deg;

        return angleToPlayer < fieldOfViewAngle / 2;
    }

    // Coroutine to wait before moving
    IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1f);
        isWaiting = false;
    }

    // Optional: Draw Gizmos for debugging purposes
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, crouchDetectionRange);

        Gizmos.color = Color.green;
        DrawFieldOfViewGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void DrawFieldOfViewGizmos()
    {
        Vector3 forward = transform.forward;

        Quaternion leftFOVRotation = Quaternion.AngleAxis(-fieldOfViewAngle / 2, Vector3.up);
        Quaternion rightFOVRotation = Quaternion.AngleAxis(fieldOfViewAngle / 2, Vector3.up);

        Vector3 leftFOVDirection = leftFOVRotation * forward;
        Vector3 rightFOVDirection = rightFOVRotation * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftFOVDirection * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightFOVDirection * detectionRange);

        DrawFOVArc();
    }

    void DrawFOVArc()
    {
        int segments = 30;
        float angleStep = fieldOfViewAngle / segments;
        Vector3 previousPoint = transform.position + Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward * detectionRange;

        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = -fieldOfViewAngle / 2 + i * angleStep;
            Vector3 nextPoint = transform.position + Quaternion.Euler(0, currentAngle, 0) * transform.forward * detectionRange;
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }
    }
}
