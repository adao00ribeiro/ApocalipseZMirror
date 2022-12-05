using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;
using FishNet.Object;

namespace ApocalipseZ
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class Zombie : NetworkBehaviour
    {
        public event Action OnZombieIsDead;

        private Animator animator;
        private NavMeshAgent agent;
        NavMeshPath path;

        IStats stats;
        //components
        EnemyPatrol Patrol;
        EnemyDetection Detection;
        EnemyAttack Attack;
        EnemyAnimation Animation;
        EnemyChase chase;
        //target
        public List<MonoBehaviour> listMono = new List<MonoBehaviour>();
        public Vector3 TargetPosition;
        public AudioClip zombieRoar;

        private void Start()
        {
            stats = GetComponent<IStats>();
            Patrol = GetComponent<EnemyPatrol>();
            Detection = GetComponent<EnemyDetection>();
            Attack = GetComponent<EnemyAttack>();
            Animation = GetComponent<EnemyAnimation>();
            chase = GetComponent<EnemyChase>();
            listMono.Add(Patrol);
            listMono.Add(Detection);
            listMono.Add(Attack);
            listMono.Add(chase);
            agent = GetComponent<NavMeshAgent>();
            path = new NavMeshPath();
            animator = GetComponent<Animator>();
            agent.angularSpeed = 999;
            if (base.IsClient)
            {
                Destroy(Patrol);
                Destroy(Detection);
                Destroy(Attack);
                Destroy(Animation);
                Destroy(chase);
            }
        }

        private void FixedUpdate()
        {
            if (base.IsServer)
            {
                if (stats.IsDead() && Animation.type != EnemyMovimentType.DEFAULT)
                {
                    Animation.SetType(EnemyMovimentType.DIE);
                    OnZombieIsDead?.Invoke();
                    GameController.Instance.TimerManager.Add(() =>
                    {
                        base.Despawn();
                    }, 10);
                    DisablesAllMonos();
                    agent.speed = 0;
                    enabled = false;
                    return;
                }
                if (Detection.target)
                {
                    if (Detection.target.GetComponent<IStats>().IsDead())
                    {
                        DisablesAllMonos();
                        Animation.enabled = true;
                        Patrol.enabled = true;
                        Detection.enabled = true;
                        Detection.target = null;
                        Attack.target = null;
                        chase.Target = null;
                    }
                }

                if (agent.velocity.x == 0 && agent.velocity.z == 0 && !Attack.IsAttacking)
                {
                    Animation.type = EnemyMovimentType.IDLE;
                }
                if (agent.velocity.x != 0 || agent.velocity.z != 0 && !Attack.IsAttacking)
                {
                    Animation.type = EnemyMovimentType.WALK;
                }
                if (Attack.IsAttacking)
                {
                    Animation.type = EnemyMovimentType.ATACK;
                }

                if (Detection.IsDetection)
                {
                    DisablesAllMonos();
                    chase.Target = Detection.target;
                    Attack.target = Detection.target;
                    Animation.enabled = true;
                    chase.enabled = true;
                    Attack.enabled = true;
                }

            }
        }
        public void DisablesAllMonos()
        {
            foreach (var item in listMono)
            {
                item.enabled = false;
            }
        }
        private void FaceTarget()
        {
            // Vector3 direction = (target.position - transform.position).normalized;
            // Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            // transform.rotation = Quaternion.Slerp ( transform.rotation , lookRotation , Time.deltaTime * turnSpeed );
        }

    }
}