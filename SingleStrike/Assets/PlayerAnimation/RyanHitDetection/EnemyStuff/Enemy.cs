using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectionRange = 5f;
    public float chaseRange = 10f;
    public float crouchDetectionRange = 2f;
    public float attackRange = 2f;
    public float fieldOfViewAngle = 90f;
    public float waitTimeAtWaypoint = 2f;

    private Rigidbody rb;
    private Vector3 movement;
    private Transform targetPlayer;
    private bool playerInRange = false;
    private bool playerInAttackRange = false;
    private bool inRange = false;
    private bool weaponTakenOut = false;
    private bool isWaiting = false;
    private bool hasBlocked = false;

    public AudioClip deathSound;
    private AudioSource audioSource;
    private Animator animator;

    public EnemyAnimationController enemyAnimationController;
    public PlayerMovement2 playerMovement;
    private BoxCollider boxCollider;

    public Transform waypoint1;
    public Transform waypoint2;
    private Transform currentTargetWaypoint;
    private bool isPatrolling = true;
    private Vector3 lastPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

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

        currentTargetWaypoint = waypoint1;
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
                playerInRange = true;
                Vector3 direction = (targetPlayer.position - transform.position).normalized;
                movement = new Vector3(direction.x, 0f, direction.z);

                if (!weaponTakenOut)
                {
                    Debug.Log("Playing weapon take-out animation.");
                    enemyAnimationController.TriggerTakeOutWeaponAnimation();
                    weaponTakenOut = true;
                    StartCoroutine(WaitBeforeMoving());
                }
            }
            // Stop chasing and return to patrol if player moves beyond chase range
            else if (playerInRange && distanceToPlayer > chaseRange)
            {
                Debug.Log("Player is out of chase range. Returning to patrol.");
                targetPlayer = null;
                movement = Vector3.zero;
                playerInRange = false;
                weaponTakenOut = false;
                isPatrolling = true;
            }
        }
        else if (playerInRange)
        {

            targetPlayer = null;
            movement = Vector3.zero;
            playerInRange = false;
            weaponTakenOut = false;
            isPatrolling = true;
        }
    }

    void MoveEnemy()
    {
        if (movement != Vector3.zero)
        {
            rb.velocity = movement * moveSpeed;
            enemyAnimationController.SetWalkingState(true);
            isPatrolling = false;
        }
    }

    private IEnumerator BlockAndThenAttack()
    {
        inRange = true; // Prevents other actions during blocking
        enemyAnimationController.TriggerBlockAnimation(); // Start blocking animation

        // Wait for the block animation to finish
        yield return new WaitForSeconds(2f); // Adjust this duration based on the length of the block animation

        inRange = false; // Allow other actions after blocking
        enemyAnimationController.TriggerAttackAnimation(); // Start attacking after blocking
    }

    void CheckAttackRange()
    {
        if (targetPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

            // Check if the enemy is within attack range
            if (distanceToPlayer < attackRange)
            {
                if (!playerInAttackRange)
                {
                    playerInAttackRange = true;
                    inRange = true;

                    // If the enemy has blocked already, go directly to attacking
                    if (!hasBlocked)
                    {
                        StartCoroutine(BlockAndThenAttack());
                        hasBlocked = true; // Prevents further blocking
                    }
                    else
                    {
                        enemyAnimationController.TriggerAttackAnimation(); // Directly attack
                    }

                    // Stop movement as the enemy is close enough to attack
                    rb.velocity = Vector3.zero;
                }
            }
            else
            {
                // When the player moves out of attack range, reset attack state and allow movement
                if (playerInAttackRange)
                {
                    playerInAttackRange = false;
                    inRange = false;
                    enemyAnimationController.SetWalkingState(true); // Resume walking animation if needed
                }
            }
        }
    }






    public void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log("Enemy died. Attempting to play death sound.");

        // Play death sound
        if (audioSource != null && deathSound != null)
        {
            audioSource.clip = deathSound;
            audioSource.Play();
            Debug.Log("Death sound played.");
        }

        DisableEnemyComponents();
        StartCoroutine(DestroyAfterDeathSound());
    }

    void DisableEnemyComponents()
    {
        rb.velocity = Vector3.zero;
        this.enabled = false;
        if (boxCollider != null)
            boxCollider.enabled = false;
    }

    IEnumerator DestroyAfterDeathSound()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }

    public void OnPlayerAttack()
    {
        Die();
    }

    bool IsPlayerInFront(Transform playerTransform)
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
        float angleToPlayer = Mathf.Acos(Mathf.Clamp(dotProduct, -1f, 1f)) * Mathf.Rad2Deg;
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
        Debug.Log("Walking animation should be playing.");

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
                transform.rotation = Quaternion.Euler(0, 90, 0);
            else if (movementDirection.x < 0)
                transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        lastPosition = transform.position;
    }
}
