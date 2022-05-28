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
        private int SlotIndex;
        [SerializeField]private SItem item;
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
public class Inventory : NetworkBehaviour,IInventory
    {
        public event Action OnInventoryAltered;

        private SSlotInventory[] Items;

        public string[] nomeitems;

        int totalItems = 0;
        [SerializeField]private int maxSlot = 6;

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

           
        // Start is called before the first frame update
        void Awake ( )
        {
            nomeitems = new string[6];
            Items = new SSlotInventory[maxSlot];
        }
        private void Update ( )
        {
            for ( int i = 0 ; i < Items.Length ; i++ )
            {
                if ( Items[i] == null)
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

            for ( int i = 0 ; i < maxSlot ; i++ )
            {
                Items[i] = null;
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
            if ( maxSlot == totalItems )
            {
                print ( "inventario cheio");
                return false;
            }
            int posicao = -1;
            if(CheckFreeSpace ( ref posicao ) )
            {
                slot.SetSlotIndex ( posicao );
            }
            Items[posicao] = slot;
            totalItems++;
            Debug.Log ( "Added item: " + slot.GetSItem ( ).name );

            OnInventoryAltered?.Invoke ( );
            return true;
        }

        public bool CheckFreeSpace (ref int posicao )
        {

            bool isfreespace = false;

            for ( int i = 0 ; i < maxSlot; i++ )
            {
                if (Items[i] == null)
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

            for ( int i = 0 ; i < Items.Length ; i++ )
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
          
            

            for ( int i = 0 ; i < Items.Length ; i++ )
            {
                if ( Items[i].GetSlotIndex() == item.GetSlotIndex())
                {
                    print ( "removido");
                    Items[i] = null;
                    totalItems--;
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

            SSlotInventory[] novo = new SSlotInventory[maxslot];

            for ( int i = 0 ; i < Items.Length ; i++ )
            {
                novo[i] = Items[i];
            }

            Items = novo;
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
            OnInventoryAltered?.Invoke ( );
        }

        public InventoryTemp GetInventoryTemp ( )
        {
            List<SlotInventoryTemp> list = new List<SlotInventoryTemp>();

            for ( int i = 0 ; i < Items.Length ; i++ )
            {
                SlotInventoryTemp novo;
                if (Items[i]==null)
                {
                     novo = new SlotInventoryTemp(-1,"",0);
                }
                else
                {
                    novo = new SlotInventoryTemp ( Items[i].GetSlotIndex() ,Items[i].GetSItem ( ).GuidId , Items[i].GetQuantity ( ) );
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
              //  slotnovo = new SSlotInventory ( item.sitem , slot.slot.Quantity );
            }
            else
            {
             //   slotnovo = new SSlotInventory ( null , 0 );
            }
       //     AddItem ( slotnovo );

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
               slotnovo = new SSlotInventory ( slot.slot.slotindex , item.sitem , slot.slot.Quantity );
               RemoveItem ( slotnovo , true );
            }


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
                if ( item != null)
                {
                    target.identity.GetComponent<FpsPlayer> ( ).GetInventory ( ).AddItem ( new SSlotInventory ( inventory.slot[i].slotindex , item.sitem , inventory.slot[i].Quantity ) );
                }
   
            }
            print ( "targer inventory");
        }
        #endregion
    }
}