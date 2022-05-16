using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    public class Weapon : MonoBehaviour,IWeapon
    {
        [SerializeField]private ScriptableWeapon weaponSetting;

        public string weaponName;
        public WeaponType Type;

        [Header("Transforms Objects Spaws")]
        [Tooltip("Transform to instantiate particle system shot fx")]
        public Transform muzzleFlashTransform;
        [Tooltip("Transform to eject shell after shot")]
        public Transform shellTransform;
        [Tooltip("Transform to instantiate bullet on shot")]
        public Transform bulletTransform;

        [Tooltip("If you have animations for your weapon so better to use animator. Play animations if true and not if false")]
        public bool useAnimator = true;

        [Tooltip("How long reload animation is? Time in seconds to synch reloading animation with script")]
        public float reloadAnimationDuration = 3.0f;

        [Tooltip("Should weapon reload when ammo is 0")]
        public bool autoReload = true;

        [Header("Ammo")]
        [Tooltip("Ammo count in weapon magazine")]
        public int currentAmmo = 30;
        [Tooltip("Max weapon ammo capacity")]
        public int maxAmmo = 30;

        public enum FireMode { automatic, single }
        [Header("Fire mode")]
        public FireMode fireMode;

        private Animator Animator;

        // Start is called before the first frame update
        void Start ( )
        {
            if ( GetComponent<Animator> ( ) )
                Animator = GetComponent<Animator> ( );
        }



        // Update is called once per frame
        void Update ( )
        {
            
        }

        public void Fire ( )
        {
            if ( useAnimator )
                Animator.Play ( "Shot" );
        }
        public void ReloadBegin ( )
        {
             if ( useAnimator )
                {
                    Animator.SetBool ( "Aim" , false );
                    Animator.Play ( "Reload" );
                }
        }

        void ReloadEnd ( )
        {

        }
    }
}