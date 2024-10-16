using UnityEngine;
using System.Collections;

public class SwordHitDetection : MonoBehaviour
{
    public float damageAmount = 10f; // Damage dealt by the sword
    private bool canDealDamage = false; // Tracks if the sword can deal damage

    void Start()
    {
        // Get the collider component attached to the sword
        Collider swordCollider = GetComponent<Collider>();

        // Ensure the collider is set to be a trigger and keep it always enabled
        if (swordCollider != null)
        {
            swordCollider.isTrigger = true;
            swordCollider.enabled = true; // Always enabled
            Debug.Log("Sword collider initialized and set as trigger, always enabled.");
        }
        else
        {
            Debug.LogError("Sword collider not found. Make sure a Collider is attached to the sword.");
        }
    }

    // Call this method to allow damage during the attack cooldown
    public void EnableDamageWindow()
    {
        canDealDamage = true;
        Debug.Log("Damage window enabled. Sword can deal damage.");
    }

    // Call this method to end the damage window
    public void DisableDamageWindow()
    {
        canDealDamage = false;
        Debug.Log("Damage window disabled. Sword can no longer deal damage.");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called. Collided with: " + other.name);

        // Check if the sword can deal damage and if the collided object is the player
        if (canDealDamage && other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("Sword hit the player! Damage dealt: " + damageAmount);
                canDealDamage = false; // Disable further damage until the next window
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on the collided object.");
            }
        }
        else
        {
            //Debug.Log("Collision detected, but sword is not allowed to deal damage.");
        }
    }
}
