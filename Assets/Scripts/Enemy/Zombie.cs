using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{

    NavMeshAgent NavMeshAgent;
    public Vector3 positionSpaw;
    public Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent> ( );
        positionSpaw = transform.position;
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.updatePosition = true;
    }
    private void Update ( )
    {
        if ( target != Vector3.zero )
            NavMeshAgent.SetDestination ( target );

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
