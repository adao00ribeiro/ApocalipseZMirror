using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;

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

        }

        [Command ( requiresAuthority = false )]
        public void CmdTakeDamage ( NetworkConnectionToClient sender = null)
        {
            OnAlteredStats?.Invoke ( );
            TakeDamage ( );
        }


        public void TakeDamage ( )
        {
            health--;
        }
        private void PlayerDeath ( )
        {
            isPlayerDead = true;
        }

        //[ServerCallback]
        private void OnTriggerEnter ( Collider other )
        {
           

        }
    }
}