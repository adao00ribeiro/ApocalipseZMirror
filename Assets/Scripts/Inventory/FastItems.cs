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

        public List< ListItemsInspector>ListInspector = new List<ListItemsInspector>();
        private List<SSlotInventory> FastSlots;
        [SerializeField]private int maxSlots = 3;

        IInventory inventory;

        // Start is called before the first frame update
        void Awake ( )
        {
            FastSlots = new List<SSlotInventory>();

        }
        private void Update ( )
        {
            ListInspector.Clear ( );
            for ( int i = 0 ; i < FastSlots.Count ; i++ )
            {
                ListInspector.Add ( new ListItemsInspector ( FastSlots[i].GetSlotIndex ( ) , FastSlots[i].GetSItem ( ).name ) );

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
        public SSlotInventory GetFastItems ( int slotindex )
        {
            SSlotInventory temp = null;
            for ( int i = 0 ; i < FastSlots.Count ; i++ )
            {

                if ( FastSlots[i].GetSlotIndex ( ) == slotindex )
                {
                    temp = FastSlots[i];
                }
            }

            return temp;
        }
        public bool AddItem ( int slotIndex , SSlotInventory slot )
        {
            if ( maxSlots == FastSlots.Count )
            {
                return false;
            }
            if (GetFastItems( slotIndex ) !=null)
            {
                inventory.AddItem ( GetFastItems ( slotIndex ) );
            }
            slot.SetSlotIndex ( slotIndex );

            FastSlots.Add ( slot );
            Debug.Log ( "Added item: " + slot.GetSItem ( ).name );
            OnFastItemsAltered?.Invoke ( );
            return true;
        }
        public bool AddItem ( SSlotInventory slot )
        {
            if ( maxSlots == FastSlots.Count )
            {
            
                return false;
            }
            int posicao = -1;
            if ( CheckFreeSpace ( ref posicao ) )
            {
              
                slot.SetSlotIndex ( posicao );

            }
            FastSlots.Add ( slot );
            Debug.Log ( "Added item: " + slot.GetSItem ( ).name );
            OnFastItemsAltered?.Invoke ( );
            return true;
        }

        public bool CheckFreeSpace ( ref int posicao )
        {
            bool isfreespace = false;

            for ( int i = 0 ; i < maxSlots ; i++ )
            {
                SSlotInventory item =  GetFastItems (i );
                if ( item == null )
                {
                    posicao = i;
                    isfreespace = true;
                    break;
                }
            }
            return isfreespace;
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

            for ( int i = 0 ; i < FastSlots.Count ; i++ )
            {
                SlotInventoryTemp novo;
                novo = new SlotInventoryTemp ( FastSlots[i].GetSlotIndex ( ) , FastSlots[i].GetSItem ( ).GuidId , FastSlots[i].GetQuantity ( ) );
                list.Add ( novo );
            }


            return new InventoryTemp ( list , maxSlots);
        }

        public void Clear ( )
        {
            FastSlots.Clear ( );
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
               AddItem ( slot.id , slotnovo );
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
            for ( int i = 0 ; i < fastSlots.slot.Count ; i++ )
            {
                ScriptableItem item = ScriptableManager.GetScriptable(fastSlots .slot[i].guidid);
                if ( item != null )
                {
                    target.identity.GetComponent<FpsPlayer> ( ).GetInventory ( ).AddItem ( fastSlots.slot[i].slotindex , new SSlotInventory ( fastSlots.slot[i].slotindex , item.sitem , fastSlots.slot[i].Quantity ) );
                }

            }
        }

        #endregion
    }
}
