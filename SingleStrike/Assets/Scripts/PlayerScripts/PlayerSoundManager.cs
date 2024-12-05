using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Written by ryan reisdorf

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource audioSource;     // The AudioSource component to play sounds
    public AudioClip attackClip;        // Sound effect for attacking
    public AudioClip jumpClip;          // Sound effect for jumping
    public AudioClip blockClip;         // Sound effect for blocking
    public AudioClip dashClip;         // Sound effect for dashing

    private void Start()
    {
        // Make sure there's an AudioSource component
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Call this method when the player attacks
    public void PlayAttackSound()
    {
        if (attackClip != null)
        {
            audioSource.PlayOneShot(attackClip);
            Debug.Log("Playing attack sound.");
        }
        else
        {
            Debug.LogWarning("Attack sound clip not assigned.");
        }
    }

    // Call this method when the player jumps
    public void PlayJumpSound()
    {
        if (jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip);
            Debug.Log("Playing jump sound.");
        }
        else
        {
            Debug.LogWarning("Jump sound clip not assigned.");
        }
    }

    // Call this method when the player blocks
    public void PlayBlockSound()
    {
        if (blockClip != null)
        {
            audioSource.PlayOneShot(blockClip);
            Debug.Log("Playing block sound.");
        }
        else
        {
            Debug.LogWarning("Block sound clip not assigned.");
        }
    }

    // Call this method when the player Dashes
    public void PlayDashSound()
    {
        if (dashClip != null)
        {
            audioSource.PlayOneShot(dashClip);
            Debug.Log("Playing bldashock sound.");
        }
        else
        {
            Debug.LogWarning("Dash sound clip not assigned.");
        }
    }
}


