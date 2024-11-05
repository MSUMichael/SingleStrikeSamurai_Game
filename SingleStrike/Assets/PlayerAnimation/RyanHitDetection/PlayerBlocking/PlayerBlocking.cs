using UnityEngine;

public class PlayerBlocking : MonoBehaviour
{
    public bool isBlocking = false; // Tracks if the player is currently blocking
    public Animator animator; // Reference to the Animator component
    public float blockCooldown = 2f; // Cooldown duration for blocking
    public float blockDuration = 1f; // Maximum time player can block in one instance
    public Transform sparkEmitter;
    public GameObject sparkPrefab; // Reference to the spark particle prefab

    private float lastBlockTime = -Mathf.Infinity; // Last time the player blocked
    private float blockStartTime; // Time when the block started
    private Rigidbody rb; // Reference to the Rigidbody component

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the "C" key is being held down, cooldown has passed, and the player is not already blocking
        if (Input.GetKey(KeyCode.C) && Time.time >= lastBlockTime + blockCooldown && !isBlocking)
        {
            StartBlocking();
        }

        // End blocking if the block duration has passed
        if (isBlocking && Time.time >= blockStartTime + blockDuration)
        {
            StopBlocking();
        }
    }

    private void StartBlocking()
    {
        isBlocking = true;
        blockStartTime = Time.time; // Record the time when blocking starts
        Debug.Log("Player is blocking.");

        // Trigger the block animation
        if (animator != null)
        {
            animator.SetBool("IsBlocking", true);
        }

        // Freeze player position
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    private void StopBlocking()
    {
        isBlocking = false;
        Debug.Log("Player stopped blocking.");

        // Stop the block animation
        if (animator != null)
        {
            animator.SetBool("IsBlocking", false);
        }

        // Unfreeze player position
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None; // Allow movement again
            rb.constraints = RigidbodyConstraints.FreezeRotation; // Only freeze rotation if needed
        }

        // Set the cooldown timer
        lastBlockTime = Time.time;

        
    }

    private void CreateSparks()
    {
        if (sparkPrefab != null && sparkEmitter != null)
        {
            GameObject sparks = Instantiate(sparkPrefab, sparkEmitter.position, sparkEmitter.rotation);
            Destroy(sparks, 0.2f); // Destroy the sparks after 0.5 seconds
        }
    }
}
