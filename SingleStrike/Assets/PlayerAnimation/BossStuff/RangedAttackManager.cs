using UnityEngine;
//Written by ryan reisdorf

public class RangedAttackManager : MonoBehaviour
{
    public GameObject arrowPrefab; // Arrow to be shot
    public Transform shootPoint; // Point from where the arrow will be shot
    public float arrowForce = 20f; // The force to apply to the arrow
    public AudioClip bowShootSound; // Sound effect for bow shooting
    public AudioClip bowLoadSound; // Sound effect for bow shooting

    private Animator bossAnimator; // Reference to the boss animator
    private AudioSource audioSource; // Reference to the AudioSource

    void Start()
    {
        bossAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // Add an AudioSource component if not already present
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void StartShooting()
    {
        bossAnimator.SetBool("isShooting", true);
        bossAnimator.SetBool("isWalking", false); // Stop walking animation when shooting
        bossAnimator.SetTrigger("Shoot");
    }

    public void StopShooting()
    {
        bossAnimator.SetBool("isShooting", false);
    }

    // This method will be triggered by the animation event
    public void FireArrow()
    {
        // Instantiate the arrow
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

        // Rotate the arrow 90 degrees on the X-axis to make it parallel to the ground
        arrow.transform.Rotate(270f, 0f, 0f);

        // Add force to the arrow to make it move
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply forward force from the shootPoint
            rb.AddForce(shootPoint.forward * arrowForce, ForceMode.Impulse);
        }
    }

    // This method will be triggered by an animation event
    public void PlayBowShootSound()
    {
        // Play the bow shoot sound
        if (bowShootSound != null)
        {
            audioSource.PlayOneShot(bowShootSound);
        }
    }
    public void PlayBowLoadSound()
    {
        // Play the bow shoot sound
        if (bowShootSound != null)
        {
            audioSource.PlayOneShot(bowLoadSound);
        }
    }
}
