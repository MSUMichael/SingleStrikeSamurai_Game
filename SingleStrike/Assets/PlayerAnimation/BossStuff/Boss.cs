using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Animator bossAnimator; // Reference to the boss animator
    public float detectionRange = 20f;
    public float stopAttackRange = 15f;
    public GameObject arrowPrefab; // Arrow to be shot
    public Transform shootPoint; // Point from where the arrow will be shot
    public float arrowForce = 20f; // The force to apply to the arrow
    public float fireAtNormalizedTime = 0.5f; // Time in the animation (normalized) to fire the arrow
    private bool hasFiredArrow = false; // Track if the arrow was fired during the animation

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Draw a line showing the direction the boss is facing
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2, Color.green);

        // Draw a line to the player if within range
        if (distanceToPlayer <= detectionRange)
        {
            Debug.DrawLine(transform.position, player.position, Color.red);
        }

        // Check if the player is in range
        if (distanceToPlayer <= detectionRange && distanceToPlayer >= stopAttackRange)
        {
            StartShooting();
        }
        else
        {
            StopShooting();
        }

        // Continuously check if we need to fire the arrow during the shooting animation
        CheckAnimationForFire();
    }

    void StartShooting()
    {
        bossAnimator.SetBool("isShooting", true);
        bossAnimator.SetTrigger("Shoot");
        Debug.Log("Boss started shooting!");
    }

    void StopShooting()
    {
        bossAnimator.SetBool("isShooting", false);
        Debug.Log("Boss stopped shooting!");
        hasFiredArrow = false; // Reset arrow firing for the next animation cycle
    }

    void CheckAnimationForFire()
    {
        // Get the current state info of the animator
        AnimatorStateInfo stateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0); // Layer 0

        // Check if the current state is the "ShootRelease" animation
        if (stateInfo.IsName("ShootRelease"))
        {
            
            // Check the normalized time to fire the arrow
            if (stateInfo.normalizedTime >= fireAtNormalizedTime && !hasFiredArrow)
            {
                FireArrow(); // Fire the arrow once
                hasFiredArrow = true; // Prevent multiple arrows being fired during the same cycle
            }

            // Reset firing flag when the animation is finished
            if (stateInfo.normalizedTime >= .6537f)
            {
                hasFiredArrow = false; // Ready to fire again in the next cycle
            }
        }
    }

    void FireArrow()
    {
        // Instantiate the arrow
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

        // Rotate the arrow 90 degrees on X to make it parallel to the ground
        arrow.transform.Rotate(270f, 0f, 0f);

        // Add force to the arrow to make it move
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply forward force from the shootPoint
            rb.AddForce(shootPoint.forward * arrowForce, ForceMode.Impulse);
        }

        Debug.Log("Boss fired an arrow!");
    }

    // This method draws gizmos to visualize the attack ranges in the editor
    private void OnDrawGizmos()
    {
        // Draw detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw stop attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopAttackRange);
    }
}
