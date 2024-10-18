using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_behavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public float safeDistance = 2.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // If the enemy is within the safe distance, move away from the player
        if (distanceToPlayer < safeDistance)
        {
            Vector3 moveDirection = (transform.position - player.position).normalized;
            Vector3 targetPosition = player.position + moveDirection * safeDistance;
            agent.SetDestination(targetPosition);
        }
        else
        {
            // If outside safe distance, move towards the player
            agent.SetDestination(player.position);
        }
    }
}
