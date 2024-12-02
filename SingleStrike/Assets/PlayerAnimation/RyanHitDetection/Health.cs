using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Animator animator; // Reference to the Animator component
    private Rigidbody rb; // Reference to the Rigidbody component
    private PlayerBlocking playerBlocking; // Reference to the PlayerBlocking script

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        playerBlocking = GetComponent<PlayerBlocking>(); // Get the PlayerBlocking component
    }

    public void TakeDamage(int damageAmount)
    {
        // Check if the player is blocking; if so, do not apply damage
        if (playerBlocking != null && playerBlocking.isBlocking)
        {
            Debug.Log("Player blocked the attack and took no damage.");
            return; // Exit the method early, preventing damage
        }

        currentHealth -= damageAmount;
        Debug.Log("Player took " + damageAmount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died.");

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Freeze position
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        // Wait for 4 seconds before loading the title screen
        Invoke("LoadTitleScreen", 4f);

        // Destroy the player object after 4 seconds
        Destroy(gameObject, 4f);
    }

    private void LoadTitleScreen()
    {
        // Assuming you are using Unity's SceneManager to load the title screen
        // If you have a scene called "TitleScreen", use this:
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }

}
