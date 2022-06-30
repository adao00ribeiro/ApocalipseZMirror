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
        public GameObject Target;
        public Vector3 TargetPosition;
        public Vector3 positionSpaw;

        Animator animatorController;
        IStats stats;
        NavMeshAgent NavMeshAgent;
        NavMeshPath path;
        

        [Header("INFORMACAO")]
        float distance;
        public float TimerResetPatrol;
        // Start is called before the first frame update
        void Start ( )
        {
            TimerResetPatrol = 9;
            path = new NavMeshPath();
            animatorController = GetComponent<Animator> ( );
            stats = GetComponent<EnemyStats> ( );
            NavMeshAgent = GetComponent<NavMeshAgent> ( );
            RandomSpeed ( );
            positionSpaw = transform.position;

        }
        private void FixedUpdate ( )
        {
            if ( stats.IsPlayerDead ( ) )
            {
                OnZombieIsDead?.Invoke ( );
                animatorController.SetLayerWeight ( 1 , 0 );
                animatorController.Play ( "Death" );
                NavMeshAgent.speed = 0;
                Timer.Instance.Add ( ( ) =>
                {

                    NetworkBehaviour.Destroy ( gameObject );

                } , 10 );
                CancelInvoke ( );
                NavMeshAgent.enabled = false;
                Target = null;
                this.enabled = false;
                return;
            }
            Animation ( );
            

            if ( Target == null && !stats.IsPlayerDead ( ) )
            {
                Collider[] collider  = Physics.OverlapSphere ( transform.position , Laudiness , layer, QueryTriggerInteraction.Collide );

                if ( collider.Length > 0 )
                {
                    Target = collider[0].gameObject;
                }
                TimerResetPatrol += Time.fixedDeltaTime;

                if (TimerResetPatrol >= 10 )
                {
                    Patrol ( positionSpaw + PositionRandom());
                    TimerResetPatrol = 0;
                }
                NavMeshAgent.speed = CurrentSpeed;
                NavMeshAgent.SetDestination ( TargetPosition );
            }
           
            if ( Target != null  )
            {
             
                distance = GetDistanceFrom( Target.transform.position , transform.position );
                
                NavMeshAgent.CalculatePath ( Target.transform.position , path );
                // Only walk if we can get all the way there
                if ( path.status == NavMeshPathStatus.PathComplete )
                {
                   
                    if ( distance <= NavMeshAgent.stoppingDistance )
                    {
                        // Calculate direction is toward player
                        Vector3 direction = Target.transform.position - this.transform.position;
                        float angle = Vector3.Angle(direction, this.transform.forward);

                        if ( angle <= 60f )
                        {
                            animatorController.SetLayerWeight ( 1 , 1 );
                            animatorController.Play ( "Attack" );
                        }
                    }
                    LookAt ( Target.transform.position );
                }
                else
                {
                    Target = null;
                }
                NavMeshAgent.speed = CurrentSpeed ;
                NavMeshAgent.SetDestination ( Target.transform.position );
                if ( Math.Abs ( NavMeshAgent.velocity.x + NavMeshAgent.velocity.y + NavMeshAgent.velocity.z ) < .05 )
                {
                    NavMeshAgent.speed = 0f;

                }
            }

        }
        float GetDistanceFrom ( Vector3 src , Vector3 dist )
        {
            return Vector3.Distance ( src , dist );
        }
        public Vector3 PositionRandom ( )
        {
            return Random.insideUnitSphere * 10;
        
        }
        public void RandomSpeed ( )
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
               CurrentSpeed = 0.3f;
            }
            else if ( MovimentState == MovimentState.RUN )
            {
                CurrentSpeed = 2.5f;
            }


        }
        public void FightHit ( )
        {
            IStats stats = Target.GetComponent<IStats> ( );
            if ( stats != null )
            {
                float distance = Vector3.Distance(Target.transform.position, transform.position);
                if ( distance <= NavMeshAgent.stoppingDistance )
                {
                    stats.TakeDamage ( 23 );
                }

                if ( stats.IsPlayerDead ( ) )
                {
                    Target = null;
                }
            }

        }

        void Patrol ( Vector3 randPos )
        {
            NavMeshHit hit;
            NavMesh.SamplePosition ( randPos , out hit , Random.Range(5,15) , NavMesh.AllAreas );
            TargetPosition = hit.position;
          
        }

        public void LookAt (Vector3 Target )
        {
            Quaternion newRotation = Quaternion.LookRotation(Target - transform.position);
            newRotation.x = 0f;
            newRotation.z = 0f;
            transform.rotation = Quaternion.Slerp ( transform.rotation , newRotation , Time.deltaTime * 10 );
        }

        public void Animation ( )
        {
            animatorController.SetFloat ( "Vertical" , NavMeshAgent.speed );
        }
        public void SetTarget (GameObject _target )
        {
            Target = _target;
        }
        private void OnDrawGizmos ( )
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine ( transform.position , transform.position + transform.forward * MaxDistance );
            Gizmos.DrawWireSphere ( transform.position + transform.forward * MaxDistance , Laudiness );
        }
    }

}