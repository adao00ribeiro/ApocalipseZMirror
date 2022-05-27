using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace ApocalipseZ
{
    public class FastItems : NetworkBehaviour, IFastItems
    {
        public event Action OnFastItemsAltered;

        public SyncList<SlotInventoryTemp> Items = new SyncList<SlotInventoryTemp>();
        public string[] nomesItems = new string[3];
        public List<SSlotInventory> FastSlots = new List<SSlotInventory>();
        public int maxSlots = 3;

        IInventory inventory;
        public override void OnStartClient ( )
        {
          // Items.Callback += OnInventoryUpdated; ;
          //
          // // Process initial SyncList payload
          // for ( int index = 0 ; index < Items.Count ; index++ )
          // {
          //     OnInventoryUpdated ( SyncList<SlotInventoryTemp>.Operation.OP_ADD , index , new SlotInventoryTemp ( ) , Items[index] );
          // }
        }


        public void OnInventoryUpdated ( SyncList<SlotInventoryTemp>.Operation op , int index , SlotInventoryTemp oldItem , SlotInventoryTemp newItem )
        {

            nomesItems[index] = newItem.guidid;
            switch ( op )
            {
                case SyncList<SlotInventoryTemp>.Operation.OP_ADD:
                    Items[index] = newItem;
                    break;
                case SyncList<SlotInventoryTemp>.Operation.OP_INSERT:
                    // index is where it was inserted into the list
                    // newItem is the new item
                    break;
                case SyncList<SlotInventoryTemp>.Operation.OP_REMOVEAT:
                    // index is where it was removed from the list
                    // oldItem is the item that was removed
                    break;
                case SyncList<SlotInventoryTemp>.Operation.OP_SET:
                    // index is of the item that was changed
                    // oldItem is the previous value for the item at the index
                    // newItem is the new value for the item at the index
                    break;
                case SyncList<SlotInventoryTemp>.Operation.OP_CLEAR:
                    // list got cleared
                    break;
            }
        }
        // Start is called before the first frame update
        void Awake ( )
        {
            for ( int i = 0 ; i < maxSlots ; i++ )
            {
                FastSlots.Add ( new SSlotInventory ( ) );
            }
        }
        public void SetInventory ( IInventory inventory )
        {
            this.inventory = inventory;
        }
        // Update is called once per frame

        public void SlotChange ( int switchSlotIndex )
        {

            if ( !FastSlots[switchSlotIndex - 3].ItemIsNull ( ) )
            {
                FastSlots[switchSlotIndex - 3].Use ( );
            }

        }
        public SSlotInventory GetSlotFastItems ( int id )
        {

            return FastSlots[id];
        }
        public void SetFastSlots ( int id , SSlotInventory slot )
        {
            if ( !slot.ItemIsNull())
            {
                Items.Add ( new SlotInventoryTemp ( slot.GetSItem ( ).GuidId , slot.GetQuantity ( ) ) );
            }
           


            if ( FastSlots[id].ItemIsNull ( ) )
            {
                FastSlots[id].SetSItem ( slot.GetSItem ( ) );
                FastSlots[id].SetQuantity ( slot.GetQuantity ( ) );
            }
            else
            {
                inventory.AddItem ( FastSlots[id] );
                FastSlots[id].SetSItem ( slot.GetSItem ( ) );
                FastSlots[id].SetQuantity ( slot.GetQuantity ( ) );

            }
            OnFastItemsAltered?.Invoke ( );
        }

        public void RemoveSlot ( SSlotInventory slot )
        {
            for ( int i = 0 ; i < FastSlots.Count ; i++ )
            {
                if ( FastSlots[i].GetSItem ( ) == slot.GetSItem ( ) )
                {
                    FastSlots[i] = new SSlotInventory ( );
                    OnFastItemsAltered.Invoke ( );
                    break;
                }
            }

        }

        public void MoveItem ( int id , int idmove )
        {
            SSlotInventory slottemp = FastSlots[idmove];
            FastSlots[idmove] = FastSlots[id];
            FastSlots[id] = slottemp;
            OnFastItemsAltered?.Invoke ( );
        }

        public InventoryTemp GetFastItemsTemp ( )
        {
            List<SlotInventoryTemp> list = new List<SlotInventoryTemp>();

            for ( int i = 0 ; i < FastSlots.Count ; i++ )
            {
                SlotInventoryTemp novo;
                if ( FastSlots[i].ItemIsNull ( ) )
                {
                    novo = new SlotInventoryTemp ( "" , 0 );
                }
                else
                {
                    novo = new SlotInventoryTemp ( FastSlots[i].GetSItem ( ).GuidId , FastSlots[i].GetQuantity ( ) );
                }

                list.Add ( novo );
            }


            return new InventoryTemp ( list , maxSlots );
        }

        public void Clear ( )
        {
            FastSlots.Clear ( );
            for ( int i = 0 ; i < maxSlots ; i++ )
            {
                SSlotInventory temp = new SSlotInventory ( );
                FastSlots.Add ( temp );
            }
        }
        #region COMMAND
        // fast ITEMS
        [Command]
        public void CmdGetFastItems ( NetworkConnectionToClient sender = null )
        {

            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FastItems> ( ).GetFastItemsTemp ( ) );

        }
        [Command]
        public void CmdMoveSlotFastItems ( int idselecionado , int identer , NetworkConnectionToClient sender = null )
        {
            MoveItem ( idselecionado , identer );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FastItems> ( ).GetFastItemsTemp ( ) );
        }
        [Command]
        public void CmdAddSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
            ScriptableItem item = ScriptableManager.GetScriptable(slot.slot.guidid) ;
            SSlotInventory slotnovo;
            if ( item )
            {
                slotnovo = new SSlotInventory ( item.sitem , slot.slot.Quantity );
            }
            else
            {
                slotnovo = new SSlotInventory ( null , 0 );
            }


            SetFastSlots ( slot.id , slotnovo );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FastItems> ( ).GetFastItemsTemp ( ) );

        }
        [Command]
        public void CmdRemoveSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
            //    FastItems.RemoveSlot ( slot.slot );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FastItems> ( ).GetFastItemsTemp ( ) );
        }
        #endregion
        #region TARGERRPC


        [TargetRpc]
        public void TargetGetFastItems ( NetworkConnection target , InventoryTemp fastSlots )
        {
            maxSlots = fastSlots.maxSlot;
            Clear ( );
            for ( int i = 0 ; i < fastSlots.maxSlot ; i++ )
            {
                ScriptableItem item = ScriptableManager.GetScriptable(fastSlots .slot[i].guidid);
                if ( item == null )
                {
                    target.identity.GetComponent<FpsPlayer> ( ).GetFastItems ( ).SetFastSlots (  i, new SSlotInventory ( ) );
                }
                else
                {
                    target.identity.GetComponent<FpsPlayer> ( ).GetFastItems ( ).SetFastSlots (  i, new SSlotInventory ( item.sitem , fastSlots.slot[i].Quantity ) );
                }
            }
            print ( "targer fastites" );
        }

        #endregion
    }
}
