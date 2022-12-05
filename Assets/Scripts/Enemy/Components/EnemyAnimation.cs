
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
public enum EnemyMovimentType
{
    DEFAULT,
    IDLE,
    WALK,
    RUN,
    ATACK,
    DIE
}
public class EnemyAnimation : NetworkBehaviour
{
    private Animator animator;
    public EnemyMovimentType type;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {

        Animation();

    }
    private void Animation()
    {
        if (type == EnemyMovimentType.DEFAULT)
        {
            return;
        }
        if (type == EnemyMovimentType.DIE)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetTrigger("IsDeath");
            type = EnemyMovimentType.DEFAULT;
            return;
        }
        if (type == EnemyMovimentType.ATACK)
        {
            animator.SetLayerWeight(1, 1);
        }
        animator.SetBool("IsAttack", type == EnemyMovimentType.ATACK);
        animator.SetBool("Walk", type == EnemyMovimentType.WALK);
        animator.SetBool("Run", type == EnemyMovimentType.RUN);
    }
    public void SetType(EnemyMovimentType _type)
    {
        type = _type;
    }

}
