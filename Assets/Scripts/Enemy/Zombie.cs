using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;
namespace ApocalipseZ
{
    [RequireComponent ( typeof ( NavMeshAgent ) )]
    [RequireComponent ( typeof ( EnemyMove ) )]
    [RequireComponent ( typeof ( EnemyDetection ) )]
    [RequireComponent ( typeof ( EnemyPatrol ) )]
    [RequireComponent ( typeof ( EnemyAttack ) )]
    [RequireComponent ( typeof ( EnemyAnimation ) )]
    public class Zombie : NetworkBehaviour
    {
        public event Action OnZombieIsDead;
        public EnemyMovimentType Type;
        private EnemyMove Move;
        private NavMeshAgent agent;
        NavMeshPath path;
        private EnemyDetection EnemyDetection;
        private EnemyPatrol EnemyPatrol;
        private IStats stats;
        private Animator animator;
        private EnemyAttack EnemyAtack;
        private EnemyAnimation EnemyAnimation;
        public Transform Target;
        public Vector3 TargetPosition;
        private void Start ( )
        {
            Move = GetComponent<EnemyMove> ( );
            agent = GetComponent<NavMeshAgent> ( );
            path = new NavMeshPath ( );
            EnemyDetection = GetComponent<EnemyDetection> ( );
            EnemyPatrol = GetComponent<EnemyPatrol> ( );
            stats = GetComponent<IStats> ( );
            animator = GetComponent<Animator> ( );
            EnemyAtack = GetComponent<EnemyAttack> ( );
            EnemyAtack.SetstoppingDistance ( agent.stoppingDistance );
            EnemyAnimation = GetComponent<EnemyAnimation> ( );
            agent.angularSpeed = 999;
        }
        private void FaceTarget ( )
        {
           // Vector3 direction = (target.position - transform.position).normalized;
           // Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
           // transform.rotation = Quaternion.Slerp ( transform.rotation , lookRotation , Time.deltaTime * turnSpeed );
        }
        private void FixedUpdate ( )
        {
            EnemyAnimation.Animation ( Type );

            if ( stats.IsPlayerDead ( ) )
            {
                OnZombieIsDead?.Invoke ( );
                Type = EnemyMovimentType.DIE;
                EnemyAnimation.Animation ( Type );
                agent.speed = 0;
                Timer.Instance.Add ( ( ) =>
                {
                    NetworkBehaviour.Destroy ( gameObject );
                } , 10 );
                agent.enabled = false;
                Target = null;
                this.enabled = false;
                return;
            }
            if ( Target != null && EnemyPatrol.ItsFarFrinSpawPoint ( ) )
            {
                Target = null;
                EnemyPatrol.TimerResetPatrol = 20;
            }

            if ( Target == null && !stats.IsPlayerDead ( ) )
            {
                TargetPosition = EnemyPatrol.Patrol ( TargetPosition );
                Target = EnemyDetection.Detection ( );
                agent.stoppingDistance = 0f;

            }
            if ( Target != null )
            {
                TargetPosition = Target.position;
                agent.stoppingDistance = 1.5f;
                EnemyAtack.SetTarget ( Target );
            }

            if ( !EnemyAtack.IsAttacking )
            {

                if ( Vector3.Distance ( transform.position , TargetPosition ) <= agent.stoppingDistance )
                {
                    Type = EnemyMovimentType.IDLE;
                    if ( Target )
                    {
                        EnemyAtack.Attack ( );
                        agent.speed = 0;
                        Type = EnemyMovimentType.ATACK;
                    }
                }
                else
                {
                    agent.CalculatePath ( TargetPosition , path );

                    if ( path.status == NavMeshPathStatus.PathComplete )
                    {
                        Move.SetAgentSpeedRun ( );
                        Type = Move.CurrentSpeed == Move.SpeedWalk ? EnemyMovimentType.WALK : EnemyMovimentType.RUN;
                        Move.MoveToTarget ( agent , TargetPosition );
                    }
                    else
                    {
                        agent.speed = 0;
                        Type = EnemyMovimentType.IDLE;
                        Target = null;
                    }


                }


            }


        }

    }
}