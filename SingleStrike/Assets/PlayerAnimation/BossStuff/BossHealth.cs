using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Animator animator;
    public AudioClip bossDeathSound; // Sound effect for boss dying
    private AudioSource audioSource; // Reference to the AudioSource

    public GameObject bloodSprayPrefab; // Reference to the Blood Spray prefab
    public GameObject bloodPoolPrefab;  // Reference to the Blood Pool prefab
    public Transform bloodSpawnPoint;   // Point where the blood spray will be spawned

    private bool hasDied;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        hasDied = false;

        if (audioSource == null)
        {
            // Add an AudioSource component if not already present
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took " + damageAmount + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();

        }
    }

    private void Die()
    {
        // Destroy the enemy GameObject or trigger a death animation
        Debug.Log("Enemy died.");
        LockPosition();
        if (hasDied != false)
        {
            BossDeathSFX();
            BossBloodSpray();
            BossBloodPool();
        }
         
        animator.SetTrigger("IsDead");
        GetComponent<Collider>().enabled = false; // Disable the enemy's collider to prevent further interactions
        this.enabled = false;

        Destroy(gameObject, 15f);
    }

    private void LockPosition()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Make Rigidbody kinematic to stop physics from affecting it
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this) // Do not disable the current EnemyHealth script
            {
                script.enabled = false;
            }
        }

        transform.hasChanged = false; // Reset any pending changes to the transform
    }

    public void BossDeathSFX()
    {
        if (bossDeathSound != null)
        {
            audioSource.PlayOneShot(bossDeathSound);
        }
    }

    public void BossBloodSpray()
    {
        if (bloodSprayPrefab != null && bloodSpawnPoint != null)
        {
            Instantiate(bloodSprayPrefab, bloodSpawnPoint.position, bloodSpawnPoint.rotation);
            Debug.Log("Blood spray created.");
        }
    }

    public void BossBloodPool()
    {
        if (bloodPoolPrefab != null)
        {
            Instantiate(bloodPoolPrefab, transform.position, Quaternion.identity);
            Debug.Log("Blood pool created.");
        }
    }
}
