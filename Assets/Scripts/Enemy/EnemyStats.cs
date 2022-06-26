using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace ApocalipseZ
{
    public class EnemyStats : NetworkBehaviour, IStats
    {
        public int Damage;
        public int health;

        public bool IsPlayerDead ( )
        {
            return health <= 0;
        }

        public void RestoreLife ( int life )
        {
            health += life;

            if ( health > 100 )
            {
                health = 100;
            }

        }
        public void TakeDamage ( int damage )
        {
            health -= damage;
            print ( "take enemy" );
            if ( health < 0 )
            {
                health = 0;
                SpawEnemy.Instance.Spawn ( ScriptableManager.Instance.GetPrefabEnemy ( TypeEnemy.ZOMBIE ) , 1 );
            }
        }
        [Command ( requiresAuthority = false )]
        public void CmdTakeDamage ( int damage , NetworkConnectionToClient sender = null )
        {
            TakeDamage ( damage );
        }
    }
}