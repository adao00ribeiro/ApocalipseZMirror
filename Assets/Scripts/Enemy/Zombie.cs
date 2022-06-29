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
        public MovimentState MovimentState;
        public float Laudiness;
	    public float MaxDistance;
        
	  
	     
        public LayerMask layer;
        public GameObject Target;
	    Vector3 TargetPosition;
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
	        RandomSpeed ( );
	        positionSpaw = transform.position;
	        TargetPosition = positionSpaw;
	        NavMeshAgent.updateRotation = false;
	        NavMeshAgent.updatePosition = true;
            InvokeRepeating ( "Behavior" , 10 , 10 );

        }
	    public void MoveRandom()
	    {
	    	Vector3 position = Random.insideUnitSphere * 10;
	    	position += positionSpaw;
	    	TargetPosition = position;
	    
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

            if ( MovimentState  == MovimentState.WALK)
            {
                NavMeshAgent.speed = 0.5f;
            }else if ( MovimentState == MovimentState.RUN)
            {
                NavMeshAgent.speed = 2.5f;
            }
           

        }
	  
	    void Patrol(Vector3 randPos)
	    {
		
		    NavMeshHit hit;
		    NavMesh.SamplePosition(randPos, out hit, Random.Range(2, 8), 1);
		    Vector3 finalPosition = hit.position;
            NavMeshAgent.SetDestination(finalPosition);
		    transform.LookAt( NavMeshAgent.destination,Vector3.up);
	    }
	 
	    
	    public void Behavior ( )
        {
            if ( Target == null )
            {
                MoveRandom ( );
                Patrol ( TargetPosition );
            }
        }
        private void FixedUpdate ( )
        {
         
            Animation ( );
            if ( stats.IsPlayerDead ( ) )
            {
                animatorController.SetLayerWeight ( 1 , 0 );
                animatorController.Play ( "Death" );
                NavMeshAgent.speed = 0;
                Timer.Instance.Add (()=> {

                    SpawEnemy.Instance.Spawn (ScriptableManager.Instance.GetPrefabEnemy(TypeEnemy.ZOMBIE) , 1);
                    Destroy ( gameObject );
                    
                },10);
                CancelInvoke();
                NavMeshAgent.enabled = false;
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
	        
            if ( Target != null )
            {
                             
                    float distance = Vector3.Distance(Target.transform.position, transform.position);

                    TargetPosition = Target.transform.position;

                    if ( distance <= NavMeshAgent.stoppingDistance )
                    {
                        animatorController.SetLayerWeight ( 1 , 1 );
                        animatorController.Play ( "Attack" );
                    }

                LookAt ( );
                NavMeshAgent.SetDestination ( TargetPosition );

            }

        }
        public void LookAt ( )
        {
            Quaternion newRotation = Quaternion.LookRotation(Target.transform.position - transform.position);
            newRotation.x = 0f;
            newRotation.z = 0f;
            transform.rotation = Quaternion.Slerp ( transform.rotation , newRotation , Time.deltaTime * 10 );
        }
        public void Animation ( )
        {
            animatorController.SetFloat ( "Vertical" , NavMeshAgent.velocity.magnitude );
        }
   

        private void OnDrawGizmos ( )
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine ( transform.position , transform.position + transform.forward * MaxDistance );
            Gizmos.DrawWireSphere ( transform.position + transform.forward * MaxDistance , Laudiness );
        }
    }

}