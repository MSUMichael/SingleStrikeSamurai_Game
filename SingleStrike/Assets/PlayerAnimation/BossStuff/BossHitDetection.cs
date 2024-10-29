using UnityEngine;

public class BossHitDetection : MonoBehaviour
{
    public int damage = 20; // Damage dealt by the boss
    public Animator animator; // Reference to the Animator component for the boss
    
   

    private BossHitDetection parentBossHitDetection; // Reference to the parent’s BossHitDetection script
    
    void Start()
    {
       
        // Set up reference to the parent BossHitDetection script
        parentBossHitDetection = GetComponentInParent<BossHitDetection>();

        // Disable the hitbox at the start of the scene
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.name);

        if (other.CompareTag("PlayerHurtbox"))
        {
            Debug.Log("Boss Hit the player!");

            PlayerBlocking playerBlocking = other.GetComponentInParent<PlayerBlocking>();
            Health playerHealth = other.GetComponentInParent<Health>();
            Animator playerAnimator = other.GetComponentInParent<Animator>();

            if (playerBlocking != null && playerHealth != null)
            {
                if (playerBlocking.isBlocking)
                {
                    Debug.Log("Boss attack was blocked by the player!");
                    int reducedDamage = Mathf.Max(damage / 2, 0);
                    playerHealth.TakeDamage(reducedDamage);

                    if (animator != null)
                    {
                        animator.SetTrigger("Blocked");
                    }

                    if (playerAnimator != null)
                    {
                        playerAnimator.SetTrigger("SuccessfulBlock");
                        
                    }
                }
                else
                {
                    playerHealth.TakeDamage(damage);
                    Debug.Log("Player took damage: " + damage);
                }
            }
        }
    }
    

}
