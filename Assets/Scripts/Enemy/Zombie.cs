using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
namespace ApocalipseZ
{
    [RequireComponent ( typeof ( NavMeshAgent ) )]
    public class Zombie : NetworkBehaviour
    {
        Animator animatorController;
        PlayerStats stats;
        NavMeshAgent NavMeshAgent;
        public Vector3 positionSpaw;
        public Vector3 target;
        // Start is called before the first frame update
        void Start ( )
        {
            animatorController = GetComponent<Animator> ( );
            stats = GetComponent<PlayerStats> ( );
            NavMeshAgent = GetComponent<NavMeshAgent> ( );
            positionSpaw = transform.position;
            NavMeshAgent.updateRotation = false;
            NavMeshAgent.updatePosition = true;
        }

        private void Update ( )
        {
            if ( target != Vector3.zero )
                NavMeshAgent.SetDestination ( target );
            if ( stats.IsPlayerDead( ) )
            {
                animatorController.SetLayerWeight ( 1 , 0 );
                animatorController.Play ( "Death" );
                NavMeshAgent.speed = 0;
                this.enabled = false;
            }
            try
            {
                if ( NavMeshAgent.remainingDistance > NavMeshAgent.stoppingDistance )
                {

                    Move ( NavMeshAgent.desiredVelocity , false , false );

                }
                else
                {

                    Move ( Vector3.zero , false , false );

                }
            }
            catch
            {

            }
        }
        public void Move ( Vector3 move , bool crouch , bool jump )
        {
            NavMeshAgent.SetDestination ( move );
        }

        public void SetTarget ( Vector3 target )
        {
            this.target = target;
        }

        private void OnTriggerEnter ( Collider other )
        {
            target = other.transform.position;
        }
    }

}