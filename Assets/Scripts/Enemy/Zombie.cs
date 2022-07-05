using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;
namespace ApocalipseZ
{
    public enum MovimentState
    {
        WALK, RUN, RANDOM
    }
    [RequireComponent ( typeof ( NavMeshAgent ) )]
    public class Zombie : NetworkBehaviour
    {
        public event Action OnZombieIsDead;
        public MovimentState MovimentState;
        public float Laudiness;
        public float MaxDistance;

        float CurrentSpeed;
        public LayerMask layer;
        public Vector3 TargetPosition;
        public Vector3 positionSpaw;


        IStats stats;
        NavMeshPath path;


        [Header("INFORMACAO")]

        public float TimerResetPatrol;
        IEnumerator distUpdateCo = null;

        //outro

        private Animator animator;

        //private AudioSource audioSource;

        public GameObject Target;// test
        public float Damage = 100.0f;// test

        [SerializeField] private bool isAttacking = false;
        [SerializeField] private float maxSeeDistance = 20f;

        [SerializeField] private bool isInLateUpdate;
        [SerializeField] private bool haveToUpdate = true;
        private NavMeshAgent agent;
        //private EnemyUnit agent; TODO It doesnt work with astar yet, so I need to fix it for the summative assignment
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AudioClip diengSound;

        [SerializeField] private float normalSpeed = 0.2f;
        [SerializeField] private float runSpeed = 2.5f;
        //[SerializeField] private float currentSpeed;
        [SerializeField] private GameObject noChaseObj;
        [SerializeField] private Vector3 notChaseTarget;
        [SerializeField]private float distance;

        // Start is called before the first frame update
        void Start ( )
        {
            TimerResetPatrol = 9;
            path = new NavMeshPath ( );
            animator = GetComponent<Animator> ( );
            stats = GetComponent<EnemyStats> ( );
            agent = GetComponent<NavMeshAgent> ( );
            positionSpaw = transform.position;
            RandomMovimentState ( );
            agent.angularSpeed = 999;
            agent.speed = CurrentSpeed;
        }
        [Server]
        private void FixedUpdate ( )
        {
            if ( stats.IsPlayerDead ( ) )
            {
                OnZombieIsDead?.Invoke ( );
                animator.SetLayerWeight ( 1 , 0 );
                animator.Play ( "Death" );
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
            Detection ( );

            float distanceToPlayer = GetActualDistanceFromTarget();
            distance = distanceToPlayer;
            agent.CalculatePath ( TargetPosition , path );

            if ( !isAttacking)
            {
                if (  distance > 2 )
                {
                    agent.speed = CurrentSpeed;
                }
                 else
                {
                    agent.speed = 0;
                }
                if ( path.status == NavMeshPathStatus.PathComplete )
                {
                    if ( Target != null && distanceToPlayer <= 2.0f )
                    {
                        Attack ( );
                    }
                   
                }
                if ( Target != null )
                {
                    if ( Target.GetComponent<IStats> ( ).IsPlayerDead ( ) )
                    {
                        Target = null;
                    }
                }
               
                MoveToTarget ( );
            }
           
            Animation ( );
        }
        public void FightHit ( )
        {
            float distanceFromTarget = GetActualDistanceFromTarget();

            // Calculate direction is toward player
            Vector3 direction = Target.transform.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);

            if (  distanceFromTarget <= 5.0f && angle <= 60f )
             {
                Target.GetComponent<IStats> ( ).TakeDamage ( ( int ) Damage );
            }
          
        }
        public void RandomMovimentState ( )
        {
            if ( MovimentState == MovimentState.RANDOM )
            {
                System.Type tipo = typeof ( MovimentState );
                System.Array values = System.Enum.GetValues(tipo);
                //Array values = Enum.GetValues(type);
                MovimentState = ( MovimentState ) values.GetValue ( Random.Range ( 0 , values.Length - 1 ) );
            }

            if ( MovimentState == MovimentState.WALK )
            {
                CurrentSpeed = 0.2f;
            }
            else if ( MovimentState == MovimentState.RUN )
            {
                CurrentSpeed = 2.5f;
            }


        }
        private void MoveToTarget ( )
        {
            agent.SetDestination ( TargetPosition );
         
        }
        public void Animation ( )
        {
            animator.SetBool ( "Walk" , agent.speed == 0.2f);
            animator.SetBool ( "Run" , agent.speed == 2.5f);
        }
        public void Detection ( )
        {
            if ( Target == null && !stats.IsPlayerDead ( ) )
            {
                Collider[] collider  = Physics.OverlapSphere ( transform.position , Laudiness , layer, QueryTriggerInteraction.Collide );

                if ( collider.Length > 0 )
                {
                    Target = collider[0].gameObject;
                }
                TimerResetPatrol += Time.fixedDeltaTime;

                if ( TimerResetPatrol >= 10 )
                {
                    TargetPosition = RandomNavmeshLocation ( 5);
                    TargetPosition.y = transform.position.y;
                    TimerResetPatrol = 0;
                }
            }
            else
            {
                TargetPosition = Target.transform.position;
            }
        }
        public Vector3 RandomNavmeshLocation ( float radius )
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += positionSpaw;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if ( NavMesh.SamplePosition ( randomDirection , out hit , radius , 1 ) )
            {
                finalPosition = hit.position;
            }
            else
            {
                finalPosition = positionSpaw;
            }
            return finalPosition;
        }
        private void Attack ( )
        {
            // Calculate actual distance from target
            float distanceFromTarget = GetActualDistanceFromTarget();

            // Calculate direction is toward player
            Vector3 direction = Target.transform.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);

            if ( !isAttacking && distanceFromTarget <= 5.0f && angle <= 60f )
            {
                isAttacking = true;
                agent.speed = 0;
            
                animator.SetLayerWeight ( 1 , 1 );
                //audioSource.PlayOneShot(attackSound);
                animator.Play ( "Attack" );
                
                new WaitForSeconds ( 0.5f );
                StartCoroutine ( ResetAttacking ( ) );
            }
        }

        IEnumerator LateDistanceUpdate ( float duration )
        {
            isInLateUpdate = true;
            agent.destination = Target.transform.position;
            yield return new WaitForSeconds ( duration );
            isInLateUpdate = false;
            distUpdateCo = null;
            yield break;
        }

        float GetActualDistanceFromTarget ( )
        {
            return GetDistanceFrom ( TargetPosition , this.transform.position );
        }

        float GetDistanceFrom ( Vector3 src , Vector3 dist )
        {
            return Vector3.Distance ( src , dist );
        }

        IEnumerator ResetAttacking ( )
        {
            yield return new WaitForSeconds ( 2.4f );

            isAttacking = false;

            if ( !stats.IsPlayerDead ( ) )
            {
                agent.speed = CurrentSpeed;
            }
            yield break;
        }
     
    }
}