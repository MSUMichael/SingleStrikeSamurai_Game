using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public float attackDamage = 10f;
    public float attackRange = 2f;   // Range for detecting enemies
    public float attackSpeed = 0.5f;
    public LayerMask enemyLayer;     // Layer to define enemies
    public bool isBlocking = false;
    public bool isAttacking = false;
    

    public float blockCooldown = 1f;
    private float lastBlockTime = -10f;  // Allows for immediate block on game start

    public PlayerSoundManager soundManager;
    private void Update()
    {
        // Handle Blocking (Right Mouse Click)
        if (Input.GetMouseButtonDown(1))
        {
            Block();
        }
        else if (Input.GetMouseButtonUp(1) && isBlocking)
        {
            StopBlocking();
        }

        // Handle Attack (Left Mouse Click)
        if (Input.GetMouseButtonDown(0) && !isBlocking)
        {
            Attack();
        }
    }

    private void Block()
    {
        // Only allow blocking if cooldown has passed
        if (Time.time - lastBlockTime >= blockCooldown)
        {
            isBlocking = true;
            Debug.Log("Blocking");
            if (soundManager != null) soundManager.PlayBlockSound();
        }
    }

    private void StopBlocking()
    {
        isBlocking = false;
        lastBlockTime = Time.time;  // Set block cooldown
        Debug.Log("Stopped Blocking");
    }

    private void Attack()
    {
        isAttacking = true;
        Debug.Log("Attacking with " + attackDamage + " damage");

        // Detect all nearby enemies within attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy: " + enemy.name);
                // Apply damage to the enemy
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
        if (soundManager != null) soundManager.PlayAttackSound();
        // Reset attacking state after a short delay
        Invoke("ResetAttack", attackSpeed);  // Attack lasts for 0.5 seconds
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    // Optional: Visualize attack range in the Scene view for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
