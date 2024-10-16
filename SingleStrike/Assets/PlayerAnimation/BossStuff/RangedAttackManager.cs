using UnityEngine;

public class RangedAttackManager : MonoBehaviour
{
    public GameObject arrowPrefab; // Arrow to be shot
    public Transform shootPoint; // Point from where the arrow will be shot
    public float arrowForce = 20f; // The force to apply to the arrow

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
    }

    // This method will be triggered by the animation event
    public void FireArrow()
    {
        // Instantiate the arrow
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

        // Rotate the arrow 90 degrees on the X-axis to make it parallel to the ground
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
