using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
namespace ApocalipseZ
{
    public enum TypeContainer :byte{ INVENTORY,FASTITEMS,WEAPONS}
    [System.Serializable]
    public struct ListItemsInspector
    {
        public int slotindex;
        public string name;

       public  ListItemsInspector (int _slotindex ,  string _name )
        {
            slotindex = _slotindex;
            name = _name;
        }
    }

    [System.Serializable]
    public class SSlotInventory
    {
        private int SlotIndex;
        private SItem item;
        private int Quantity;
        public SSlotInventory ( )
        {
            item = null;
            Quantity = 0;
        }
        public SSlotInventory ( int slotindex,SItem _item , int _Quantity )
        {
            SlotIndex = slotindex;
            item = _item;
            Quantity = _Quantity;
        }
        public bool Compare ( SSlotInventory other )
        {
            if ( this.item == other.item )
            {
                return true;
            }

            return false;
        }
        public void Use ( )
        {
            Quantity--;
        }
        public bool ItemIsNull ( )
        {
            if (item == null)
            {
                return true;
            }
            return false;
        }

        public int GetSlotIndex ( )
        {
            return SlotIndex;
        }
        public SItem GetSItem ( )
        {
            return item;
        }
        public int GetQuantity ( )
        {
            return Quantity;
        }
        public void SetSlotIndex ( int _slotIndex )
        {
            
            SlotIndex = _slotIndex;
            
        }
        public void SetQuantity (int _Quantity )
        {
            Quantity = _Quantity;
        }
        public void SetSItem ( SItem _item )
        {
            item = _item;
        }

        public SlotInventoryTemp GetSlotTemp ( )
        {
            return new SlotInventoryTemp ( SlotIndex,item.GuidId, Quantity );
        }
    }
public class Container : NetworkBehaviour,IContainer
    {
        public event Action OnInventoryAltered;
        public TypeContainer type;
        private List<SSlotInventory> Items;

        public List< ListItemsInspector>ListInspector = new List<ListItemsInspector>();

        [SerializeField]private int maxSlot = 6;
               
        IFpsPlayer player;
        private InputManager PInputManager;
        public InputManager InputManager
        {
            get
            {
                if ( PInputManager == null )
                {
                    PInputManager = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ( );
                }
                return PInputManager;
            }
        }

           
        // Start is called before the first frame update
        void Awake ( )
        {
            
            Items = new List<SSlotInventory>();
        }
        private void Update ( )
        {
            ListInspector.Clear ( );
            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                ListInspector.Add ( new ListItemsInspector ( Items[i].GetSlotIndex ( ) , Items[i].GetSItem ( ).name ) );

            }
        }

        public void Clear ( )
        {
            Items.Clear ( );
        }
   
        public bool AddItem ( int slotIndex , SSlotInventory slot  )
        {
            if ( maxSlot == Items.Count )
            {
                print ( type.ToString ( ) + "cheio" );
                return false;
            }
            slot.SetSlotIndex ( slotIndex );
            Items.Add ( slot );
            Debug.Log ( "Added item: " + slot.GetSItem ( ).name +"  " +type.ToString ( ) );
            OnInventoryAltered?.Invoke ( );
            return true;
        }
        public bool AddItem ( SSlotInventory slot )
        {
            if (slot ==null)
            {
                print ( "ITEM null" );
                return false;
            }
            if ( maxSlot == Items.Count  )
            {
                print ( "inventario cheio");
                return false;
            }
            int posicao = -1;
            if (CheckFreeSpace( ref posicao ) )
            {
                print ( "posciasodsaod" + posicao );
                slot.SetSlotIndex ( posicao );
              
            }
            Items.Add ( slot );
            Debug.Log ( "Added item: " + slot.GetSItem ( ).name + "  " + type.ToString ( ) );
            OnInventoryAltered?.Invoke ( );
            return true;
        }

        public bool CheckFreeSpace (ref int posicao )
        {
            bool isfreespace = false;

            for ( int i = 0 ; i < maxSlot ; i++ )
            {
                SSlotInventory item =  GetSlotContainer (i );
                if ( item ==null)
                {
                posicao = i;
                isfreespace = true;
                break;
                }
            }
            return isfreespace;
        }

        public bool CheckIfItemExist (int slotIndex )
        {
            return Items.TrueForAll ( item => item.GetSlotIndex ( ) == slotIndex );
        }

        public void RemoveItem ( SSlotInventory item  )
        {

            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                if ( Items[i].GetSlotIndex() == item.GetSlotIndex())
                {
                    Items.Remove ( Items[i] );
                    OnInventoryAltered?.Invoke ( );
                    break;
                }
            }
           

        }
        public void RemoveItem ( int slotIndex )
        {

            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                if ( Items[i].GetSlotIndex ( ) == slotIndex )
                {
                    Items.Remove ( Items[i] );
                    OnInventoryAltered?.Invoke ( );
                    break;
                }
            }


        }
        public void UseItem ( SSlotInventory slotitem , bool closeInventory )
        {
            /*
            Items.ForEach ((slot)=> {

                if ( slot.Compare( slotitem ) )
                {
                    switch ( slot.GetSItem().Type )
                    {
                        case ItemType.none:
                            //faz nada
                            break;
                        case ItemType.weapon:
                            print ("WEAPON" );
                            slot.Use();
                            //euipa
                            break;
                        case ItemType.ammo:
                            slot.Use ( );
                            //recarrega
                            break;
                        case ItemType.consumable:
                            print ( "CONSUMABLE" );
                            slot.Use ( );
                            //CONSUME

                            break;
                    }

                    if ( slot.GetQuantity() == 0 )
                    {
                        slot.SetSItem ( null );
                        slot.SetQuantity ( 0 );
                    }
                    OnInventoryAltered.Invoke ( );
                }
            
            
            
            
            } );
           */
                
        }

        public int GetMaxSlots ( )
        {
            return maxSlot;
        }

        public void SetMaxSlots ( int maxslot )
        {
            maxSlot = maxslot;
        }

        public SSlotInventory GetSlotContainer ( int slotindex )
        {
            SSlotInventory temp = null;
            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                
                    if ( Items[i].GetSlotIndex ( ) == slotindex )
                    {
                        temp = Items[i];
                    }
            }

            return temp;
        }

        public void MoveItem ( int id , int idmove )
        {
            SSlotInventory tempid = GetSlotContainer(id );
            SSlotInventory tempmove = GetSlotContainer(idmove );
            if ( tempid !=null && tempmove !=null)
            {
                tempid.SetSlotIndex ( idmove );
                tempmove.SetSlotIndex ( id );
            }
            else
            {

                if ( tempid == null )
                {

                    tempmove.SetSlotIndex ( id );
                }
                else
                {
                    Items.ForEach (item => {
                        if (item.GetSlotIndex() == tempid.GetSlotIndex())
                        {
                            print ( "movendo");
                            item.SetSlotIndex ( idmove );
                        }
                    } );
                  
                }

            }
            
            OnInventoryAltered?.Invoke ( );
        }

        public InventoryTemp GetContainerTemp ( )
        {
            List<SlotInventoryTemp> list = new List<SlotInventoryTemp>();

            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                SlotInventoryTemp novo;
                    novo = new SlotInventoryTemp ( Items[i].GetSlotIndex() ,Items[i].GetSItem ( ).GuidId , Items[i].GetQuantity ( ) );
                list.Add( novo );
            }


            return  new InventoryTemp ( list , GetMaxSlots ( ) );
        }
        #region COMMAND
        [Command]
        public void CmdGetContainer (TypeContainer type , NetworkConnectionToClient sender = null )
        {
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            IContainer tempContainer = GetContainerCommandd(sender.identity.GetComponent<FpsPlayer> ( ));
            
            TargetGetContainer ( opponentIdentity.connectionToClient , tempContainer.GetContainerTemp ( ) );
        }
        [Command]
        public void CmdMoveSlotContainer ( TypeContainer type , int idselecionado , int identer , NetworkConnectionToClient sender = null )
        {
            sender.identity.GetComponent<Container> ( ).MoveItem ( idselecionado , identer );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetContainer ( opponentIdentity.connectionToClient , sender.identity.GetComponent<Container> ( ).GetContainerTemp ( ) );
        }
        [Command]
        public void CmdAddSlotContainer ( TypeContainer type , UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {

            ScriptableItem item = ScriptableManager.GetScriptable(slot.slot.guidid) ;
            SSlotInventory slotnovo;

            if ( item )
            {
               
               slotnovo = new SSlotInventory ( slot.slotIndex, item.sitem , slot.slot.Quantity );
                
               AddItem (slot.slotIndex, slotnovo );

            }

            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetContainer ( opponentIdentity.connectionToClient , sender.identity.GetComponent<Container> ( ).GetContainerTemp ( ) );

        }
        [Command]
        public void CmdRemoveSlotContainer ( TypeContainer type ,UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
            RemoveItem ( slot.slot.slotindex );


            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetContainer ( opponentIdentity.connectionToClient , sender.identity.GetComponent<Container> ( ).GetContainerTemp ( ) );

        }

      
        #endregion

        #region TARGERRPC

        [TargetRpc]
        public void TargetGetContainer ( NetworkConnection target , InventoryTemp inventory )
        {
            SetMaxSlots ( inventory.maxSlot );
            Clear ( );
            for ( int i = 0 ; i < inventory.slot.Count ; i++ )
            {
                ScriptableItem item = ScriptableManager.GetScriptable(inventory .slot[i].guidid);
                if ( item != null)
                {
                    AddItem ( inventory.slot[i].slotindex , new SSlotInventory ( inventory.slot[i].slotindex , item.sitem , inventory.slot[i].Quantity ) );
                }
   
            }
            
        }
        #endregion
    }
}