using UnityEngine;

public class BossHitDetection : MonoBehaviour
{
    public int damage = 20; // Damage dealt by the boss
    public Animator animator; // Reference to the Animator component for the boss

    void Start()
    {
        // Disable the hitbox at the start of the scene
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.name); // Check if anything is detected

        // Check if the collided object is the player's hurtbox
        if (other.CompareTag("PlayerHurtbox"))
        {
            Debug.Log("Boss Hit the player!");

            // Get the PlayerBlocking and PlayerHealth components
            PlayerBlocking playerBlocking = other.GetComponentInParent<PlayerBlocking>();
            Health playerHealth = other.GetComponentInParent<Health>();
            Animator playerAnimator = other.GetComponentInParent<Animator>(); // Get the player's Animator

            if (playerBlocking != null && playerHealth != null)
            {
                if (playerBlocking.isBlocking)
                {
                    Debug.Log("Boss attack was blocked by the player!");
                    // Optionally reduce damage or negate it completely
                    int reducedDamage = Mathf.Max(damage / 2, 0); // Reduce damage by 50%
                    playerHealth.TakeDamage(reducedDamage);

                    // Trigger the blocked animation on the boss
                    if (animator != null)
                    {
                        animator.SetTrigger("Blocked");
                    }

                    // Trigger the block reaction animation on the player
                    if (playerAnimator != null)
                    {
                        playerAnimator.SetTrigger("SuccessfulBlock");
                    }
                }
                else
                {
                    // Player was not blocking, apply full damage
                    playerHealth.TakeDamage(damage);
                    Debug.Log("Player took damage: " + damage);
                }
            }
        }
    }
}
