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
    [RequireComponent(typeof(EnemyChase))]
    public class Zombie : NetworkBehaviour
    {
        public event Action OnZombieIsDead;

        public EnemyMovimentType Type;

        private Animator animator;
        private NavMeshAgent agent;
        NavMeshPath path;

        IStats stats;
        //components
         EnemyPatrol      Patrol;
         EnemyDetection   Detection;
         EnemyAttack      Attack;
         EnemyAnimation   Animation;
         EnemyChase chase;


        public List<MonoBehaviour> listMono = new List<MonoBehaviour>();
        //target
        public Transform Target;
        public Vector3 TargetPosition;
        public AudioClip zombieRoar;

        private void Start ( )
        {
            if (isServer)
            {
                stats = GetComponent<IStats> ( );
                Patrol = GetComponent<EnemyPatrol> ( );
                Detection = GetComponent<EnemyDetection> ( );
                Attack = GetComponent<EnemyAttack> ( );
                Animation = GetComponent<EnemyAnimation> ( );
                agent = GetComponent<NavMeshAgent> ( );
                chase = GetComponent<EnemyChase>();
                listMono.Add(Patrol);
                listMono.Add(Detection);
                listMono.Add(Attack);
                listMono.Add(chase);
                path = new NavMeshPath ( );
                animator = GetComponent<Animator> ( );
                agent.angularSpeed = 999;
            }
            else
            {
                Destroy ( GetComponent<EnemyPatrol> ( ) );
                Destroy ( GetComponent<EnemyDetection> ( ) );
               
            }
            
          
        }
    
        private void FixedUpdate ( )
        {
            if (!isServer)
            {
                return;
            }
            if ( stats.IsDead())
            {
                OnZombieIsDead?.Invoke ( );
                Timer.Instance.Add ( ( ) =>
                {
                    NetworkBehaviour.Destroy ( gameObject );
                } , 10 );
                DisablesAllMonos();
                Animation.SetType ( Type = EnemyMovimentType.DIE );
                agent.speed = 0;
                enabled = false;
                return;
            }
            if (Detection.target)
            {
                if (Detection.target.GetComponent<IStats>().IsDead())
                {
                    DisablesAllMonos();
                    Patrol.enabled = true;
                    Detection.enabled = true;
                    Detection.target = null;
                    Attack.target = null;
                    chase.Target = null;
                }
            }
            if (agent.velocity.x == 0 && agent.velocity.z == 0 && !Attack.IsAttacking)
            {
                Type = EnemyMovimentType.IDLE;
            }
            if (agent.velocity.x != 0 || agent.velocity.z != 0 && !Attack.IsAttacking)
            {
                Type = EnemyMovimentType.RUN;
            }
            if (Attack.IsAttacking)
            {
                Type = EnemyMovimentType.ATACK;
            }

            if (Detection.IsDetection)
            {
                DisablesAllMonos();
                chase.Target = Detection.target;
                Attack.target = Detection.target;
                chase.enabled = true;
                Attack.enabled = true;
            }

            Animation.SetType ( Type );
        }
        public void DisablesAllMonos()
        {
            foreach (var item in listMono)
            {
                item.enabled = false;
            }
        }

        private void FaceTarget ( )
        {
            // Vector3 direction = (target.position - transform.position).normalized;
            // Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            // transform.rotation = Quaternion.Slerp ( transform.rotation , lookRotation , Time.deltaTime * turnSpeed );
        }

    }
}