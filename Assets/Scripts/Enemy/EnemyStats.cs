using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace ApocalipseZ
{
    public class EnemyStats : NetworkBehaviour, IStats
    {
        [SerializeField]private int Damage;
        [SerializeField]private int health;
        
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
            GetComponent<EnemyDetection> ( ).SetIsProvoked (true);
            if ( health < 0 )
            {
                health = 0;
               
            }
        }
        [Command ( requiresAuthority = false )]
        public void CmdTakeDamage ( int damage , NetworkConnectionToClient sender = null )
        {
            TakeDamage ( damage );
        }

        public float GetDamage ( )
        {
            return Damage;
        }
    }
}