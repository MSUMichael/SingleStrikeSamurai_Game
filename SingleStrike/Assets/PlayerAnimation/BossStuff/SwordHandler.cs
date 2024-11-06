using UnityEngine;

public class SwordHandler : MonoBehaviour
{
    public GameObject swordPrefab; // The sword prefab to attach
    public Transform rightHandPosition; // The position on the right hand to attach the sword
    public Animator bossAnimator; // Reference to the boss animator
    public AudioClip swordEquipSound; // Sound effect for equipping the sword

    private GameObject currentSword; // The instantiated sword
    private bool isSwordEquipped = false; // Track if the sword is equipped in the hand
    private BossAI bossAI;
    private AudioSource audioSource; // Reference to the AudioSource

    void Start()
    {
        bossAI = GetComponent<BossAI>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // Add an AudioSource component if not already present
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        InstantiateSword(); // Create the sword but do not attach it initially
    }

    void Update()
    {
        if (bossAI == null) return; // Ensure the BossAI reference is valid

        float distanceToPlayer = Vector3.Distance(transform.position, bossAI.player.position);

        // Equip the sword when within a range slightly larger than melee range
        if (distanceToPlayer <= bossAI.meleeAttackRange + 1f)
        {
            // Trigger the equip animation if the sword is not already equipped
            if (!isSwordEquipped)
            {
                bossAnimator.SetTrigger("EquipSword"); // Trigger the equip animation
            }
        }
        else if (distanceToPlayer > bossAI.stopAttackRange)
        {
            // Unequip the sword if it's equipped
            if (isSwordEquipped)
            {
                UnequipSword();
            }
        }
    }

    // This method will be called via an animation event
    public void ActivateSwordDuringEquip()
    {
        AttachSwordToHand(); // Attach the sword to the hand
        isSwordEquipped = true;
        Debug.Log("Sword activated during the equip animation.");
    }

    // This method will be called via an animation event to play the equip sound
    public void PlaySwordEquipSound()
    {
        if (swordEquipSound != null)
        {
            
            audioSource.PlayOneShot(swordEquipSound, 0.35f);
        }
    }

    void UnequipSword()
    {
        DetachSwordFromHand();
        isSwordEquipped = false;
        Debug.Log("Sword unequipped.");
    }

    void InstantiateSword()
    {
        if (currentSword == null)
        {
            // Instantiate the sword in a deactivated state initially
            currentSword = Instantiate(swordPrefab);
            currentSword.SetActive(false);
        }
    }

    void AttachSwordToHand()
    {
        if (currentSword != null)
        {
            // Activate the sword and parent it to the right hand position
            currentSword.SetActive(true);
            currentSword.transform.SetParent(rightHandPosition);
            currentSword.transform.localPosition = Vector3.zero;

            // Adjust the rotation for proper orientation in the right hand
            currentSword.transform.localRotation = Quaternion.Euler(-32.876f, 122.654f, 151.993f); // Adjust as needed
        }
    }

    void DetachSwordFromHand()
    {
        if (currentSword != null)
        {
            currentSword.SetActive(false); // Deactivate the sword when unequipped
            currentSword.transform.SetParent(null); // Detach from the hand
        }
    }
}
