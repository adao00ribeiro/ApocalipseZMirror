using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ApocalipseZ {
    public class UiPlayerStats : MonoBehaviour
    {
        [SerializeField]private Slider HealthSlider;
        [SerializeField]private Text HealthText;
        IFpsPlayer player;
        [SerializeField]PlayerStats stats;
        private void Awake ( )
        {
            HealthSlider = transform.Find ( "HealthSlider" ).GetComponent<Slider>();
            HealthText = transform.Find ( "HealthSlider/HealthText" ).GetComponent<Text> ( );
        }
        public void OnUpdateHealth ( )
        {
            if ( player == null )
            {
                return;
            }
            print ( "updatestats");
            HealthSlider.value = stats.health;
            HealthText.text = stats.health.ToString ( );
        }
        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
            stats = player.GetPlayerStats ( );
            stats.OnAlteredStats += OnUpdateHealth; ;

        }
    }
}