using UnityEngine;

public class EnemyV5AnimCtrr : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWalkingState(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    public void TriggerAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void TriggerDyingAnimation()
    {
        animator.SetTrigger("Die");
    }
}