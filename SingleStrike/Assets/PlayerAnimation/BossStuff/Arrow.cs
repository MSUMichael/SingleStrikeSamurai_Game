using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Written by ryan reisdorf

public class Arrow : MonoBehaviour
{
    public float lifetime = 5f; // Time before the arrow is automatically destroyed

    void Start()
    {
        // Destroy the arrow after a certain lifetime to avoid cluttering the scene
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the arrow collided with an object tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Health component from the collided object
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Deal 5 damage to the player's health
                playerHealth.TakeDamage(5);
            }

            // Destroy the arrow
            Destroy(gameObject);
        }
        else
        {
            // Optionally, destroy the arrow if it hits any other object
            // Destroy(gameObject);
        }
    }
}
