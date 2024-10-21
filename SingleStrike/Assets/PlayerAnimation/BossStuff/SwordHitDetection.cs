using UnityEngine;
using System.Collections;

public class SwordHitDetection : MonoBehaviour
{
    private bool hasHit; // Tracks if the sword has already hit the player
    public float hitResetDelay = 0.5f; // Time to wait before allowing another hit

    void Start()
    {
        hasHit = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is tagged "Player" and if the sword hasn't already hit
        if (other.CompareTag("Player") && !hasHit)
        {
            Debug.Log("Player hit by the sword!");

            // Mark that the player has been hit
            hasHit = true;

            // Optionally, add code to apply damage to the player here
            // For example: other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);

            // Reset hit detection after a delay to allow for multiple hits
            StartCoroutine(ResetHitAfterDelay());
        }
    }

    IEnumerator ResetHitAfterDelay()
    {
        yield return new WaitForSeconds(hitResetDelay);
        hasHit = false;
        //Debug.Log("Hit detection reset after delay.");
    }
}
