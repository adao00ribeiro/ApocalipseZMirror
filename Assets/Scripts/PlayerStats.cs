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

        public TextMesh playerhealthText;

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
		[Server]
        // Update is called once per frame
        void Update ( )
        {
            if ( playerhealthText )
            {
                playerhealthText.text = health.ToString();
            }

            if ( health <= 0 && !isPlayerDead )
            {
                PlayerDeath ( );
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
	        Timer.Instance.Add(()=>
	        {
	        	isPlayerDead = false;
		        FpsPlayer player = GetComponent<FpsPlayer> ( );
		        if ( player)
		        {
			  
		        transform.position = PlayerSpawPoints.Instance.GetPointSpaw ( );
		        transform.rotation = Quaternion.identity;
			    health = 100;
			    player.Respaw ( );
	        	}
	        },5);
        }
     
        
    }
}