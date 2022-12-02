
using System;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
namespace ApocalipseZ
{
    public class FastItemsManager : NetworkBehaviour, IFastItemsManager
    {
        #region  PUBLIC
        [SyncObject]
        public readonly SyncList<SlotInventoryTemp> container = new SyncList<SlotInventoryTemp>();
        #endregion
        #region  PRIVATE
        private UiFastItems uiFastItems;

        private int switchSlotIndex = 0;
        [SerializeField] private InputManager InputManager;

        #endregion

        void Awake()
        {
            InputManager = GameController.Instance.InputManager;
        }
        public override void OnStartServer()
        {
            base.OnStartServer();
            for (int i = 0; i < 3; i++)
            {
                container.Add(new SlotInventoryTemp());
            }
        }
        // Start is called before the first frame update
        public override void OnStartClient()
        {

            base.OnStartClient();
            if (base.IsOwner)
            {

                uiFastItems = GameController.Instance.CanvasFpsPlayer.GetUiFastItems();
                uiFastItems.AddSlots(GetComponent<FpsPlayer>());
                container.OnChange += OnFastItemsUpdated;
            }

        }

        void Update()
        {
            if (!base.IsOwner)
            {
                return;
            }
            if (InputManager.GetAlpha3())
            {
                CmdSlotChange(0);
            }
            else if (InputManager.GetAlpha4())
            {
                CmdSlotChange(1);
            }
            else if (InputManager.GetAlpha5())
            {
                CmdSlotChange(2);
            }
        }
        private void OnFastItemsUpdated(SyncListOperation op, int index, SlotInventoryTemp oldItem, SlotInventoryTemp newItem, bool asServer)
        {
            switch (op)
            {
                case SyncListOperation.Add:
                    // index is where it was added into the list
                    // newItem is the new item
                    uiFastItems.UpdateSlot(index, newItem);
                    break;
                case SyncListOperation.Insert:
                    // index is where it was inserted into the list
                    // newItem is the new item
                    uiFastItems.UpdateSlot(index, newItem);
                    break;
                case SyncListOperation.RemoveAt:
                    // index is where it was removed from the list
                    // oldItem is the item that was removed
                    uiFastItems.UpdateSlot(index, newItem);
                    break;
                case SyncListOperation.Set:
                    // index is of the item that was changed
                    // oldItem is the previous value for the item at the index
                    // newItem is the new value for the item at the index
                    uiFastItems.UpdateSlot(index, newItem);
                    break;
                case SyncListOperation.Clear:
                    // list got cleared
                    break;
                case SyncListOperation.Complete:
                    break;
            }
        }
        #region COMMAND

        [ServerRpc]
        public void CmdSlotChange(int slotIndex, NetworkConnection sender = null)
        {



        }
        [ServerRpc]
        internal void CmdMoveInventory(int slotenter, int SlotSelecionado)
        {
            Inventory inventory = GetComponent<Inventory>();
            SlotInventoryTemp slot = inventory.GetSlot(SlotSelecionado);
            inventory.AddItem(SlotSelecionado, container[slotenter]);
            container[slotenter] = slot;
        }
        [ServerRpc]
        internal void CmdMoveWeaponManager(int slotenter, int SlotSelecionado)
        {
            WeaponManager weaponManager = GetComponent<WeaponManager>();
            SlotInventoryTemp slot = weaponManager.container[SlotSelecionado];
            weaponManager.container[SlotSelecionado] = container[slotenter];
            container[slotenter] = slot;
        }
        [ServerRpc]
        internal void CmdMove(int slotEnterIndex, int slotIndexselecionado)
        {
            SlotInventoryTemp auxEnter = container[slotEnterIndex];
            container[slotEnterIndex] = container[slotIndexselecionado];
            container[slotIndexselecionado] = auxEnter;
        }

        internal ItemType GetTypeItem(int slotIndex)
        {
            DataItem item = GameController.Instance.DataManager.GetDataItemById(container[slotIndex].guidid);
            return item.Type;
        }

        #endregion

    }
}