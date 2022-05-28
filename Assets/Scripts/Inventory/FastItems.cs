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
       
        public string[] nomesItems = new string[3];
        private SSlotInventory[] FastSlots;
        public int maxSlots = 3;

        IInventory inventory;

        // Start is called before the first frame update
        void Awake ( )
        {
            FastSlots = new SSlotInventory[maxSlots];

            FastSlots[0] = null;
            FastSlots[1] = null;
            FastSlots[2] = null;

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

            if ( FastSlots[id]== null )
            {
                FastSlots[id] = slot;
             
            }
            else
            {
                inventory.AddItem ( FastSlots[id] );

                FastSlots[id] = slot;
               

            }
            OnFastItemsAltered?.Invoke ( );
        }

        public void RemoveSlot ( SSlotInventory slot )
        {
            for ( int i = 0 ; i < maxSlots ; i++ )
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

            for ( int i = 0 ; i < maxSlots ; i++ )
            {
                SlotInventoryTemp novo;
                if ( FastSlots[i] == null )
                {
                    novo = new SlotInventoryTemp ( -1,"" , 0 );
                }
                else
                {
                    novo = new SlotInventoryTemp ( FastSlots[i].GetSlotIndex(), FastSlots[i].GetSItem ( ).GuidId , FastSlots[i].GetQuantity ( ) );
                }

                list.Add ( novo );
            }


            return new InventoryTemp ( list , maxSlots );
        }

        public void Clear ( )
        {
            for ( int i = 0 ; i < maxSlots ; i++ )
            {
                FastSlots[i] = null;
            }
        }
        #region COMMAND
        // fast ITEMS
        [Command]
        public void CmdGetFastItems ( NetworkConnectionToClient sender = null )
        {
            print ("command getfastitems");
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FastItems> ( ).GetFastItemsTemp ( ) );

        }
        [Command]
        public void CmdMoveSlotFastItems ( int idselecionado , int identer , NetworkConnectionToClient sender = null )
        {
            print ( "command moveitem fastitems" );
            MoveItem ( idselecionado , identer );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FastItems> ( ).GetFastItemsTemp ( ) );
        }
        [Command]
        public void CmdAddSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
            print ( "command add fastitems" );
            ScriptableItem item = ScriptableManager.GetScriptable(slot.slot.guidid) ;
            SSlotInventory slotnovo;
            if ( item )
            {
               slotnovo = new SSlotInventory ( slot.slot.slotindex,item.sitem , slot.slot.Quantity );
               SetFastSlots ( slot.id , slotnovo );
            }
                     
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FastItems> ( ).GetFastItemsTemp ( ) );

        }
        [Command]
        public void CmdRemoveSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
            ScriptableItem item = ScriptableManager.GetScriptable(slot.slot.guidid) ;
            SSlotInventory slotnovo;
            if ( item )
            {
               // slotnovo = new SSlotInventory ( item.sitem , slot.slot.Quantity );
            }
            else
            {
              //  slotnovo = new SSlotInventory ( null , 0 );
            }
          //  RemoveSlot ( slotnovo );
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
                print ( fastSlots.slot[i].guidid );
                ScriptableItem item = ScriptableManager.GetScriptable(fastSlots .slot[i].guidid);
                if ( item == null )
                {
                    target.identity.GetComponent<FpsPlayer> ( ).GetFastItems ( ).SetFastSlots (  i,null);
                }
                else
                {
                   target.identity.GetComponent<FpsPlayer> ( ).GetFastItems ( ).SetFastSlots (  i, new SSlotInventory ( fastSlots.slot[i].slotindex, item.sitem , fastSlots.slot[i].Quantity ) );
                }
            }
            print ( "targer fastites" );
        }

        #endregion
    }
}
