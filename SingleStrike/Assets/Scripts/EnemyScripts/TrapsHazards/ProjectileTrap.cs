using UnityEngine;

public class ProjectileTrap : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the enemy moves
    public float attackRange = 1.5f;  // Range at which the enemy will attack the player
    public float attackCooldown = 2f;  // Time between attacks
    

    private Transform player;  // Reference to the player's transform
    private Rigidbody rb;  // Reference to the enemy's Rigidbody
    private bool playerDetected = false;  // Is the player detected by the enemy

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Find the player by tag (you can also assign this manually in the Inspector)
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerDetected)
        {
            MoveTowardsPlayer();
        }
    }

    // This method will be called when the player enters the detection area (trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;  // Player is detected, enemy starts moving
        }
    }

    // This method will be called when the player leaves the detection area (trigger)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;  // Player leaves detection area, enemy stops moving
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Apply force to the Rigidbody to move towards the player
        rb.AddForce(new Vector3(direction.x, 0, direction.z) * moveSpeed, ForceMode.VelocityChange);

        // Check the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        
    }

    

    // On collision with the player, the enemy is destroyed
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy collided with the player and is destroyed!");
            Destroy(gameObject);  // Destroy the enemy GameObject
        }
    }
}
