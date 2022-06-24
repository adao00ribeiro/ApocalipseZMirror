using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
namespace ApocalipseZ
{
    public class PlayerStats : NetworkBehaviour
    {
        public event Action OnAlteredStats;

        [SyncVar(hook = nameof(SetOnAltered))]
        public int health;
                
        public bool isPlayerDead = false;
        
        void SetOnAltered ( int oldhealthHook , int newhealthHook )
        {
          
            OnAlteredStats?.Invoke ( );
        }

        // Start is called before the first frame update
        void Start ( )
        {
           // HealthSlider = GameObject.Find ( "Canvas Main/HealthSlider" ).GetComponent<Slider> ( );
            //HealthText = GameObject.Find ( "Canvas Main/HealthSlider/HealthText" ).GetComponent<Text> ( );
        }
	
        // Update is called once per frame
        void Update ( )
        {
            if ( !isClient )
            {
                return;
            }
            if (transform.position.y < -11.1  && !isPlayerDead )
            {
                TakeDamage ( 200);
            }
        }

        [Command ( requiresAuthority = false )]
        public void CmdTakeDamage (int damage ,  NetworkConnectionToClient sender = null)
        {
            TakeDamage ( damage );
            OnAlteredStats?.Invoke ( );
           
        }

        public void RestoreLife ( int life )
        {
            health += life;
            if ( health > 100 )
            {
                health = 100;
            }
        }
        public void TakeDamage (int  damage)
        {
            health -= damage;
            if ( health <= 0 && !isPlayerDead )
            {
                if ( GetComponent<FpsPlayer> ( ) )
                {
                    CmdPlayerDeath ( );
                }
                else
                {
                    PlayerDeath ( );
                }
               
            }
            if ( health < 0 )
            {
                health = 0;
            }
        }
        private void PlayerDeath ( )
        {
            isPlayerDead = true;
            if ( GetComponent<FpsPlayer> ( ) )
            {
                GetComponent<FpsPlayer> ( ).CmdDropAllItems ( );
                StartCoroutine ( Respawn ( ) );
            }
            else
            {
                //zumbi timer respaw
            }
            
        }
        private IEnumerator Respawn ( )
        {
            yield return new WaitForSeconds ( 5f );
            FpsPlayer player = GetComponent<FpsPlayer> ( );
            if ( player )
            {
                transform.position = PlayerSpawPoints.Instance.GetPointSpaw ( );
                transform.rotation = Quaternion.identity;
                RestoreLife ( 200 );
                 isPlayerDead = false;
                player.Respaw ( );
            }
        }
        [Command ( requiresAuthority = false )]
        public void CmdPlayerDeath(  NetworkConnectionToClient sender = null )
        {
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();

            TargetPlayerDeath ( opponentIdentity.connectionToClient  );
        }
        [TargetRpc]
        public void TargetPlayerDeath ( NetworkConnection target  )
        {
            PlayerDeath ( );
        }

    }
}