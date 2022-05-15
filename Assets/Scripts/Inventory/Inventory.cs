using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace ApocalipseZ
{
    [System.Serializable]
    public class SSlotInventory
    {
       
        public SItem item ;
        public int Quantity;
        public SSlotInventory ( )
        {
          
            item = null;
            Quantity = 0;
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

    }
public class Inventory : MonoBehaviour,IInventory
    {
        public event Action OnInventoryAltered;

        [SerializeField]private List<SSlotInventory> Items = new List<SSlotInventory>();

        [SerializeField]private int maxSlot = 6;

        public bool debug = true;

        public bool isOpen = true;


        IFpsPlayer player;

     

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
        }

        // Update is called once per frame
        void UpdateInventory ( )
        {
            if (InputManager.instance.GetInventory())
            {
                isOpen = !isOpen;
            }
        }

        public bool AddItem ( SSlotInventory slot )
        {
            int posicao = 0;
            if ( CheckFreeSpace (ref posicao) == false )
            {
                return false;
            }
           
            Items[posicao]  = slot;
           
            if ( debug ) Debug.Log ( "Added item: " + slot.item.name ); 

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
                if (Items[i].item == null)
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
                if ( Items[i].item == item.item)
                {
                    Items[i] = new SSlotInventory ( );
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
                    switch ( slot.item.Type )
                    {
                        case ItemType.none:
                            //faz nada
                            break;
                        case ItemType.weapon:
                            print ("WEAPON" );
                            slot.Quantity--;
                            //euipa
                            break;
                        case ItemType.ammo:
                            slot.Quantity--;
                            //recarrega
                            break;
                        case ItemType.consumable:
                            print ( "CONSUMABLE" );
                            slot.Quantity--;
                            //CONSUME

                            break;
                    }

                    if ( slot.Quantity == 0 )
                    {
                        slot = new SSlotInventory (  );
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
    }
}