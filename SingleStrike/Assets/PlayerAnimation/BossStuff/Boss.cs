using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Animator bossAnimator; // Reference to the boss animator
    public float detectionRange = 20f;
    public float stopAttackRange = 15f;
    public float movementSpeed = 3f; // Speed at which the boss moves toward the player
    public float meleeAttackRange = 2f; // Melee attack range
    private RangedAttackManager rangedAttackManager; // Reference to the RangedAttackManager

    private bool isDelayStarted = false; // Track if the movement delay has started
    private bool canMove = false; // Track if the boss is allowed to move

    void Start()
    {
        rangedAttackManager = GetComponent<RangedAttackManager>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Draw debug lines for visualization
        DrawDebugLines(distanceToPlayer);

        // Check if the player is within the attack range
        if (distanceToPlayer <= detectionRange && distanceToPlayer > stopAttackRange)
        {
            bossAnimator.SetBool("swordEquipped", false);
            rangedAttackManager.StartShooting();
            canMove = false; // Reset movement if shooting
        }
        else if (distanceToPlayer <= stopAttackRange && distanceToPlayer > meleeAttackRange)
        {
            rangedAttackManager.StopShooting();

            if (!isDelayStarted && !canMove)
            {
                bossAnimator.SetTrigger("equipSword");
                StartCoroutine(MovementDelay());
                bossAnimator.SetBool("swordEquipped", true);
            }

            if (canMove)
            {
                MoveTowardsPlayer();
            }
        }
        else if (distanceToPlayer <= meleeAttackRange)
        {
            rangedAttackManager.StopShooting();
            bossAnimator.SetBool("isWalking", false);
        }
        else
        {
            rangedAttackManager.StopShooting();
            bossAnimator.SetBool("isWalking", false);
            canMove = false;
        }
    }

    #region Movement Logic
    IEnumerator MovementDelay()
    {
        isDelayStarted = true;
        yield return new WaitForSeconds(4f); // Wait for 4 seconds before allowing movement
        canMove = true;
        isDelayStarted = false;
    }

    void MoveTowardsPlayer()
    {
        bossAnimator.SetBool("isWalking", true);
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    #endregion

    #region Debug and Gizmos
    void DrawDebugLines(float distanceToPlayer)
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2, Color.green);
        if (distanceToPlayer <= detectionRange)
        {
            Debug.DrawLine(transform.position, player.position, Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopAttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
    }
    #endregion
}
