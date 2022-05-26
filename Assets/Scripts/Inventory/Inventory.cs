using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
namespace ApocalipseZ
{
    [System.Serializable]
    public class SSlotInventory
    {
        private SItem item;
       [SerializeField] private int Quantity;
        public SSlotInventory ( )
        {
            item = null;
            Quantity = 0;
        }
        public SSlotInventory ( SItem _item , int _Quantity )
        {
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
        public SItem GetSItem ( )
        {
            return item;
        }
        public int GetQuantity ( )
        {
            return Quantity;
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
            return new SlotInventoryTemp ( item.GuidId, Quantity );
        }
    }
public class Inventory : NetworkBehaviour,IInventory
    {
        public event Action OnInventoryAltered;

        [SerializeField]private List<SSlotInventory> Items = new List<SSlotInventory>();

        public string[] nomeitems;
      
        [SerializeField]private int maxSlot = 6;

        public bool debug = true;

        public bool isOpen = true;

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

        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
        }
        // Start is called before the first frame update
        void Awake ( )
        {
            for ( int i = 0 ; i < maxSlot ; i++ )
            {
                SSlotInventory temp = new SSlotInventory ( );
                Items.Add ( temp );
            }
            nomeitems = new string[6];
        }
        private void Update ( )
        {
            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                if ( Items[i].ItemIsNull())
                {
                    nomeitems[i] = "NONE";
                }
                else
                {
                    nomeitems[i] = Items[i].GetSItem ( ).name;
                }
                
            }
        }
        public void Clear ( )
        {
            Items.Clear ( );
            for ( int i = 0 ; i < maxSlot ; i++ )
            {
                SSlotInventory temp = new SSlotInventory ( );
                Items.Add ( temp );
            }
        }
        // Update is called once per frame
        void UpdateInventory ( )
        {
            if (InputManager.GetInventory())
            {
                isOpen = !isOpen;
            }
        }

        public bool AddItem ( SSlotInventory slot )
        {
            print ( "entrou aki ");
            int posicao = 0;
            if ( CheckFreeSpace (ref posicao) == false )
            {
                return false;
            }
           
            Items[posicao].SetSItem( slot .GetSItem());
            Items[posicao].SetQuantity ( slot.GetQuantity ( ) );

            if ( debug ) Debug.Log ( "Added item: " + slot.GetSItem().name ); 

            //Events
            // item.onPickupEvent.Invoke ( );
            OnInventoryAltered?.Invoke ( );
            return true;
        }
        public bool AddItem ( SSlotInventory slot, int index )
        {
            if ( slot.ItemIsNull()) 
            {
                Items[index].SetSItem ( null);
                Items[index].SetQuantity ( 0);
                OnInventoryAltered.Invoke ( );
                return false;
            }
            Items[index].SetSItem ( slot.GetSItem ( ) );
            Items[index].SetQuantity ( slot.GetQuantity ( ) );

            if ( debug ) Debug.Log ( "Added item: " + slot.GetSItem ( ).name );

            //Events
            // item.onPickupEvent.Invoke ( );
            OnInventoryAltered.Invoke ( );
            return true;
        }

        public bool CheckFreeSpace (ref int posicao )
        {
            bool isfreespace = false;

            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                if (Items[i].GetSItem() == null)
                {
                    posicao = i;
                isfreespace = true;
                break;
                }
            }
            return isfreespace;
        }

        public bool CheckIfItemExist ( SSlotInventory item )
        {
            bool Exist = false;

            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                if ( Items[i].Compare ( item ) )
                {
                    Exist = true;
                    break;
                }
            }
            return Exist;
        }

        public void RemoveItem ( SSlotInventory item , bool destroy )
        {
            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                if ( Items[i].GetSItem() == item.GetSItem())
                {
                    Items[i].SetSItem ( null );
                    Items[i].SetQuantity (0);
                    OnInventoryAltered.Invoke ( );
                    break;
                }
            }

        }

   

        public void UseItem ( SSlotInventory slotitem , bool closeInventory )
        {
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
           
                
        }

        public int GetMaxSlots ( )
        {
            return maxSlot;
        }

        public void SetMaxSlots ( int maxslot )
        {
            maxSlot = maxslot;
        }

        public SSlotInventory GetSlotInventory ( int index )
        {
            return Items[index];
        }

        public void MoveItem ( int id , int idmove )
        {
            SSlotInventory slottemp = Items[idmove];
            Items[idmove] = Items[id];
            Items[id] = slottemp;
            OnInventoryAltered.Invoke ( );
        }

        public InventoryTemp GetInventoryTemp ( )
        {
            List<SlotInventoryTemp> list = new List<SlotInventoryTemp>();

            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                SlotInventoryTemp novo;
                if (Items[i].ItemIsNull())
                {
                     novo = new SlotInventoryTemp("",0);
                }
                else
                {
                    novo = new SlotInventoryTemp ( Items[i].GetSItem ( ).GuidId , Items[i].GetQuantity ( ) );
                }

                list.Add( novo );
            }


            return  new InventoryTemp ( list , GetMaxSlots ( ) );
        }
        #region COMMAND
        [Command]
        public void CmdGetInventory ( NetworkConnectionToClient sender = null )
        {

            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetInventory ( opponentIdentity.connectionToClient , sender.identity.GetComponent<Inventory> ( ).GetInventoryTemp ( ) );

        }
        [Command]
        public void CmdMoveSlotInventory ( int idselecionado , int identer , NetworkConnectionToClient sender = null )
        {
            sender.identity.GetComponent<Inventory> ( ).MoveItem ( idselecionado , identer );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetInventory ( opponentIdentity.connectionToClient , sender.identity.GetComponent<Inventory> ( ).GetInventoryTemp ( ) );
        }
        [Command]
        public void CmdAddSlotInventory ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
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
            AddItem ( slotnovo );

            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetInventory ( opponentIdentity.connectionToClient , sender.identity.GetComponent<Inventory> ( ).GetInventoryTemp ( ) );

        }
        [Command]
        public void CmdRemoveSlotInventory ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
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
                RemoveItem ( slotnovo , true );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetInventory ( opponentIdentity.connectionToClient , sender.identity.GetComponent<Inventory> ( ).GetInventoryTemp ( ) );

        }
        #endregion

        #region TARGERRPC

        [TargetRpc]
        public void TargetGetInventory ( NetworkConnection target , InventoryTemp inventory )
        {
            SetMaxSlots ( inventory.maxSlot );
            Clear ( );
            for ( int i = 0 ; i < inventory.maxSlot ; i++ )
            {
                ScriptableItem item = ScriptableManager.GetScriptable(inventory .slot[i].guidid);
                if ( item == null)
                {
                    target.identity.GetComponent<FpsPlayer> ( ).GetInventory ( ).AddItem ( new SSlotInventory() , i );
                }
                else
                {
                    target.identity.GetComponent<FpsPlayer> ( ).GetInventory ( ).AddItem ( new SSlotInventory( item .sitem, inventory .slot[i].Quantity) , i );
                }
            }
        }
        #endregion
    }
}