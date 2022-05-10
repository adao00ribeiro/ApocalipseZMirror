﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
public class WeaponManager : MonoBehaviour,IWeaponManager
{
        /*
        public List<Weapon> ArmsWeapons;

        public Weapon primarySlot;
        public Weapon secondarySlot;
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

        private Transform swayTransform;

        private IInventory inventory;

        [HideInInspector]
        //public Weapon currentWeapon;
        // Start is called before the first frame update
        void Start()
        {
            swayTransform = FindObjectOfType<Sway>().GetComponent<Transform>();
            scopeImage.SetActive(false);
            if (UseNonPhysicalReticle)
            {
                reticleStatic.SetActive(true);
                reticleDynamic.SetActive(false);
            }
            else
            {
                reticleStatic.SetActive(false);
                reticleDynamic.SetActive(true);
            }
            foreach (Weapon weapon in swayTransform.GetComponentsInChildren<Weapon>(true))
            {
                    ArmsWeapons.Add(weapon);
            }
        }
        public void SetFpsPlayer( FpsPlayer player)
        {
            playerTransform = player.gameObject.transform;
            inventory = player.GetInventory();

        }
            // Update is called once per frame
        void Update()
        {
            SlotInput();

            if(InputManager.instance.GetDropWeapon())
             {
                DropWeapon();
             }
            /*I
            if (Input.GetKeyDown(KeyCode.H))
            {
                DropAllWeapons();
            }


        }

        private void SlotInput()
        {
            if (InputManager.instance.GetAlpha1()){ switchSlotIndex = 1; SlotChange(); }
            else if (InputManager.instance.GetAlpha2()){ switchSlotIndex = 2; SlotChange(); }
            else if (InputManager.instance.GetAlpha3()){ switchSlotIndex = 3; SlotChange(); }
            else if (InputManager.instance.GetAlpha4()){ switchSlotIndex = 4; SlotChange(); }
            else if (InputManager.instance.GetAlpha5()){ switchSlotIndex = 5; SlotChange(); }
            else if (InputManager.instance.GetAlpha6()){ switchSlotIndex = 6; SlotChange(); }
            else if (InputManager.instance.GetAlpha7()){ switchSlotIndex = 7; SlotChange(); }
            else if (InputManager.instance.GetAlpha8()){ switchSlotIndex = 8; SlotChange(); }
            else if (InputManager.instance.GetAlpha9()){ switchSlotIndex = 9; SlotChange(); }

        }
            private void SlotChange ( )
            {

                print ( "Slot change 2" );
                if ( activeSlot != null && activeSlot.storedWeapon != null )
                {///////  012            /// 012     
                    if ( slots.Count > switchSlotIndex )
                    {
                        if ( slots[switchSlotIndex].storedWeapon != null )
                        {
                            activeSlot.storedWeapon.gameObject.SetActive ( false );

                            activeSlot = null;
                            activeSlot = slots[switchSlotIndex];
                            activeSlot.storedWeapon.gameObject.SetActive ( true );

                            print ( "I equip slot number" + switchSlotIndex );

                            weaponHolderAnimator.Play ( "Unhide" );
                        }
                    }
                    else
                        return;
                }
                else
                    return;
            }
            //EquipWeapon is called from Item class on pickup. Item class passes arguments to EquipWeapon
            public bool IsWeaponAlreadyPicked ( string weaponName )
            {
                foreach ( Slot slot in slots )
                {
                    if ( slot.storedWeapon != null && slot.storedWeapon.weaponName == weaponName )
                        return true;
                }
                return false;
            }

            public Weapon FindWeaponByName ( string name )
            {
                foreach ( Slot slot in slots )
                {
                    if ( slot.storedWeapon.weaponName == name )
                        return slot.storedWeapon;
                }

                return null;
            }

            public void EquipWeapon ( string weaponName , GameObject temp )
            {
                print ( temp.name );

                grenade.gameObject.SetActive ( false );

                if ( !IsWeaponAlreadyPicked ( weaponName ) )
                {
                    if ( FindFreeSlot ( ) != null )
                    {   
                        if ( activeSlot != null )
                            activeSlot.storedWeapon.gameObject.SetActive ( false );

                        activeSlot = FindFreeSlot ( );

                        foreach ( Weapon weapon in weapons )
                        {
                            if ( weapon.weaponName == weaponName )
                            {
                                activeSlot.storedWeapon = weapon;
                                print ( activeSlot.storedWeapon.currentAmmo );
                                print ( temp.GetComponent<WeaponPickup> ( ).ammoInWeaponCount );
                                activeSlot.storedWeapon.currentAmmo = temp.GetComponent<WeaponPickup> ( ).ammoInWeaponCount;
                                activeSlot.storedDropObject = temp;
                                activeSlot.storedDropObject.SetActive ( false );
                                temp = null;
                                activeSlot.storedWeapon.gameObject.SetActive ( true );

                                switchSlotIndex = switchSlotIndex + 1;

                                weaponHolderAnimator.Play ( "Unhide" );

                                break;
                            }
                        }

                    }
                }
                else
                {
                    Weapon weapon_temp = FindWeaponByName(weaponName);

                    //temp.SetActive(false);
                    //temp = null;
                    print ( "Weapon already exist" );
                }
            }

            private Slot FindEquipedSlot ( )
            {
                foreach ( Slot slot in slots )
                {
                    if ( !slot.IsFree ( ) )
                        return slot;
                }

                return null;
            }

            public void HideWeaponOnDeath ( )
            {
                weaponHolderAnimator.SetLayerWeight ( 1 , 0 );
                weaponHolderAnimator.SetBool ( "HideWeapon" , true );
            }

            public void UnhideWeaponOnRespawn ( )
            {
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

                weaponHolderAnimator.SetLayerWeight ( 1 , 1 );
                weaponHolderAnimator.SetBool ( "HideWeapon" , false );
            }

            public void HideAll ( )
            {
                print ( "Hide weapon works" );

                if ( activeSlot != null )
                {
                    activeSlot.storedWeapon.gameObject.SetActive ( false );

                    weaponHolderAnimator.Play ( "HideWeapon" );
                }
            }

            public void UnhideWeaponAfterGrenadeDrop ( )
            {
                grenade.gameObject.SetActive ( false );

                if ( activeSlot != null )
                {
                    weaponHolderAnimator.Play ( "Unhide" );
                    activeSlot.storedWeapon.gameObject.SetActive ( true );
                }
            }

            public void DropAllWeapons ( )
            {
                weaponHolderAnimator.SetLayerWeight ( 1 , 0 );
                weaponHolderAnimator.SetBool ( "HideWeapon" , true );

                foreach ( Slot slot in slots )
                {
                    if ( !slot.IsFree ( ) )
                    {
                        if ( slot.storedWeapon.weaponType != WeaponType.Melee && haveMeleeWeaponByDefault )
                        {
                            if ( slot.storedWeapon == activeSlot.storedWeapon )
                            {
                                DropWeapon ( );
                            }
                            else
                            {
                                slot.storedWeapon.gameObject.SetActive ( false );
                                if ( slot.storedDropObject )
                                {
                                    slot.storedDropObject.SetActive ( true );
                                    slot.storedDropObject.transform.position = playerTransform.transform.position + playerTransform.forward * 0.5f;
                                    slot.storedDropObject = null;
                                    slot.storedWeapon = null;
                                }
                            }
                        }
                    }
                }

                if ( haveMeleeWeaponByDefault )
                {
                    activeSlot = slots[0];
                    activeSlot.storedWeapon.gameObject.SetActive ( true );
                }
            }

            public void ChangeWeaponMobile ( bool equipMelee )
            {
                print ( "I work!" );

                if ( !equipMelee )
                {
                    print ( "I equip no melee" );

                    if ( slots[1].storedWeapon != null && slots[2].storedWeapon != null )
                    {
                        if ( activeSlot.storedWeapon == slots[2].storedWeapon )
                        {
                            HideAll ( );
                            switchSlotIndex = 1;
                            SlotChange ( );
                        }
                        else if ( activeSlot.storedWeapon == slots[1].storedWeapon )
                        {
                            HideAll ( );
                            switchSlotIndex = 2;
                            SlotChange ( );
                        }
                        else if ( activeSlot.storedWeapon == slots[0].storedWeapon )

                        {
                            if ( slots[1].storedWeapon != null )
                            {
                                HideAll ( );
                                switchSlotIndex = 1;
                                SlotChange ( );
                            }
                            else if ( slots[2].storedWeapon != null )
                            {
                                HideAll ( );
                                switchSlotIndex = 2;
                                SlotChange ( );
                            }
                        }
                    }
                    else if ( activeSlot.storedWeapon == slots[0].storedWeapon )
                    {
                        if ( slots[1].storedWeapon != null )
                        {
                            HideAll ( );
                            switchSlotIndex = 1;
                            SlotChange ( );
                        }
                        else if ( slots[2].storedWeapon != null )
                        {
                            HideAll ( );
                            switchSlotIndex = 2;
                            SlotChange ( );
                        }
                    }

                }
                else if ( equipMelee && haveMeleeWeaponByDefault )
                {
                    print ( "I equip melee" );

                    HideAll ( );
                    switchSlotIndex = 0;
                    SlotChange ( );
                }
            }

            private void DropWeapon ( )
            {
                if ( activeSlot != null )
                {
                    if ( activeSlot != slots[0] && haveMeleeWeaponByDefault )
                    {
                        activeSlot.storedDropObject.GetComponent<WeaponPickup> ( ).ammoInWeaponCount = activeSlot.storedWeapon.currentAmmo;
                        activeSlot.storedWeapon.gameObject.SetActive ( false );
                        activeSlot.storedDropObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;
                        activeSlot.storedDropObject.SetActive ( true );
                        activeSlot.storedDropObject = null;
                        activeSlot.storedWeapon = null;
                        activeSlot = FindEquipedSlot ( );

                        if ( activeSlot != null )
                            activeSlot.storedWeapon.gameObject.SetActive ( true );

                        weaponHolderAnimator.Play ( "Unhide" );
                    }
                }
            }

            public void DropWeaponFromSlot ( int slot )
            {
                if ( activeSlot.storedWeapon == slots[slot].storedWeapon )
                {
                    DropWeapon ( );
                }
                else
                {
                    slots[slot].storedDropObject.GetComponent<WeaponPickup> ( ).ammoInWeaponCount = slots[slot].storedWeapon.currentAmmo;
                    slots[slot].storedDropObject.transform.position = playerTransform.position + playerTransform.forward * 0.5f;
                    slots[slot].storedDropObject.SetActive ( true );
                    slots[slot].storedDropObject = null;
                    slots[slot].storedWeapon = null;
                }
            }
        */
    }
}
