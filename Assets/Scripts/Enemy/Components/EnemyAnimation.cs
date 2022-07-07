using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyMovimentType
{
    IDLE,
    WALK,
    RUN,
    ATACK,
    DIE
}
public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start ( )
    {
     
        animator = GetComponent<Animator> ( );
    }
    public void Animation ( EnemyMovimentType type)
    {
        if ( type == EnemyMovimentType.DIE )
        {
            animator.SetLayerWeight ( 1 , 0 );
            animator.Play ( "Death" );
            return;
        }
        if ( type == EnemyMovimentType.ATACK )
        {
            animator.SetLayerWeight ( 1 , 1 );
            animator.Play ( "Attack" );
        }
        animator.SetBool ( "Walk" , type == EnemyMovimentType.WALK);
        animator.SetBool ( "Run" , type == EnemyMovimentType.RUN );

        
    }
    
}
