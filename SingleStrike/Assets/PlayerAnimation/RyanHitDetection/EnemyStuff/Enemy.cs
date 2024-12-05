using UnityEngine;
using System.Collections;
//Written by ryan reisdorf

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float detectionRange = 5f;
    public float crouchDetectionRange = 2f;
    public float attackRange = 2f;
    public float fieldOfViewAngle = 90f;
    public float weaponDelay = 1f; // Delay before enemy starts moving after taking out the weapon
    public float attackCooldown = 1f;

    private Rigidbody rb;
    private Vector3 movement;
    private Transform targetPlayer;
    private bool playerInRange = false;
    private bool playerInAttackRange = false;
    private bool weaponTakenOut = false;
    private bool isWaiting = false;

    private EnemyAnimationController enemyAnimationController;
    private PlayerMovement2 playerMovement;
    private BoxCollider boxCollider;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        enemyAnimationController = GetComponent<EnemyAnimationController>();

        Debug.Log("Enemy initialized.");

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
        DetectPlayer();
        CheckAttackRange();
    }

    void FixedUpdate()
    {
        if (targetPlayer != null && !playerInAttackRange && !isWaiting)
        {
            ChasePlayer();
        }
        else
        {
            rb.velocity = Vector3.zero;
            enemyAnimationController.SetWalkingState(false);
        }

        UpdateFacingDirection();
    }

    /// <summary>
    /// Detects the player and sets the target if within range and field of view.
    /// </summary>
    void DetectPlayer()
    {
        if (playerMovement == null) return;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            float currentDetectionRange = playerMovement.isCrouching ? crouchDetectionRange : detectionRange;

            if (distanceToPlayer < currentDetectionRange)
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
                        StartCoroutine(WaitBeforeChasing());
                    }
                }

                Vector3 direction = (targetPlayer.position - transform.position).normalized;
                movement = new Vector3(direction.x, 0f, direction.z);
            }
            else
            {
                ResetEnemyState();
            }
        }
        else
        {
            ResetEnemyState();
        }
    }

    /// <summary>
    /// Moves the enemy towards the player.
    /// </summary>
    void ChasePlayer()
    {
        if (targetPlayer == null) return;

        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        movement = new Vector3(direction.x, 0f, direction.z);

        rb.velocity = movement * moveSpeed;
        enemyAnimationController.SetWalkingState(true);
    }

    /// <summary>
    /// Checks if the player is within attack range and triggers attack if so.
    /// </summary>
    void CheckAttackRange()
    {
        if (targetPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

            if (distanceToPlayer < attackRange)
            {
                if (!playerInAttackRange)
                {
                    playerInAttackRange = true;
                    StartCoroutine(ContinuousAttack());
                }
            }
            else
            {
                playerInAttackRange = false;
            }
        }
    }

    IEnumerator ContinuousAttack()
    {
        while (playerInAttackRange)
        {
            // Trigger the attack animation
            enemyAnimationController.TriggerAttackAnimation();

            // Wait for the attack animation cooldown (adjust as needed)
            yield return new WaitForSeconds(attackCooldown);
        }
    }


    /// <summary>
    /// Resets the enemy's state when the player is no longer detected.
    /// </summary>
    void ResetEnemyState()
    {
        playerInRange = false;
        targetPlayer = null;
        movement = Vector3.zero;
        weaponTakenOut = false;
    }

    /// <summary>
    /// Determines if the player is within the enemy's field of view.
    /// </summary>
    /// <param name="playerTransform">The player's transform.</param>
    /// <returns>True if the player is in front; otherwise, false.</returns>
    bool IsPlayerInFront(Transform playerTransform)
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
        float clampedDotProduct = Mathf.Clamp(dotProduct, -1f, 1f);
        float angleToPlayer = Mathf.Acos(clampedDotProduct) * Mathf.Rad2Deg;

        return angleToPlayer < fieldOfViewAngle / 2;
    }

    /// <summary>
    /// Updates the enemy's facing direction based on the player's position.
    /// </summary>
    void UpdateFacingDirection()
    {
        if (targetPlayer != null)
        {
            // Direction vector to the player
            Vector3 directionToPlayer = (targetPlayer.position - transform.position).normalized;

            // Determine if the player is to the right (positive X) or left (negative X) of the enemy
            if (directionToPlayer.x > 0)
            {
                // Player is to the right, set rotation to face +X (Y = 90°)
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (directionToPlayer.x < 0)
            {
                // Player is to the left, set rotation to face -X (Y = -90°)
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }




    /// <summary>
    /// Coroutine to wait before the enemy starts chasing after taking out the weapon.
    /// </summary>
    IEnumerator WaitBeforeChasing()
    {
        isWaiting = true;
        yield return new WaitForSeconds(weaponDelay);
        isWaiting = false;
    }

    /// <summary>
    /// Visualizes enemy detection ranges in the Unity Editor.
    /// </summary>
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

    /// <summary>
    /// Draws the enemy's field of view in the Unity Editor.
    /// </summary>
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

    /// <summary>
    /// Draws an arc representing the enemy's field of view in the Unity Editor.
    /// </summary>
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
