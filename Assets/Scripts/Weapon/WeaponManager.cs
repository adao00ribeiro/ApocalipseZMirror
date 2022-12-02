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
        [SyncObject]
        public readonly SyncList<SlotInventoryTemp> container = new SyncList<SlotInventoryTemp>();

        [Tooltip("Animator that contain pickup animation")]
        public Animator weaponHolderAnimator;


        IFpsPlayer fpsplayer;
        //Transform where weapons will dropped on Drop()
        private Transform playerTransform;

        [SerializeField] private Transform swayTransform;

        private InputManager InputManager;

        public static bool IsChekInventory;
        int currentSlot;

        private void Awake()
        {
            InputManager = GameController.Instance.InputManager;
        }
        public void SetAmmoNetwork(int oldSlot, int newSlot, bool asServer)
        {
            AmmoNetwork = newSlot;
        }
        public override void OnStartServer()
        {
            base.OnStartServer();
            for (int i = 0; i < 2; i++)
            {
                container.Add(new SlotInventoryTemp());
            }
        }
        public override void OnStartClient()
        {
            base.OnStartClient();
            if (base.IsOwner)
            {
                UiPrimaryAndSecondWeapons = GameController.Instance.CanvasFpsPlayer.GetUiPrimaryandSecundaryWeapons();
                UiPrimaryAndSecondWeapons.AddSlots(GetComponent<FpsPlayer>());
                container.OnChange += OnWeaponManagerUpdated;
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
        private void OnWeaponManagerUpdated(SyncListOperation op, int index, SlotInventoryTemp oldItem, SlotInventoryTemp newItem, bool asServer)
        {
            switch (op)
            {
                case SyncListOperation.Add:
                    // index is where it was added into the list
                    // newItem is the new item
                    UiPrimaryAndSecondWeapons.UpdateSlot(index, newItem);
                    break;
                case SyncListOperation.Insert:
                    // index is where it was inserted into the list
                    // newItem is the new item
                    UiPrimaryAndSecondWeapons.UpdateSlot(index, newItem);
                    break;
                case SyncListOperation.RemoveAt:
                    // index is where it was removed from the list
                    // oldItem is the item that was removed
                    UiPrimaryAndSecondWeapons.UpdateSlot(index, newItem);
                    break;
                case SyncListOperation.Set:
                    // index is of the item that was changed
                    // oldItem is the previous value for the item at the index
                    // newItem is the new value for the item at the index
                    UiPrimaryAndSecondWeapons.UpdateSlot(index, newItem);
                    break;
                case SyncListOperation.Clear:
                    // list got cleared
                    break;
                case SyncListOperation.Complete:
                    break;
            }
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
            if (activeSlot != null)
            {
                weaponHolderAnimator.Play("Unhide");
                DesEquipWeapon();
            }
            DataArmsWeapon tempArms = null;
            int ammo = 0;
            tempArms = GameController.Instance.DataManager.GetArmsWeapon(container[currentSlot].Name);
            ammo = container[currentSlot].Ammo;
            if (tempArms == null)
            {
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
            if (activeSlot != null)
            {
                weaponHolderAnimator.Play("Unhide");
                DesEquipWeapon();
            }
            DataArmsWeapon tempArms = null;
            int ammo = 0;
            tempArms = GameController.Instance.DataManager.GetArmsWeapon(container[currentSlot].Name);
            ammo = container[currentSlot].Ammo;
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

        [ServerRpc]
        public void CmdAddWeaponRemoveInventory(int slotenter, int SlotSelecionado)
        {

            SlotInventoryTemp slot = fpsplayer.GetInventory().GetSlot(SlotSelecionado);
            fpsplayer.GetInventory().AddItem(SlotSelecionado, container[slotenter]);
            container[slotenter] = slot;
        }

        [ServerRpc]
        public void CmdMove()
        {
            MoveItem();
        }
        public void MoveItem()
        {
            SlotInventoryTemp auxEnter = container[0];
            container[0] = container[1];
            container[1] = auxEnter;
        }
        internal ItemType GetTypeItem(int slotIndex)
        {
            DataItem item = GameController.Instance.DataManager.GetDataItemById(container[slotIndex].guidid);
            return item.Type;
        }

        internal void CmdMoveFastItemManager(int slotenter, int SlotSelecionado)
        {
            FastItemsManager fastitems = GetComponent<FastItemsManager>();
            SlotInventoryTemp slot = fastitems.container[SlotSelecionado];
            fastitems.container[SlotSelecionado] = container[slotenter];
            container[slotenter] = slot;
        }
    }

}

