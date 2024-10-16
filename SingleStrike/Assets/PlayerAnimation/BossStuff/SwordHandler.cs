using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwordHandler : MonoBehaviour
{
    public GameObject swordPrefab; // The sword prefab to attach
    public GameObject scabbard; // The scabbard object
    public Transform scabbardPosition; // The position on the scabbard to attach the sword
    public Transform rightHandPosition; // The position on the right hand to attach the sword
    public Animator bossAnimator; // Reference to the boss animator
    public float equipDelay = 0.5f; // Delay time for equipping the sword

    private GameObject currentSword; // The instantiated sword
    private bool isSwordEquipped = false; // Track if the sword is equipped in the hand
    private bool isEquipping = false; // Track if the sword is in the process of equipping/unequipping
    private BossAI bossAI;

    void Start()
    {
        bossAI = GetComponent<BossAI>();
        // Attach the sword to the scabbard at the start
        AttachSwordToScabbard();
        scabbard.SetActive(true); // Make sure the scabbard is visible
    }

    void Update()
    {
        if (bossAI == null) return; // Ensure the BossAI reference is valid

        float distanceToPlayer = Vector3.Distance(transform.position, bossAI.player.position);

        // Equip the sword when within a range slightly larger than melee range
        if (distanceToPlayer <= bossAI.meleeAttackRange + 1f)
        {
            // Equip the sword if it's not already equipped and not in the process of equipping
            if (!isSwordEquipped && !isEquipping)
            {
                StartCoroutine(EquipSword());
            }
        }
        else if (distanceToPlayer > bossAI.stopAttackRange)
        {
            // Unequip the sword if it's equipped and not in the process of unequipping
            if (isSwordEquipped && !isEquipping)
            {
                StartCoroutine(UnequipSword());
            }
        }
    }



    IEnumerator EquipSword()
    {
        isEquipping = true;

        // Move the sword to the hand position immediately before the animation starts
        AttachSwordToHand();
        scabbard.SetActive(false); // Hide the scabbard immediately

        //bossAnimator.SetTrigger("equipSword"); // Trigger the sword equip animation
        yield return new WaitForSeconds(equipDelay); // Wait for the animation to complete

        isSwordEquipped = true;
        isEquipping = false;
        Debug.Log("Sword equipped to the hand.");
    }


    IEnumerator UnequipSword()
    {
        isEquipping = true;
        yield return new WaitForSeconds(equipDelay); // Wait for the unequip delay

        AttachSwordToScabbard(); // Move the sword back to the scabbard
        isSwordEquipped = false;
        isEquipping = false;
        scabbard.SetActive(true); // Show the scabbard when the sword is unequipped
        Debug.Log("Sword returned to the scabbard.");
    }


    void AttachSwordToScabbard()
    {
        if (currentSword == null)
        {
            // Instantiate the sword if it doesn't already exist
            currentSword = Instantiate(swordPrefab, scabbardPosition);
        }

        // Parent the sword to the scabbard position and reset its local transform
        currentSword.transform.SetParent(scabbardPosition);
        currentSword.transform.localPosition = Vector3.zero;
        currentSword.transform.localRotation = Quaternion.identity;
    }

    void AttachSwordToHand()
    {
        if (currentSword != null)
        {
            // Parent the sword to the right hand position and reset its local transform
            currentSword.transform.SetParent(rightHandPosition);
            currentSword.transform.localPosition = Vector3.zero;

            // Adjust the rotation for proper orientation in the right hand
            currentSword.transform.localRotation = Quaternion.Euler(41.515f, -98.321f, 2.308f); // Adjust Y rotation as needed
        }
    }

}
