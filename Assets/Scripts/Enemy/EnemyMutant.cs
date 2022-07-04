using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
[RequireComponent ( typeof ( Animator ) )]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMutant : MonoBehaviour
{
    [Header("Detection")]
    public Transform Target;
    public float Laudiness;
    [Header("NavMesh")]
    NavMeshPath path;
    private NavMeshAgent agent;
    float CurrentSpeed;
    public LayerMask layer;
    private Vector3 TargetPosition;
    public Vector3 positionSpaw;
    private float TimerResetPatrol;
    private Animator animator;

    private bool isAttacking;
    // Start is called before the first frame update
    void Start()
    {
        TimerResetPatrol = 9;

        path = new NavMeshPath ( );
        agent = GetComponent<NavMeshAgent> ( );
        animator = GetComponent<Animator> ( );
        positionSpaw = transform.position;
        agent.angularSpeed = 999;
        agent.speed = CurrentSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        DetectionTarget ( );

        if ( !isAttacking )
        {
            MoveToTarget ( );
        }
        Animation ( );
    }
    public void DetectionTarget ( )
    {
        if ( Target == null  )
        {
            Collider[] collider  = Physics.OverlapSphere ( transform.position , Laudiness , layer, QueryTriggerInteraction.Collide );

            if ( collider.Length > 0 )
            {
                Target = collider[0].transform;
            }

            TimerResetPatrol += Time.fixedDeltaTime;

            if ( TimerResetPatrol >= 10 )
            {
                TargetPosition = positionSpaw + Random.insideUnitSphere * 5;
                TargetPosition.y = 0;
                TimerResetPatrol = 0;
            }
        }
        else
        {
            TargetPosition = Target.transform.position;
        }
    }
    private void MoveToTarget ( )
    {
        agent.SetDestination ( TargetPosition );

    }
    public void Animation ( )
    {
        animator.SetBool ( "Walk" , agent.speed == 0.2f );
        animator.SetBool ( "Run" , agent.speed == 2.5f );
    }
}
