using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    
    public float Radius;
    public Vector3 positionSpaw;
    public float TimerResetPatrol;

    public float DistanceSpaw;
    private void Start ( )
    {
        positionSpaw = transform.position;
    }
    public Vector3 Patrol (Vector3 TargetPosition)
    {
        TimerResetPatrol += Time.fixedDeltaTime;
        Vector3 position =TargetPosition;
        if ( TimerResetPatrol >= 10 )
        {
            position = RandomNavmeshLocation ( Radius );
            position.y = transform.position.y;
            Debug.DrawLine ( position, position + Vector3.up * 2,Color.blue , 10);
            TimerResetPatrol = 0;
        }
        return position;
    }
    public Vector3 RandomNavmeshLocation ( float radius )
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += positionSpaw;
        UnityEngine.AI.NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if ( UnityEngine.AI.NavMesh.SamplePosition ( randomDirection , out hit , radius , 1 ) )
        {
            finalPosition = hit.position;
        }
        else
        {
            finalPosition = positionSpaw;
        }
        return finalPosition;
    }
    public bool ItsFarFrinSpawPoint ( )
    {
       
        return Vector3.Distance ( transform.position , positionSpaw ) > DistanceSpaw; 
    }
}
