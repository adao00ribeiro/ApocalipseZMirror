using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace ApocalipseZ
{
    public class FastItems : MonoBehaviour,IFastItems
    {
        public event Action OnFastItemsAltered;

        public List<SSlotInventory> FastSlots = new List<SSlotInventory>();
        public int maxSlots = 3;

        IInventory inventory;
        // Start is called before the first frame update
        void Awake ( )
        {
            for ( int i = 0 ; i < maxSlots ; i++ )
            {
                FastSlots.Add ( new SSlotInventory());
            }
        }
        public void SetInventory ( IInventory inventory )
        {
            this.inventory = inventory;
        }
        // Update is called once per frame
      
        public void SlotChange (int switchSlotIndex )
        {
          
            if ( !FastSlots[switchSlotIndex-3].ItemIsNull())
            {
                FastSlots[switchSlotIndex-3].Use ( );
            }
           
        }
        public SSlotInventory GetSlotFastItems ( int id)
        {
           
            return FastSlots[id];
        }
        public void SetFastSlots ( int id, SSlotInventory slot )
        {
           
            if ( FastSlots[id].ItemIsNull())
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
            OnFastItemsAltered.Invoke ( );
        }

        public void RemoveSlot ( SSlotInventory slot )
        {
            for ( int i = 0 ; i < FastSlots.Count ; i++ )
            {
                if ( FastSlots[i].GetSItem() == slot.GetSItem ( ) )
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
            OnFastItemsAltered.Invoke ( );
        }

        public InventoryTemp GetFastItemsTemp ( )
        {
            InventoryTemp temp = new InventoryTemp(FastSlots,maxSlots);
            return temp;
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
    }
}