using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public bool IsAttacking;
    public Transform Target;
    public float stoppingDistance;

    public void SetTarget (Transform _target )
    {
        Target = _target;
    }
    public void SetstoppingDistance (float Distance )
    {
        stoppingDistance = Distance;
    }
    public void Attack ( )
    {
        if (Target == null  )
        {
            return;
        }
        if ( !IsAttacking )
        {
            IsAttacking = true;
        }
         
            new WaitForSeconds ( 0.5f );
            StartCoroutine ( ResetAttacking ( ) );
    }
    IEnumerator ResetAttacking ( )
    {
        yield return new WaitForSeconds ( 2f );

        IsAttacking = false;
      
        yield break;
    }
    public void FightHit ( )
    {
        if ( Target == null )
        {
            return;
        }
        float distanceFromTarget = Vector3.Distance(transform.position , Target.position);
        if ( distanceFromTarget <= stoppingDistance )
        {
            Target.GetComponent<IStats> ( ).TakeDamage ( ( int ) GetComponent<IStats> ( ).GetDamage ( ) );
        }
      
    }
}
