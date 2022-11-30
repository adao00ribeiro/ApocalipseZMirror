using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using System;

namespace ApocalipseZ
{
    public class WeaponManager : NetworkBehaviour
    {

        UiPrimaryAndSecondWeapons UiPrimaryAndSecondWeapons;
        public List<Weapon> ArmsWeapons = new List<Weapon>();


        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(SetAmmoNetwork))]
        public int AmmoNetwork;
        public Weapon activeSlot;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(SetPrimaryWeapon))]
        public SlotInventoryTemp PrimaryWeapon;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(SetSecundaryWeapon))]
        public SlotInventoryTemp SecundaryWeapon;

        [Tooltip("Animator that contain pickup animation")]
        public Animator weaponHolderAnimator;


        IFpsPlayer fpsplayer;
        //Transform where weapons will dropped on Drop()
        private Transform playerTransform;

        [SerializeField] private Transform swayTransform;

        private InputManager InputManager;

        public static bool IsChekInventory;
        int currentSlot;


        public void SetAmmoNetwork(int oldSlot, int newSlot, bool asServer)
        {
            AmmoNetwork = newSlot;

        }
        private void Awake()
        {
            PrimaryWeapon = new SlotInventoryTemp();
            SecundaryWeapon = new SlotInventoryTemp();
            InputManager = GameController.Instance.InputManager;

        }
        public override void OnStartClient()
        {

            base.OnStartClient();
            if (base.IsOwner)
            {

                UiPrimaryAndSecondWeapons = GameController.Instance.CanvasFpsPlayer.GetUiPrimaryandSecundaryWeapons();
                UiPrimaryAndSecondWeapons.SetInventory(GetComponent<Inventory>());
                UiPrimaryAndSecondWeapons.SetWeaponManager(this);
            }

        }
        void Start()
        {

            swayTransform = transform.Find("Camera & Recoil/Weapon holder/Sway").transform;
            weaponHolderAnimator = transform.Find("Camera & Recoil/Weapon holder").GetComponent<Animator>();
            foreach (Weapon weapon in swayTransform.GetComponentsInChildren<Weapon>(true))
            {
                ArmsWeapons.Add(weapon);
            }

        }
        public void SetFpsPlayer(IFpsPlayer fps)
        {
            fpsplayer = fps;
        }
        // Update is called once per frame
        void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            if (IsChekInventory)
            {
                IsChekInventory = false;
            }
            if (InputManager.GetAlpha1())
            {

                currentSlot = 0;
                CmdSlotChange(this.gameObject);
            }
            else if (InputManager.GetAlpha2())
            {
                currentSlot = 1;
                CmdSlotChange(this.gameObject);
            }
            if (activeSlot == null)
            {
                return;
            }


            if (InputManager.GetFire() && !fpsplayer.GetMoviment().CheckIsRun() && !CanvasFpsPlayer.IsInventoryOpen)
            {
                CmdFire();

            }
            if (InputManager.GetReload())
            {
                activeSlot.ReloadBegin();

            }
            if (InputManager.GetAim() && !fpsplayer.GetMoviment().CheckIsRun())
            {
                activeSlot.Aim(true);
                weaponHolderAnimator.SetBool("Walk", false);
                weaponHolderAnimator.SetBool("Run", false);
            }
            else
            {
                activeSlot.Aim(false);
            }

            if (InputManager.GetDropWeapon())
            {
                DropWeapon();
            }

            // if (Input.GetKeyDown(KeyCode.H))
            // {
            //     DropAllWeapons();
            // }
        }
        [ServerRpc]
        public void CmdFire()
        {
            if (activeSlot.Fire())
            {
                RpcFire(base.Owner);
            }
            AmmoNetwork = activeSlot.CurrentAmmo;
        }
        [TargetRpc]
        public void RpcFire(NetworkConnection conn)
        {
            activeSlot.PlayFX();
        }
        [ServerRpc]
        public void CmdSlotChange(GameObject target)
        {
            DataArmsWeapon tempArms = null;
            int ammo = 0;
            if (currentSlot == 0)
            {
                DesEquipWeapon();
                tempArms = GameController.Instance.DataManager.GetArmsWeapon(PrimaryWeapon.Name);
                ammo = PrimaryWeapon.Ammo;
            }
            else
            {
                tempArms = GameController.Instance.DataManager.GetArmsWeapon(SecundaryWeapon.Name);
                ammo = SecundaryWeapon.Ammo;
            }
            if (tempArms == null)
            {
                print("aki");
                return;
            }
            activeSlot = Instantiate(tempArms.PrefabArmsWeapon, swayTransform).GetComponent<Weapon>();
            activeSlot.Cam = fpsplayer.GetFirstPersonCamera();
            activeSlot.CurrentAmmo = ammo;
            activeSlot.gameObject.SetActive(true);
            weaponHolderAnimator.Play("Unhide");
            TargetRpcChanve(base.Owner);
        }

        [TargetRpc]
        public void TargetRpcChanve(NetworkConnection target)
        {
            DataArmsWeapon tempArms = null;
            int ammo = 0;
            if (currentSlot == 0)
            {
                DesEquipWeapon();
                print("aki");
                tempArms = GameController.Instance.DataManager.GetArmsWeapon(PrimaryWeapon.Name);
                ammo = PrimaryWeapon.Ammo;
            }
            else
            {
                tempArms = GameController.Instance.DataManager.GetArmsWeapon(SecundaryWeapon.guidid);
                ammo = SecundaryWeapon.Ammo;
            }

            if (tempArms == null)
            {
                print("aki");
                return;
            }

            activeSlot = Instantiate(tempArms.PrefabArmsWeapon, swayTransform).GetComponent<Weapon>();
            activeSlot.Cam = fpsplayer.GetFirstPersonCamera();
            activeSlot.CurrentAmmo = ammo;
            activeSlot.gameObject.SetActive(true);
            weaponHolderAnimator.Play("Unhide");
        }
        private void SelecionaWeapon()
        {
            if (CanvasFpsPlayer.IsInventoryOpen)
            {
                return;
            }

        }
        private void DropWeapon()
        {
            if (activeSlot != null)
            {
                //CmdDropWeapon(container.GetSlotContainer(switchSlotIndex).GetSlotTemp());
                weaponHolderAnimator.Play("Unhide");
                DesEquipWeapon();
            }
        }
        /*
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
        */


        public void DesEquipWeapon()
        {
            if (activeSlot != null)
            {
                weaponHolderAnimator.Play("Hide");
                Destroy(activeSlot.gameObject);
                activeSlot = null;
            }

        }

        public IWeapon GetActiveWeapon()
        {
            return activeSlot;
        }
        public void SetPrimaryWeapon(SlotInventoryTemp oldSlot, SlotInventoryTemp newSlot, bool asServer)
        {

            PrimaryWeapon = newSlot;
            UiPrimaryAndSecondWeapons.UpdatePrimaryWeapon(newSlot);
        }
        public void SetSecundaryWeapon(SlotInventoryTemp oldSlot, SlotInventoryTemp newSlot, bool asServer)
        {
            SecundaryWeapon = newSlot;
            UiPrimaryAndSecondWeapons.UpdateSecundaryWeapon(newSlot);
        }
        [ServerRpc]
        public void CmdAddWeaponRemoveInventory(int slotenter, int SlotSelecionado)
        {
            SlotInventoryTemp slot = fpsplayer.GetInventory().GetSlot(SlotSelecionado);
            if (slotenter == 0)
            {
                fpsplayer.GetInventory().AddItem(SlotSelecionado, PrimaryWeapon);
                PrimaryWeapon = slot;

            }
            else
            {
                fpsplayer.GetInventory().AddItem(SlotSelecionado, SecundaryWeapon);
                SecundaryWeapon = slot;
            }

        }

        [ServerRpc]
        public void CmdMove()
        {
            MoveItem();
        }
        public void MoveItem()
        {
            SlotInventoryTemp auxEnter = PrimaryWeapon;
            PrimaryWeapon = SecundaryWeapon;
            SecundaryWeapon = auxEnter;
        }
        internal ItemType GetTypeItem(int slotIndex)
        {

            DataItem item = null;
            if (slotIndex == 0)
            {
                item = GameController.Instance.DataManager.GetDataItemById(PrimaryWeapon.guidid);
                return item.Type;
            }

            item = GameController.Instance.DataManager.GetDataItemById(SecundaryWeapon.guidid);

            return item.Type;
        }
    }

}

