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
        [SerializeField]private Text WeaponText;
        [SerializeField]private Text AmmoText;

        IFpsPlayer player;
        [SerializeField]PlayerStats stats;
        [SerializeField]IWeaponManager WeaponManager;
        private void Awake ( )
        {
            HealthSlider = transform.Find ( "HealthSlider" ).GetComponent<Slider>();
            HealthText = transform.Find ( "HealthSlider/HealthText" ).GetComponent<Text> ( );
            WeaponText = transform.Find ( "InfoPanel/WeaponText" ).GetComponent<Text> ( );
            AmmoText = transform.Find ( "InfoPanel/AmmoText" ).GetComponent<Text> ( );
        }
        public void OnUpdateHealth ( )
        {
            if ( player == null )
            {
                return;
            }
            HealthSlider.value = stats.health;
            HealthText.text = stats.health.ToString ( );
                    
            
        }
        public void OnActiveSlot ( Weapon weapon)
        {
            if ( weapon )
            {
                WeaponText.text = weapon.weaponName;
                AmmoText.text = weapon.currentAmmo.ToString ( );
            }
            else
            {
                WeaponText.text = " ";
                AmmoText.text = " ";
            }
        }
        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
            WeaponManager = _player.GetWeaponManager ( );
            stats = player.GetPlayerStats ( );
            stats.OnAlteredStats += OnUpdateHealth; ;
            WeaponManager.OnActiveWeapon += OnActiveSlot; ;
        }
    }
}