using UnityEngine;

public class PlayerBlocking : MonoBehaviour
{
    public bool isBlocking = false; // Tracks if the player is currently blocking
    public Animator animator; // Reference to the Animator component

    void Update()
    {
        // Check if the "C" key is being held down
        if (Input.GetKey(KeyCode.C))
        {
            StartBlocking();
        }
        else
        {
            StopBlocking();
        }
    }

    private void StartBlocking()
    {
        if (!isBlocking)
        {
            isBlocking = true;
            Debug.Log("Player is blocking.");
            // Trigger the block animation
            if (animator != null)
            {
                animator.SetBool("IsBlocking", true);
            }
        }
    }

    private void StopBlocking()
    {
        if (isBlocking)
        {
            isBlocking = false;
            Debug.Log("Player stopped blocking.");
            // Stop the block animation
            if (animator != null)
            {
                animator.SetBool("IsBlocking", false);
            }
        }
    }
}
