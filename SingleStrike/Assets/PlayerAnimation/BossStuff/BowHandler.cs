using UnityEngine;

public class BowHandler : MonoBehaviour
{
    public GameObject bowPrefab; // The bow prefab to attach
    public Transform backPosition; // The position on the back to attach the bow
    public Transform leftHandPosition; // The position on the left hand to attach the bow
    public Animator bossAnimator; // Reference to the boss animator
    private GameObject currentBow; // The instantiated bow

    private bool isBowEquipped = false; // Track if the bow is equipped in the hand

    void Start()
    {
        // Attach the bow to the back at the start
        AttachBowToBack();
    }

    void Update()
    {
        // Check the isShooting boolean in the Animator
        bool isShooting = bossAnimator.GetBool("isShooting");

        if (isShooting)
        {
            // If isShooting is true and the bow is not yet equipped, move it to the hand
            if (!isBowEquipped)
            {
                AttachBowToHand();
                isBowEquipped = true;
            }
        }
        else
        {
            // If isShooting is false and the bow is equipped, move it back to the back position
            if (isBowEquipped)
            {
                AttachBowToBack();
                isBowEquipped = false;
            }
        }
    }

    void AttachBowToBack()
    {
        if (currentBow == null)
        {
            // Instantiate the bow if it doesn't already exist
            currentBow = Instantiate(bowPrefab, backPosition);
        }

        // Parent the bow to the back position and reset its local transform
        currentBow.transform.SetParent(backPosition);
        currentBow.transform.localPosition = Vector3.zero;
        currentBow.transform.localRotation = Quaternion.identity;
        //Debug.Log("Bow attached to the back.");
    }

    void AttachBowToHand()
    {
        if (currentBow != null)
        {
            // Parent the bow to the left hand position and reset its local position
            currentBow.transform.SetParent(leftHandPosition);
            currentBow.transform.localPosition = Vector3.zero;

            // Set the local rotation to adjust the bow's orientation in the left hand
            currentBow.transform.localRotation = Quaternion.Euler(83.592f, 72.121f, -31.385f); // Adjust the Z-axis as needed
            //Debug.Log("Bow attached to the left hand with adjusted rotation.");
        }
    }
}
