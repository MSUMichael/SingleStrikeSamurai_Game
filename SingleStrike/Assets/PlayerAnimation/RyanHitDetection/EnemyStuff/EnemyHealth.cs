using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Animator animator;
    public AudioClip deathSound;  
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took " + damageAmount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Calling Die() method");  // Log to check
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died.");

        LockPosition();
        animator.SetTrigger("Die");

  
        GetComponent<Collider>().enabled = false;

        
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
            Debug.Log("Death sound should be playing now.");
        }

        
        Destroy(gameObject, deathSound.length);
    }

    private void LockPosition()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Optionally disable other scripts
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }

        transform.hasChanged = false;
    }
}
