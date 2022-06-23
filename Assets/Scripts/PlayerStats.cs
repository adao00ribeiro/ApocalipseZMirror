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
		[Client]
        // Update is called once per frame
        void Update ( )
        {
           
            if ( health <= 0 && !isPlayerDead )
            {
                CmdPlayerDeath ( );
            }

            if ( health < 0 )
            {
                health = 0;
            }

            if ( health > 100 )
            {
                health = 100;
            }
            if (transform.position.y < -11.1  && !isPlayerDead )
            {
                TakeDamage ( 200);
            }
        }

        [Command ( requiresAuthority = false )]
        public void CmdTakeDamage (int damage ,  NetworkConnectionToClient sender = null)
        {
            OnAlteredStats?.Invoke ( );
            TakeDamage ( damage );
        }


        public void TakeDamage (int  damage)
        {
            health -= damage;
        }
        private void PlayerDeath ( )
        {
            isPlayerDead = true;
            // GetComponent<FpsPlayer> ( ).DropAllItems ( );
            StartCoroutine ( Respawn ( ) );
        }
        private IEnumerator Respawn ( )
        {
            yield return new WaitForSeconds ( 5f );
            FpsPlayer player = GetComponent<FpsPlayer> ( );
            if ( player )
            {
                transform.position = PlayerSpawPoints.Instance.GetPointSpaw ( );
                transform.rotation = Quaternion.identity;
                health = 100;
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