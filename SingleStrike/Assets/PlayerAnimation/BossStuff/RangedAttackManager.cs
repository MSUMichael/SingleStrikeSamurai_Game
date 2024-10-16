using UnityEngine;

public class RangedAttackManager : MonoBehaviour
{
    public GameObject arrowPrefab; // Arrow to be shot
    public Transform shootPoint; // Point from where the arrow will be shot
    public float arrowForce = 20f; // The force to apply to the arrow
    public float fireAtNormalizedTime = 0.5f; // Time in the animation (normalized) to fire the arrow

    private bool hasFiredArrow = false; // Track if the arrow was fired during the animation
    private Animator bossAnimator; // Reference to the boss animator

    void Start()
    {
        bossAnimator = GetComponent<Animator>();
    }

    public void StartShooting()
    {
        bossAnimator.SetBool("isShooting", true);
        bossAnimator.SetBool("isWalking", false); // Stop walking animation when shooting
        bossAnimator.SetTrigger("Shoot");
    }

    public void StopShooting()
    {
        bossAnimator.SetBool("isShooting", false);
        hasFiredArrow = false; // Reset arrow firing for the next animation cycle
    }

    public void CheckAnimationForFire()
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
            if (stateInfo.normalizedTime >= 0.6537f)
            {
                hasFiredArrow = false; // Ready to fire again in the next cycle
            }
        }
    }

    private void FireArrow()
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
    }
}
