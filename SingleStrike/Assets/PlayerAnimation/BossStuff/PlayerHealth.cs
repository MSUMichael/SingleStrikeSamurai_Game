using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f; // Player's starting health

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Player took damage: " + amount + ", Remaining health: " + health);

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died!");
        // Add player death logic here (e.g., respawn, game over screen)
    }
}
