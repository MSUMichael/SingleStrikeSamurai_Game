using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;          // Speed at which the enemy moves
    public float detectionRange = 5f;     // Base detection range
    public float crouchDetectionRange = 2f; // Shorter detection range for crouched players
    public float attackRange = 2f;        // Attack range for the enemy
    public int health = 100;              // Health of the enemy
    public float fieldOfViewAngle = 90f;  // Field of view for enemy detection

    private Rigidbody rb;                 // For 3D physics (Rigidbody)
    private Vector3 movement;             // Vector3 for 3D movement
    private Transform targetPlayer;       // To store reference to the player's transform
    private bool playerInRange = false;   // To track whether the player is currently in range
    private bool playerInAttackRange = false; // To track if the player is within attack range
    private bool inRange = false;         // To track if the player is close enough to stop movement
    private bool weaponTakenOut = false;  // To track if the weapon take-out animation has played
    private bool isWaiting = false;       // To track if the enemy is waiting before moving

    public EnemyAnimationController enemyAnimationController; // Reference to animation controller
    public PlayerMovement2 playerMovement;  // Reference to the player's movement script (to check if they are crouching)

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component (3D)
        Debug.Log("Enemy initialized.");

        // Get the reference to the EnemyAnimationController
        enemyAnimationController = GetComponent<EnemyAnimationController>();

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
        DetectPlayer();
        CheckAttackRange();
    }

    void FixedUpdate()
    {
        if (targetPlayer != null && !inRange && !isWaiting)  // Only move if not waiting and player is detected
        {
            MoveEnemy();
        }
        else
        {
            // If no player is detected or in attack range, stop movement
            rb.velocity = Vector3.zero;
            enemyAnimationController.SetWalkingState(false);  // Stop walking animation
        }
    }

    void DetectPlayer()
    {
        // Find the object with the tag "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            // Log that the player was found
            Debug.Log("Player found.");

            // Calculate distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            //Debug.Log("Distance to player: " + distanceToPlayer);

            // Adjust detection range based on whether the player is crouching
            float currentDetectionRange = playerMovement.isCrouching ? crouchDetectionRange : detectionRange;
            Debug.Log("Player is crouching: " + playerMovement.isCrouching + ". Current detection range: " + currentDetectionRange);

            // Check if the player is within detection range and in front of the enemy
            if (distanceToPlayer < currentDetectionRange && IsPlayerInFront(playerObject.transform))
            {
                Debug.Log("Player is within detection range and in front of the enemy.");

                targetPlayer = playerObject.transform;

                if (!playerInRange)  // Player just entered the detection range
                {
                    Debug.Log("Player detected within detection range for the first time.");
                    playerInRange = true;  // Set the flag to true

                    if (!weaponTakenOut)  // If the weapon take-out animation hasn't played yet
                    {
                        Debug.Log("Playing weapon take-out animation.");
                        enemyAnimationController.TriggerTakeOutWeaponAnimation();  // Play the weapon take-out animation
                        weaponTakenOut = true;  // Ensure the animation is only played once
                        StartCoroutine(WaitBeforeMoving());  // Start the waiting coroutine
                    }
                }

                // Calculate the direction to the player and prepare movement
                Vector3 direction = (targetPlayer.position - transform.position).normalized;
                movement = new Vector3(direction.x, 0f, direction.z);  // Only move on X and Z axes
                Debug.Log("Enemy is moving towards the player. Movement vector: " + movement);
            }
            else if (playerInRange)  // Player is no longer in detection range
            {
                Debug.Log("Player is out of detection range.");
                targetPlayer = null;
                movement = Vector3.zero;  // Stop moving if the player is out of range
                playerInRange = false;    // Reset the flag
            }
        }
        else
        {
            if (playerInRange)  // If the player was in range but is now gone (destroyed or deactivated)
            {
                Debug.Log("Lost track of player. Player may have been destroyed or moved out of range.");
                targetPlayer = null;
                movement = Vector3.zero;
                playerInRange = false;
                weaponTakenOut = false;  // Reset weapon animation state for next detection
            }
        }
    }

    bool IsPlayerInFront(Transform playerTransform)
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Log the enemy's forward direction and the direction to the player
        Debug.Log("Enemy forward direction: " + transform.forward);
        Debug.Log("Direction to player: " + directionToPlayer);

        // Dot product to determine if the player is in front
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        // Safely clamp the dot product to avoid NaN errors in the angle calculation
        float clampedDotProduct = Mathf.Clamp(dotProduct, -1f, 1f);
        float angleToPlayer = Mathf.Acos(clampedDotProduct) * Mathf.Rad2Deg;

        // Log the angle to the player
        Debug.Log("Angle to player: " + angleToPlayer);

        // Check if the player is within the enemy's field of view
        bool inFront = angleToPlayer < fieldOfViewAngle / 2;
        Debug.Log("Player is in front: " + inFront);

        return inFront;
    }




    // Coroutine to wait 1 seconds before allowing movement
    IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;  // Prevent movement during waiting
        yield return new WaitForSeconds(1f);  // Wait for 1 seconds
        isWaiting = false;  // Allow movement again
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
                    Debug.Log("Player within attack range.");
                    inRange = true;  // Player is close enough, stop movement
                    enemyAnimationController.TriggerAttackAnimation();  // Trigger attack animation using a trigger
                }
            }
            else
            {
                if (playerInAttackRange)
                {
                    playerInAttackRange = false;
                    Debug.Log("Player out of attack range.");
                    inRange = false;  // Player is out of attack range, allow movement again
                }
            }
        }
    }

    void MoveEnemy()
    {
        if (movement != Vector3.zero)
        {
            Debug.Log("Moving towards the player. Movement vector: " + movement);
            rb.velocity = movement * moveSpeed;
            enemyAnimationController.SetWalkingState(true);  // Start walking animation
        }
    }


    // Function to deal damage to the enemy
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Trigger death animation
        enemyAnimationController.SetDyingState(true);

        // Destroy the enemy after death animation
        Destroy(gameObject);
        Debug.Log("Enemy Died");
    }

    // Draw Gizmos in the Scene view to represent the detection and attack ranges
    void OnDrawGizmosSelected()
    {
        // Visualize the normal detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Visualize the crouch detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, crouchDetectionRange);

        // Visualize the field of view (FOV) cone
        Gizmos.color = Color.green;
        DrawFieldOfViewGizmos();

        // Visualize the attack range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void DrawFieldOfViewGizmos()
    {
        // Get the forward direction of the enemy
        Vector3 forward = transform.forward;

        // Calculate the left and right bounds of the field of view
        Quaternion leftFOVRotation = Quaternion.AngleAxis(-fieldOfViewAngle / 2, Vector3.up);
        Quaternion rightFOVRotation = Quaternion.AngleAxis(fieldOfViewAngle / 2, Vector3.up);

        Vector3 leftFOVDirection = leftFOVRotation * forward;
        Vector3 rightFOVDirection = rightFOVRotation * forward;

        // Draw lines representing the FOV boundaries
        Gizmos.DrawLine(transform.position, transform.position + leftFOVDirection * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightFOVDirection * detectionRange);

        // Optionally, you can draw an arc to represent the full FOV
        DrawFOVArc();
    }

    void DrawFOVArc()
    {
        int segments = 30;  // Number of segments for the arc
        float angleStep = fieldOfViewAngle / segments;  // Angle between segments
        Vector3 previousPoint = transform.position + Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward * detectionRange;

        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = -fieldOfViewAngle / 2 + i * angleStep;
            Vector3 nextPoint = transform.position + Quaternion.Euler(0, currentAngle, 0) * transform.forward * detectionRange;
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }
    }

    bool IsPlayerBehind(Transform playerTransform)
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Dot product to determine if the player is behind
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        // If dotProduct is less than 0, the player is behind the enemy
        bool isBehind = dotProduct < 0;

        Debug.Log("Player is behind: " + isBehind);
        return isBehind;
    }


}





