using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
namespace ApocalipseZ
{
    public class PlayerStats : NetworkBehaviour, IStats
    {
        public event Action OnAlteredStats;
        [SyncVar(hook = nameof(SetHealth))]
        public int health;
        FpsPlayer player;
        private void SetHealth ( int oldHealth , int newHealth )
        {
            health = newHealth;
            OnAlteredStats?.Invoke ( );
        }
        private void Start ( )
        {
            player = GetComponent<FpsPlayer> ( );
        }
        void Update ( )
        {

            if ( !isLocalPlayer )
            {
                return;
            }
            if ( IsPlayerDead ( ) )
            {
                CmdPlayerDeath ( );
            }
            if ( transform.position.y < -11.1 && !IsPlayerDead ( ) )
            {
                CmdTakeDamage ( 200 );
            }
        }

        [Command ( requiresAuthority = false )]
        public void CmdTakeDamage ( int damage , NetworkConnectionToClient sender = null )
        {
            TakeDamage ( damage );
        }
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
            OnAlteredStats?.Invoke ( );
        }
        public void TakeDamage ( int damage )
        {
            health -= damage;

            if ( health < 0 )
            {
                health = 0;
            }
            OnAlteredStats?.Invoke ( );
        }
        private void PlayerDeath ( )
        {
            player.DroppAllItems ( );
            StartCoroutine ( Respawn ( ) );
        }
        private IEnumerator Respawn ( )
        {
            yield return new WaitForSeconds ( 5f );
            transform.position = PlayerSpawPoints.Instance.GetPointSpaw ( );
            RestoreLife ( 200 );
        }
        [Command ( requiresAuthority = false )]
        public void CmdPlayerDeath ( NetworkConnectionToClient sender = null )
        {
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            PlayerStats stats =  sender.identity.GetComponent<PlayerStats> ( );
            stats.PlayerDeath ( );
            sender.identity.GetComponent<FpsPlayer> ( ).GetWeaponManager ( ).TargetDesEquipWeapon ( opponentIdentity.connectionToClient );
            sender.identity.GetComponent<FpsPlayer> ( ).TargetRespaw ( opponentIdentity.connectionToClient );
        }


    }
}