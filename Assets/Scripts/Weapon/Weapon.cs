using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace ApocalipseZ
{
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour,IWeapon
    {
        [SerializeField]private ScriptableWeapon weaponSetting;

        public string weaponName;
        public WeaponType Type;
        private float scopeSensitivityX, scopeSensitivityY;

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
        [SerializeField]private Sway sway;
        [SerializeField]private Recoil  recoilComponent;
        [SerializeField]private AudioSource audioSource;
        [SerializeField]private ParticleSystem temp_MuzzleFlashParticlesFX;
        //prefabs

        private GameObject PrefabProjectile;

        private float nextFireTime;

        [HideInInspector]
        public bool reloading = false;
        [HideInInspector]
        public bool canShot = true;
       
        public bool setAim = false;
        public  bool isThrowingGrenade;

        private void Awake ( )
        {
            PrefabProjectile =   weaponSetting.projectile;
            muzzleFlashTransform = transform.Find ( "Muzzle flash transform" );
        }
        // Start is called before the first frame update
        void Start ( )
        {
            sway = transform.GetComponentInParent<Sway> ( );
            recoilComponent = GameObject.FindObjectOfType<Recoil> ( );
            audioSource = GetComponent<AudioSource> ( );
            if ( GetComponent<Animator> ( ) )
                Animator = GetComponent<Animator> ( );

            if ( weaponSetting.MuzzleFlashParticlesFX  && Type != WeaponType.Grenade)
                temp_MuzzleFlashParticlesFX = Instantiate ( weaponSetting.MuzzleFlashParticlesFX , muzzleFlashTransform.position , muzzleFlashTransform.rotation , muzzleFlashTransform );
        }



        // Update is called once per frame
        void Update ( )
        {
          
        }

        public void  Fire ( IFpsPlayer player )
        {
            if ( Type != WeaponType.Melee && Type != WeaponType.Grenade )
            {
                if ( Time.time > nextFireTime && !reloading && canShot /*&& !controller.isClimbing*/ ) //Allow fire statement
                {

                    if ( currentAmmo > 0 )
                    {
                        currentAmmo -= 1;

                        PlayFX ( );
                        player.CmdSpawBullet ( new SpawBulletTransform ( weaponSetting.projectile.name ,muzzleFlashTransform.position , muzzleFlashTransform.rotation ) , player.GetConnection ( ) );
                        //Getting random damage from minimum and maximum damage.
                        //calculatedDamage = Random.Range ( damageMin , damageMax );

                        // ProjectilesManager ( );

                        recoilComponent.AddRecoil ( weaponSetting.recoil );

                        //Calculating when next fire call allowed
                        nextFireTime = Time.time + weaponSetting.fireRate;
                    }
                    else
                    {
                        if ( !reloading && autoReload )
                        {
                            //verificar autoReload 
                           /// ReloadBegin ( );
                        }
                          
                        else
                            audioSource.PlayOneShot ( weaponSetting.emptySFX );

                        nextFireTime = Time.time + weaponSetting.fireRate;
                    }

                }
               


                   
            }
            else if ( Type == WeaponType.Melee )
            {
                if ( Time.time > nextFireTime ) //Allow fire statement
                {
                    audioSource.Stop ( );
                    audioSource.PlayOneShot ( weaponSetting.shotSFX );
                    Animator.Play ( "Attack" );
                    Invoke ( "MeleeHit" , weaponSetting.meleeHitTime );
                    recoilComponent.AddRecoil ( weaponSetting.recoil );
                    nextFireTime = Time.time + weaponSetting.meleeAttackRate;
                }

            }
            else if ( Type == WeaponType.Grenade && !isThrowingGrenade )
            {
                Animator.SetTrigger ( "Throw" );
                isThrowingGrenade = true;
            }
        }
        public void ReloadBegin ( )
        {
           
            if ( false )
            {
                setAim = false;
                reloading = true;
                canShot = false;

                if ( useAnimator )
                {
                    Animator.SetBool ( "Aim" , false );
                    Animator.Play ( "Reload" );
                }

                audioSource.PlayOneShot ( weaponSetting. reloadingSFX );

                Invoke ( "ReloadEnd" , reloadAnimationDuration );
            }
            else
                return;
        }

        void ReloadEnd ( )
        {

        }
    
        public void Aim ( bool isAim)
        {
            setAim = isAim;
            if (Animator == null)
            {
                return;
            }
            if ( useAnimator )
            {
               Animator.SetBool( "Aim" , isAim );
            }
            if (isAim)
            {
                sway.AmountX = sway.AmountX * 0.3f;
                sway.AmountY = sway.AmountY * 0.3f;
            }
            else
            {
                sway.AmountX = sway.startX;
                sway.AmountY = sway.startY;
            }
            if ( !reloading && useAnimator )
            {
                Animator.SetBool ( "Aim" , setAim );
            }
            else
            {
                setAim = false;
            }
           
        }

        public void ThrowGrenade ( )
        {
            var obj = Instantiate(weaponSetting.grenadePrefab,muzzleFlashTransform.position , Quaternion.identity);
            //obj.transform.position = transform.position + transform.forward * 0.3f;
            obj.GetComponent<Rigidbody> ( ).AddForce ( transform.forward * weaponSetting.throwForce );
            isThrowingGrenade = false;
           // inventory.RemoveItem ( "Grenade" , true );
           // weaponManager.UnhideWeaponAfterGrenadeDrop ( );
        }

        private void PlayFX ( )
        {
            if ( useAnimator )
                Animator.Play ( "Shot" );

            temp_MuzzleFlashParticlesFX.time = 0;
            temp_MuzzleFlashParticlesFX.Play ( );

            audioSource.Stop ( );
            audioSource.PlayOneShot ( weaponSetting.shotSFX );

        }
        public ScriptableWeapon GetScriptableWeapon ( )
        {
            return weaponSetting;
        }
    }

}