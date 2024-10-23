using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectionRange = 5f;
    public float crouchDetectionRange = 2f;
    public float attackRange = 2f;
    public float fieldOfViewAngle = 90f;
    public float waitTimeAtWaypoint = 2f; // Public variable for the wait time

    private Rigidbody rb;
    private Vector3 movement;
    private Transform targetPlayer;
    private bool playerInRange = false;
    private bool playerInAttackRange = false;
    private bool inRange = false;
    private bool weaponTakenOut = false;
    private bool isWaiting = false;
    

    public EnemyAnimationController enemyAnimationController;
    public PlayerMovement2 playerMovement;
    private BoxCollider boxCollider;
    private Animator animator;

    public Transform waypoint1; // First waypoint for patrolling
    public Transform waypoint2; // Second waypoint for patrolling
    private Transform currentTargetWaypoint; // Current waypoint to move towards
    private bool isPatrolling = true; // Whether the enemy is patrolling

    private Vector3 lastPosition; // Tracks the last position to determine movement direction

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
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

        // Initialize patrolling
        currentTargetWaypoint = waypoint1;

        // Initialize lastPosition with the current position
        lastPosition = transform.position;
    }

    void Update()
    {
        

        DetectPlayer();
        CheckAttackRange();
    }

    void FixedUpdate()
    {
        

        if (targetPlayer != null && !inRange && !isWaiting)
        {
            MoveEnemy();
        }
        else if (isPatrolling && !playerInRange && !isWaiting)
        {
            Patrol();
        }
        else
        {
            rb.velocity = Vector3.zero;
            enemyAnimationController.SetWalkingState(false);
        }

        // Update the enemy's facing direction
        UpdateFacingDirection();
    }

    void DetectPlayer()
    {
        

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

   

    

    bool IsPlayerInFront(Transform playerTransform)
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
        float clampedDotProduct = Mathf.Clamp(dotProduct, -1f, 1f);
        float angleToPlayer = Mathf.Acos(clampedDotProduct) * Mathf.Rad2Deg;

        return angleToPlayer < fieldOfViewAngle / 2;
    }

    IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1f);
        isWaiting = false;
    }

    void Patrol()
    {
        if (currentTargetWaypoint == null) return;

        Vector3 direction = (currentTargetWaypoint.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        enemyAnimationController.SetWalkingState(true);

        float distanceToWaypoint = Vector3.Distance(transform.position, currentTargetWaypoint.position);
        if (distanceToWaypoint < 0.5f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        rb.velocity = Vector3.zero;
        enemyAnimationController.SetWalkingState(false);
        yield return new WaitForSeconds(waitTimeAtWaypoint);
        isWaiting = false;

        currentTargetWaypoint = currentTargetWaypoint == waypoint1 ? waypoint2 : waypoint1;
    }

    void UpdateFacingDirection()
    {
        Vector3 movementDirection = transform.position - lastPosition;

        if (movementDirection.sqrMagnitude > 0.001f)
        {
            if (movementDirection.x > 0)
            {
                // Rotate to face right (90 degrees)
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (movementDirection.x < 0)
            {
                // Rotate to face left (-90 degrees)
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }

        lastPosition = transform.position;
    }

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

        if (waypoint1 != null && waypoint2 != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(waypoint1.position, waypoint2.position);
        }
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
