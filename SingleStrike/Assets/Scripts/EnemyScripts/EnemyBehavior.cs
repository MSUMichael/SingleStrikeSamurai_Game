using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Enemy States
    private enum State { Idle, Patrol, Chase, Attack, Block }
    private State currentState;

    // Public variables
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float blockCooldown = 5f;    // Cooldown time between blocks
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public Transform[] Points;
    private int pointIndex = 0;

    // Internal variables
    private Animator animator;
    private float lastBlockTime;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (Points == null || Points.Length < 2)
        {
            Debug.LogError("Please assign two patrol points in the Inspector!");
            return;
        }

        // Start with patrol state
        SetPatrolState();
        lastBlockTime = Time.time; // Initialize block cooldown
    }

    private void Update()
    {
        // Update enemy behavior based on the current state
        switch (currentState)
        {
            case State.Patrol:
                PatrolBehavior();
                break;
            case State.Chase:
                ChaseBehavior();
                break;
            case State.Attack:
                AttackBehavior();
                break;
            case State.Block:
                BlockBehavior();
                break;
        }
    }

    // Patrol Behavior
    private void PatrolBehavior()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isPatrolling", true);

        Transform targetPoint = Points[pointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            pointIndex = (pointIndex + 1) % Points.Length;  // Switch between point 1 and 2
        }

        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            SetChaseState();
        }
    }

    // Chase Behavior
    private void ChaseBehavior()
    {
        animator.SetBool("isPatrolling", false);
        animator.SetBool("isChasing", true);

        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            SetAttackState();
        }
        else if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            SetPatrolState();
        }
    }

    // Attack Behavior
    private void AttackBehavior()
    {
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", true);
        Debug.Log("Attacking the player!");

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            SetChaseState();
        }

        // If the enemy can block, randomly decide to block
        if (Time.time > lastBlockTime + blockCooldown && ShouldBlock())
        {
            SetBlockState();
        }
    }

    // Block Behavior
    private void BlockBehavior()
    {
        animator.SetBool("isBlocking", true);  // Enable block animation
        Debug.Log("Enemy is blocking!");

        // Block for a short duration, then return to previous behavior (attack/chase)
        if (Time.time > lastBlockTime + 2f) // Block for 2 seconds
        {
            animator.SetBool("isBlocking", false);
            lastBlockTime = Time.time;
            SetChaseState();  // Go back to chasing or attacking after blocking
        }
    }

    // Helper methods to transition states
    private void SetPatrolState()
    {
        currentState = State.Patrol;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isBlocking", false);
        UpdateState();
    }

    private void SetChaseState()
    {
        currentState = State.Chase;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isBlocking", false);
        UpdateState();
    }

    private void SetAttackState()
    {
        currentState = State.Attack;
        UpdateState();
    }

    private void SetBlockState()
    {
        currentState = State.Block;
        lastBlockTime = Time.time;  // Reset block cooldown
        UpdateState();
    }

    private void UpdateState()
    {
        animator.SetBool("isIdle", currentState == State.Idle);
        animator.SetBool("isPatrolling", currentState == State.Patrol);
        animator.SetBool("isChasing", currentState == State.Chase);
        animator.SetBool("isAttacking", currentState == State.Attack);
        animator.SetBool("isBlocking", currentState == State.Block);
    }

    // Logic for determining if the enemy should block (random chance in this case)
    private bool ShouldBlock()
    {
        // Example: 30% chance to block when possible
        return Random.Range(0f, 1f) < 0.3f;
    }
}

//using UnityEngine;

//public class EnemyBehavior : MonoBehaviour
//{

//    private enum State { Idle, Patrol, Chase, Attack }
//    private State currentState;

//    public Transform player;                    
//    public float detectionRange = 6f;          
//    public float attackRange = 2f;              
//    public float patrolSpeed = 2f;              
//    public float chaseSpeed = 4f;               
//    public Transform[] Points;                  
//    private int pointIndex = 0;                 


//    private Animator animator;                  

//    private void Start()
//    {
//        animator = GetComponent<Animator>();

//        if (Points == null || Points.Length < 2)
//        {
//            Debug.LogError("Please assign two patrol points in the Inspector!");
//            return;
//        }

//        SetPatrolState();
//    }

//    private void Update()
//    {
//        switch (currentState)
//        {
//            case State.Patrol:
//                PatrolBehavior();
//                break;
//            case State.Chase:
//                ChaseBehavior();
//                break;
//            case State.Attack:
//                AttackBehavior();
//                break;
//        }
//    }

//    private void PatrolBehavior()
//    {

//        animator.SetBool("isIdle", false);
//        animator.SetBool("isPatrolling", true);


//        Transform targetPoint = Points[pointIndex];


//        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);


//        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
//        {

//            pointIndex = (pointIndex + 1) % Points.Length;  
//        }


//        if (Vector3.Distance(transform.position, player.position) < detectionRange)
//        {
//            SetChaseState();
//        }
//    }


//    private void ChaseBehavior()
//    {
//        animator.SetBool("isPatrolling", false);  
//        animator.SetBool("isChasing", true);  


//        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);


//        if (Vector3.Distance(transform.position, player.position) < attackRange)
//        {
//            SetAttackState();
//        }

//        else if (Vector3.Distance(transform.position, player.position) > detectionRange)
//        {
//            SetPatrolState();
//        }
//    }


//    private void AttackBehavior()
//    {
//        animator.SetBool("isChasing", false);  
//        animator.SetBool("isAttacking", true);  
//        Debug.Log("Attacking the player!");

//        if (Vector3.Distance(transform.position, player.position) > attackRange)
//        {
//            SetChaseState();
//        }
//    }


//    private void SetPatrolState()
//    {
//        currentState = State.Patrol;
//        animator.SetBool("isAttacking", false);  
//        UpdateState();
//    }

//    private void SetChaseState()
//    {
//        currentState = State.Chase;
//        animator.SetBool("isAttacking", false);  
//        UpdateState();
//    }

//    private void SetAttackState()
//    {
//        currentState = State.Attack;
//        UpdateState();
//    }


//    private void UpdateState()
//    {
//        animator.SetBool("isIdle", currentState == State.Idle);
//        animator.SetBool("isPatrolling", currentState == State.Patrol);
//        animator.SetBool("isChasing", currentState == State.Chase);
//        animator.SetBool("isAttacking", currentState == State.Attack);
//    }
//}
