using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{
    public float SpeedWalk;
    public float SpeedRun;
    public float CurrentSpeed;

    private void Start ( )
    {
        CurrentSpeed = SpeedRun;
    }
    public void RandomSpeed ( )
    {

    }
    public void MoveToTarget ( NavMeshAgent agent ,Vector3 Target)
    {
        agent.speed = CurrentSpeed;
        agent.SetDestination ( Target );
    }
    public void SetAgentSpeedWalk (   )
    {
        CurrentSpeed = SpeedWalk;
    }
    public void SetAgentSpeedRun (  )
    {
        CurrentSpeed = SpeedRun;
    }
}
