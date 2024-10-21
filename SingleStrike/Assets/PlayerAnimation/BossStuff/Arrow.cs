using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            // Destroy the arrow
            Destroy(gameObject);
            //Debug.Log("Arrow hit the player and was destroyed.");

            // Optionally, add code here to deal damage to the player or trigger an effect
        }
        else
        {
            // Optionally, destroy the arrow if it hits any other object
            // Destroy(gameObject);
            // Debug.Log("Arrow hit something else and was destroyed.");
        }
    }
}
