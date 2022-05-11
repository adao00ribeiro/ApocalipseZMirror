using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class FastItems : MonoBehaviour,IFastItems
    {
       
        public List<SSlotInventory> FastSlots = new List<SSlotInventory>();
 
      
        public int maxSlots = 3;

        IInventory inventory;
        // Start is called before the first frame update
        void Start ( )
        {
            for ( int i = 0 ; i < maxSlots ; i++ )
            {
                FastSlots.Add (new SSlotInventory("NONE"));
            }
        }

        // Update is called once per frame
      
        public void SlotChange (int switchSlotIndex )
        {
          
            if ( !FastSlots[switchSlotIndex-3].item.Compare(new SItem("NONE")))
            {
                FastSlots[switchSlotIndex-3].Use ( );
            }
           
        }

        public void SetFastSlots ( int id, SSlotInventory slot )
        {
            if (id> maxSlots)
            {
                return;
            }
            if ( FastSlots[id].Compare (new SItem("NONE")))
            {
                FastSlots[id] = slot;
            }
            else
            {
                inventory.AddItem ( FastSlots[id] );
                FastSlots[id] = slot;
            }
        }
    }
}