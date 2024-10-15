using UnityEngine;

public class AssassinationSystem : MonoBehaviour
{
    public Transform playerTransform;        // Player transform
    public float assassinationRange = 1.5f;  // Range for assassination
    public string assassinationAnimationName = "SwordSlash";  // Name of the assassination animation

    private bool assassinationTriggered = false;
    private Animator playerAnimator;

    void Start()
    {
        // Ensure playerTransform is assigned
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Get the player's Animator component
        playerAnimator = playerTransform.GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.LogError("Animator component not found on player.");
        }
    }

    void Update()
    {
        DetectAndAssassinateNearestEnemy();
    }

    void DetectAndAssassinateNearestEnemy()
    {
        // Find all enemies tagged as "Enemy" in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearestEnemyObject = null;
        float nearestDistance = Mathf.Infinity;  // Initialize with a very large value

        // Log all detected enemies
        //Debug.Log("Detected Enemies:");

        // Loop through all enemies to find the nearest one
        foreach (GameObject enemyObject in enemies)
        {
            Enemy enemy = enemyObject.GetComponent<Enemy>();

            // Skip null enemies or dead enemies
            if (enemy == null || enemy.isDead) continue;

            // Calculate the distance between player and this enemy
            float distanceToPlayer = Vector3.Distance(playerTransform.position, enemyObject.transform.position);

            // Log each enemy's name and distance
            Debug.Log("Enemy: " + enemyObject.name + " - Distance: " + distanceToPlayer);

            // Check if this enemy is the nearest one so far
            if (distanceToPlayer < nearestDistance)
            {
                nearestDistance = distanceToPlayer;  // Update nearest distance
                nearestEnemyObject = enemyObject;    // Update nearest enemy object
            }
        }

        // If a nearest enemy was found
        if (nearestEnemyObject != null)
        {
            // Debug Log to show the nearest enemy and its distance
            Debug.Log("Nearest enemy found: " + nearestEnemyObject.name + " at distance: " + nearestDistance);

            Enemy nearestEnemy = nearestEnemyObject.GetComponent<Enemy>();
            Transform enemyTransform = nearestEnemyObject.transform;

            // Calculate the direction from enemy to player
            Vector3 directionToPlayer = (playerTransform.position - enemyTransform.position).normalized;
            // Calculate dot product to determine if the player is behind the enemy
            float dotProduct = Vector3.Dot(enemyTransform.forward, directionToPlayer);
            bool isPlayerBehind = dotProduct < 0;

            // Check if the nearest enemy is within range and the player is behind the enemy
            if (nearestDistance < assassinationRange && isPlayerBehind && Input.GetKeyDown(KeyCode.X) && !assassinationTriggered)
            {
                TriggerAssassination(nearestEnemy);
            }

            // If assassination is triggered and the animation is playing, trigger enemy death
            if (assassinationTriggered && IsAssassinationAnimationPlaying())
            {
                nearestEnemy.Die();  // Kill the enemy
                assassinationTriggered = false;  // Reset trigger for the next enemy
            }
        }
    }

    // Trigger assassination for the specified enemy
    void TriggerAssassination(Enemy enemy)
    {
        Debug.Log("Assassination triggered on " + enemy.name);

        // Play assassination animation on the player
        playerAnimator.SetTrigger(assassinationAnimationName);
        assassinationTriggered = true;
    }

    // Check if the assassination animation is currently playing
    bool IsAssassinationAnimationPlaying()
    {
        if (playerAnimator == null) return false;

        // Get the current state of the player's animation
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        // Return true if the assassination animation is playing
        return stateInfo.IsName(assassinationAnimationName);
    }
}
