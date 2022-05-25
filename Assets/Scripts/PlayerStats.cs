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
        [SyncVar]
        public int health;
        public static bool isPlayerDead = false;

        public Slider HealthSlider;
        public Text HealthText;
        // Start is called before the first frame update
        void Start ( )
        {
            HealthSlider = GameObject.Find ( "Canvas Main/HealthSlider" ).GetComponent<Slider> ( );
            HealthText = GameObject.Find ( "Canvas Main/HealthSlider/HealthText" ).GetComponent<Text> ( );
        }

        // Update is called once per frame
        void Update ( )
        {
            HealthSlider.value = health;
            HealthText.text = health.ToString ( );

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

        private void PlayerDeath ( )
        {
            throw new NotImplementedException ( );
        }

        [ServerCallback]
        private void OnTriggerEnter ( Collider other )
        {
            health--;

        }
    }
}