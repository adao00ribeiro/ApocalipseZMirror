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
           
            if ( IsPlayerDead( ) )
            {
                PlayerDeath ( );
            }

            if (transform.position.y < -11.1  && !IsPlayerDead( ) )
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
        public bool IsPlayerDead ( )
        {
            return health <= 0;
        }

        public void RestoreLife ( int life )
        {
            health += life;
            print ( "isRestore life");
            if ( health > 100 )
            {
                health = 100;
            }
        }
        public void TakeDamage (int  damage)
        {
            health -= damage;
            
            if ( health < 0 )
            {
                health = 0;
            }
        }
        private void PlayerDeath ( )
        {
         
            if ( GetComponent<FpsPlayer> ( ) )
            {
              
                    GetComponent<FpsPlayer> ( ).DroppAllItems ( );
               
                //mudar Time
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
           
                transform.position = PlayerSpawPoints.Instance.GetPointSpaw ( );
                transform.rotation = Quaternion.identity;
                RestoreLife ( 200 );
                player.Respaw ( );
          
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