using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
namespace ApocalipseZ
{
    public enum MovimentState
    {
        WALK, RUN, RANDOM
    }
    [RequireComponent ( typeof ( NavMeshAgent ) )]
    public class Zombie : NetworkBehaviour
    {
        public float Laudiness;
        public float MaxDistance;
        public LayerMask layer;
        public GameObject Target;

        Animator animatorController;
        IStats stats;
        NavMeshAgent NavMeshAgent;
       
        public Vector3 positionSpaw;

        // Start is called before the first frame update
        void Start ( )
        {
            animatorController = GetComponent<Animator> ( );
            stats = GetComponent<EnemyStats> ( );
            NavMeshAgent = GetComponent<NavMeshAgent> ( );
            positionSpaw = transform.position;
            NavMeshAgent.updateRotation = false;
            NavMeshAgent.updatePosition = true;
        }

        private void Update ( )
        {
            if ( stats.IsPlayerDead ( ) )
            {
                animatorController.SetLayerWeight ( 1 , 0 );
                animatorController.Play ( "Death" );
                NavMeshAgent.speed = 0;
                this.enabled = false;
            }

            if ( Target == null )
            {
                Collider[] collider  = Physics.OverlapSphere ( transform.position , Laudiness , layer, QueryTriggerInteraction.Collide );

                if ( collider.Length > 0 )
                {
                    Target = collider[0].gameObject;
                }
            }


            if ( Target == null )
            {
                Move ( positionSpaw , false , false );
               
            }
            else
            {
                float distance = Vector3.Distance(Target.transform.position, transform.position);
               
                Move ( Target.transform.position , false , false );
                transform.LookAt ( Target.transform , Vector3.up);

                if ( distance <= NavMeshAgent .stoppingDistance)
                {
                    animatorController.SetLayerWeight ( 1,1);
                    animatorController.Play ( "Attack" );
                }
            }
           
           
            
        }


        public void Move ( Vector3 move , bool crouch , bool jump )
        {
            animatorController.SetFloat ( "Vertical" , NavMeshAgent.velocity.magnitude );
           
            NavMeshAgent.SetDestination ( move );
           
         
          
        }

        private void OnDrawGizmos ( )
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine ( transform.position , transform.position + transform.forward * MaxDistance );
            Gizmos.DrawWireSphere ( transform.position + transform.forward * MaxDistance , Laudiness );
        }
    }

}