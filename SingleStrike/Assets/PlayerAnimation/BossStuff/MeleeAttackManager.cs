using UnityEngine;
using System.Collections;

public class MeleeAttackManager : MonoBehaviour
{
    public Animator bossAnimator; // Reference to the boss's animator
    public float meleeAttackRange = 2f; // Melee attack range
    public float attackCooldown = 1.5f; // Time between consecutive attacks

    private BossAI bossAI; // Reference to the BossAI script
    public SwordHitDetection swordHitDetection; // Reference to the SwordHitDetection script
    private bool canAttack = true; // Tracks if the boss can attack

    void Start()
    {
        // Get the BossAI component
        bossAI = GetComponent<BossAI>();

        // Log to see if the reference is set correctly
        if (swordHitDetection != null)
        {
            Debug.Log("SwordHitDetection successfully assigned in Start().");
        }
        else
        {
            Debug.LogWarning("SwordHitDetection reference is still null in Start().");
        }
    }

    public void OnDefenseStance()
    {
        //Debug.Log("OnDefenseStance method called.");

        if (swordHitDetection != null)
        {
            //swordHitDetection.ResetHitDetection();
            //Debug.Log("Defense stance triggered. Hit detection reset.");
        }
        else
        {
            Debug.LogWarning("SwordHitDetection reference is missing in OnDefenseStance().");
        }
    }


    void Update()
    {
        if (bossAI == null)
        {
            // Debug.LogWarning("BossAI reference is missing.");
            return; // Make sure BossAI is present
        }

        float distanceToPlayer = Vector3.Distance(transform.position, bossAI.player.position);

        // Check if the player is within melee attack range and the boss can attack
        if (distanceToPlayer <= meleeAttackRange && canAttack)
        {
            // Debug.Log("Player is within melee range. Attempting melee attack.");
            PerformMeleeAttack();
        }
    }

    void PerformMeleeAttack()
    {
        // Trigger the melee attack animation
        int attackType = Random.Range(1, 4); // Randomly choose an attack type
        switch (attackType)
        {
            case 1:
                bossAnimator.SetTrigger("meleeAttack");
                // Debug.Log("Triggered meleeAttack animation.");
                break;
            case 2:
                bossAnimator.SetTrigger("meleeAttack2");
                // Debug.Log("Triggered meleeAttack2 animation.");
                break;
            case 3:
                bossAnimator.SetTrigger("meleeAttack3");
                // Debug.Log("Triggered meleeAttack3 animation.");
                break;
        }

        canAttack = false; // Prevent further attacks until the cooldown is over
        // Debug.Log("Attack initiated. canAttack set to false.");

        // Start the attack cooldown
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        // Debug.Log("Starting attack cooldown for " + attackCooldown + " seconds.");
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; // Allow the boss to attack again after the cooldown
        // Debug.Log("Attack cooldown complete. canAttack set to true.");
    }

    

}
