using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float MinLaudiness = 5;
    public float MaxLaudiness = 20;
    public float Laudiness;
    private float CurrentLaudines;
    public LayerMask layer;
    public bool isProvoked;
    private void Start ( )
    {
        isProvoked = true;
    }
    public Transform Detection ( )
    {
        Transform target = null;
        if ( isProvoked )
        {
            CurrentLaudines += Time.deltaTime;
            if ( CurrentLaudines >= Laudiness)
            {
                isProvoked = false;
            }
        }
        Collider[] collider  = Physics.OverlapSphere ( transform.position , CurrentLaudines , layer, QueryTriggerInteraction.Collide );
        if ( collider.Length > 0 )
        {
            target = collider[0].gameObject.transform;
        }
        return target;
    }
    public void ResetLaudiness ( )
    {
        Laudiness = MinLaudiness;
        CurrentLaudines = Laudiness;
    }
    public void SetIsProvoked (bool  _isProvoked )
    {
        isProvoked = _isProvoked;
        Laudiness += 5;
        if ( Laudiness > MaxLaudiness)
        {
            Laudiness = MaxLaudiness;
        }
    }

    private void OnDrawGizmos ( )
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere ( transform.position , CurrentLaudines );
    }
}
