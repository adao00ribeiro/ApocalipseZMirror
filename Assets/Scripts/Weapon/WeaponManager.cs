using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace ApocalipseZ
{
    public class WeaponManager : MonoBehaviour, IWeaponManager
    {

        public List<Weapon> ArmsWeapons = new List<Weapon>();
        public event Action OnWeaponAltered;

        public SSlotInventory primarySlot = new SSlotInventory ( );
        public SSlotInventory secondarySlot = new SSlotInventory ( );
        public Weapon activeSlot;

        public bool UseNonPhysicalReticle = true;
        public int switchSlotIndex = 0;
        public int currentWeaponIndex;

        [Tooltip("Scope image used for riffle aiming state")]
        public GameObject scopeImage;
        [Tooltip("Crosshair image")]
        public GameObject reticleDynamic;
        public GameObject reticleStatic;

        [Tooltip("Animator that contain pickup animation")]
        public Animator weaponHolderAnimator;

        [HideInInspector]
        public GameObject tempGameobject;

        //Transform where weapons will dropped on Drop()
        private Transform playerTransform;

        [SerializeField]private Transform swayTransform;

        private IInventory inventory;

        [HideInInspector]
        //public Weapon currentWeapon;
        // Start is called before the first frame update
        void Start ( )
        {
            scopeImage = GameObject.Find ( "Canvas Main/Scope" );
            swayTransform = transform.Find ( "Camera & Recoil/WeaponCamera/Weapon holder/Sway" ).transform;
            reticleDynamic = GameObject.Find ( "Canvas Main/Reticles/DynamicReticle" );
            reticleStatic = GameObject.Find ( "Canvas Main/Reticles/StaticReticle" );
            weaponHolderAnimator = transform.Find ( "Camera & Recoil/WeaponCamera/Weapon holder" ).GetComponent<Animator>();
            scopeImage.SetActive ( false );
            if ( UseNonPhysicalReticle )
            {
                reticleStatic.SetActive ( true );
                reticleDynamic.SetActive ( false );
            }
            else
            {
                reticleStatic.SetActive ( false );
                reticleDynamic.SetActive ( true );
            }
            foreach ( Weapon weapon in swayTransform.GetComponentsInChildren<Weapon> ( true ) )
            {
                ArmsWeapons.Add ( weapon );
            }
        }
        public void SetFpsPlayer ( FpsPlayer player )
        {
            playerTransform = player.gameObject.transform;
            inventory = player.GetInventory ( );

        }
        // Update is called once per frame
        void Update ( )
        {
            SlotInput ( );

            if (InputManager.instance.GetFire())
            {
                if (activeSlot != null )
                {
                   activeSlot.Fire ( );
                }
                else
                {
                    print ( "WEAPON NULL" );
                }
                
            }

            if (InputManager.instance.GetReload())
            {
                if ( activeSlot != null )
                {
                    activeSlot.ReloadBegin ( );
                }
                else
                {
                    print ( "WEAPON NULL");
                }
            }
            if (InputManager.instance.GetAim())
            {
               
            }
            else
            {

            }
            if ( InputManager.instance.GetDropWeapon ( ) )
            {
                // DropWeapon();
            }

            // if (Input.GetKeyDown(KeyCode.H))
            // {
            //     DropAllWeapons();
            // }


        }

        private void SlotInput ( )
        {
            if ( InputManager.instance.GetAlpha1 ( ) ) { switchSlotIndex = 1; SlotChange ( ); }
            else if ( InputManager.instance.GetAlpha2 ( ) ) { switchSlotIndex = 2; SlotChange ( ); }

        }
        private void SlotChange ( )
        {
            if ( switchSlotIndex == 1 )
            {
                EquipWeapon ( primarySlot.GetSItem() );
            }
            else if ( switchSlotIndex == 2 )
            {
                EquipWeapon ( secondarySlot.GetSItem() );
            }

        }
        public void EquipWeapon ( SItem Item )
        {

            if ( activeSlot != null )
            {
                activeSlot.gameObject.SetActive ( false );
            }

            foreach ( Weapon weapon in ArmsWeapons )
            {
                if ( weapon.weaponName == Item.name )
                {
                    print ( Item.name);
                    activeSlot = weapon;

                    activeSlot.currentAmmo = Item.Ammo;

                    activeSlot.gameObject.SetActive ( true );

                    switchSlotIndex = switchSlotIndex + 1;

                    weaponHolderAnimator.Play ( "Unhide" );

                    break;
                }
            }

        }

        

        public bool SetSlot ( int id , SSlotInventory sSlotInventory )
        {
          
            if ( sSlotInventory.GetSItem().Type != ItemType.weapon)
            {
                return false;
            }
            if (id == 0)
            {
                primarySlot = sSlotInventory;
                OnWeaponAltered.Invoke ( );
                return true;
            }
            else if ( id == 1)
            {
                secondarySlot = sSlotInventory;
                OnWeaponAltered.Invoke ( );
                return true;
            }
            else
            {
                return false;
            }
                    
        }

        public SSlotInventory GetSlot ( int id)
        {
          
            if (id == 0)
            {
                return primarySlot;
            }else if ( id == 1)
            {
                return secondarySlot;
            }
            return new SSlotInventory ( );
        }

        public void RemoveSlot ( SSlotInventory sSlotInventory )
        {
            if ( sSlotInventory.GetSItem() == primarySlot.GetSItem ( ) )
            {
                primarySlot = new SSlotInventory ( );
            }else if ( sSlotInventory.GetSItem ( ) == secondarySlot.GetSItem ( ) )
            {
                secondarySlot = new SSlotInventory ( );
            }

            OnWeaponAltered.Invoke ( );
        }

        public void MoveItem ( int id , int idmove )
        {
            if (id==0)
            {
                SSlotInventory slottemp = secondarySlot;
                secondarySlot = primarySlot;
                primarySlot = slottemp;
                OnWeaponAltered.Invoke ( );
            }else if (id==1)
            {
                SSlotInventory slottemp = primarySlot;
                primarySlot = secondarySlot;
                secondarySlot = slottemp;
                OnWeaponAltered.Invoke ( );
            }
           
        }
    }

}

