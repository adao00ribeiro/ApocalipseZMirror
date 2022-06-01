using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace ApocalipseZ
{
    public class WeaponManager : MonoBehaviour, IWeaponManager
    {

        public List<Weapon> ArmsWeapons = new List<Weapon>();
       
        public Weapon activeSlot;

        IContainer container;
       
        public int switchSlotIndex = 0;
        public int currentWeaponIndex;

        

        [Tooltip("Animator that contain pickup animation")]
        public Animator weaponHolderAnimator;

        [HideInInspector]
        public GameObject tempGameobject;

        //Transform where weapons will dropped on Drop()
        private Transform playerTransform;

        [SerializeField]private Transform swayTransform;
               

        private InputManager PInputManager;
        public  InputManager InputManager
        {
            get
            {
                if ( PInputManager==null)
                {
                    PInputManager = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ( ) ;
                }
                return PInputManager;
            }
        }

        [HideInInspector]
        //public Weapon currentWeapon;
        // Start is called before the first frame update
        void Start ( )
        {
           
            swayTransform = transform.Find ( "Camera & Recoil/WeaponCamera/Weapon holder/Sway" ).transform;
          
            weaponHolderAnimator = transform.Find ( "Camera & Recoil/WeaponCamera/Weapon holder" ).GetComponent<Animator>();
        
            foreach ( Weapon weapon in swayTransform.GetComponentsInChildren<Weapon> ( true ) )
            {
                ArmsWeapons.Add ( weapon );
            }
        }
        public void SetFpsPlayer ( FpsPlayer player )
        {
            playerTransform = player.gameObject.transform;
            container = player.GetWeaponsSlots ( );
            container.OnContainerAltered += SlotChange ( );
        }
        // Update is called once per frame
        void Update ( )
        {
            SlotInput ( );

            if (InputManager.GetFire())
            {
                if (activeSlot != null )
                {
                   activeSlot.Fire ( );
                }
                else
                {
                   // print ( "WEAPON NULL" );
                }
                
            }

            if (InputManager.GetReload())
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
            if (InputManager.GetAim())
            {
               
            }
            else
            {

            }
            if ( InputManager.GetDropWeapon ( ) )
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
            if ( InputManager.GetAlpha1 ( ) ) 
            { 
                switchSlotIndex = 1;
                SlotChange ( ); 
            }
            else if ( InputManager.GetAlpha2 ( ) )
            { 
                switchSlotIndex = 2; 
                SlotChange ( ); 
            }

        }
        
        private void SlotChange ( )
        {
            EquipWeapon ( container.GetSlotContainer ( switchSlotIndex - 1 ) );
        }
        public void EquipWeapon ( SSlotInventory slot )
        {
            if (slot==null)
            {
                return;
            }
            if ( activeSlot != null )
            {
                activeSlot.gameObject.SetActive ( false );
            }

            foreach ( Weapon weapon in ArmsWeapons )
            {
                if ( weapon.weaponName == slot.GetSItem().name )
                {
                    print ( slot.GetSItem ( ).name );
                    activeSlot = weapon;

                    activeSlot.currentAmmo = slot.GetSItem().Ammo;

                    activeSlot.gameObject.SetActive ( true );

                    switchSlotIndex = switchSlotIndex + 1;

                    weaponHolderAnimator.Play ( "Unhide" );

                    break;
                }
            }

        }

      
    }

}

