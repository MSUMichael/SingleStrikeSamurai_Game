using UnityEngine;

public class AssassinationSystem : MonoBehaviour
{
    public Transform playerTransform;        // Player transform
    public Transform enemyTransform;         // Enemy transform
    public Enemy enemy;                      // Reference to the enemy script
    public float assassinationRange = 1.5f;  // Range for assassination
    public string assassinationAnimationName = "SwordSlash";  // Name of the assassination animation

    private bool isPlayerInRange = false;
    private bool isPlayerBehind = false;
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

        // Ensure enemyTransform is assigned
        if (enemyTransform == null)
        {
            enemyTransform = GameObject.FindGameObjectWithTag("Enemy").transform;
        }

        // Assign the Enemy script from the enemyTransform
        if (enemyTransform != null)
        {
            enemy = enemyTransform.GetComponent<Enemy>();
        }

        if (enemy == null)
        {
            Debug.LogError("Enemy script not found on the enemy object.");
        }
    }

    void Update()
    {
        DetectPlayerPosition();
        CheckForAssassination();
    }

    // Detect if the player is behind and close to the enemy
    void DetectPlayerPosition()
    {
        if (enemyTransform == null || playerTransform == null) return;

        Vector3 directionToPlayer = (playerTransform.position - enemyTransform.position).normalized;
        float dotProduct = Vector3.Dot(enemyTransform.forward, directionToPlayer);
        isPlayerBehind = dotProduct < 0;

        float distanceToPlayer = Vector3.Distance(playerTransform.position, enemyTransform.position);
        isPlayerInRange = distanceToPlayer < assassinationRange;
    }

    // Check if assassination conditions are met
    void CheckForAssassination()
    {
        if (isPlayerInRange && isPlayerBehind && Input.GetKeyDown(KeyCode.X) && !assassinationTriggered)
        {
            TriggerAssassination();
        }

        // If assassination animation is playing, trigger the enemy's death
        if (assassinationTriggered && IsAssassinationAnimationPlaying())
        {
            if (enemy != null)
            {
                enemy.Die();  // Trigger enemy death if assassination is playing
            }
            else
            {
                Debug.LogError("Enemy reference is null! Cannot call Die().");
            }

            assassinationTriggered = false;  // Reset trigger to avoid multiple deaths
        }
    }

    void TriggerAssassination()
    {
        Debug.Log("Assassination triggered!");

        // Play assassination animation
        playerAnimator.SetTrigger("SwordSlash");
        assassinationTriggered = true;
    }

    // Check if the player's assassination animation is playing
    bool IsAssassinationAnimationPlaying()
    {
        if (playerAnimator == null) return false;

        // Get the current animation state of the player
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        // Check if the current animation is the assassination animation
        return stateInfo.IsName(assassinationAnimationName);
    }
}
