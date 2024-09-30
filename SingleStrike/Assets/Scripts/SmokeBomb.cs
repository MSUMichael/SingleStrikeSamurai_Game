using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    public ParticleSystem smokeEffect;  // The particle system for the smoke
    public float smokeDuration = 5f;  // How long the smoke lasts

    void Start()
    {
        // Make sure the smoke particle system is not playing by default
        if (smokeEffect != null)
        {
            smokeEffect.Stop();
        }
    }

    // Call this method to trigger the smoke bomb effect
    public void TriggerSmokeBomb(Vector3 position)
    {
        // Move the smoke effect to the desired position
        transform.position = position;

        // Play the smoke effect
        if (smokeEffect != null)
        {
            smokeEffect.Play();
        }

        // Stop the smoke effect after a duration
        Invoke("StopSmoke", smokeDuration);
    }

    private void StopSmoke()
    {
        if (smokeEffect != null)
        {
            smokeEffect.Stop();
        }
    }
}
