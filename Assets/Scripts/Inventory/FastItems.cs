using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class FastItems : MonoBehaviour
    {
       
        public List<SSlotInventory> FastSlots = new List<SSlotInventory>();
        public int switchSlotIndex = 0;
        public int currentWeaponIndex;
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
        void Update ( )
        {
            SlotInput ( );
        }

        private void SlotInput ( )
        {
                 if ( InputManager.instance.GetAlpha3 ( ) ) { switchSlotIndex = 3; SlotChange ( ); }
            else if ( InputManager.instance.GetAlpha4 ( ) ) { switchSlotIndex = 4; SlotChange ( ); }
            else if ( InputManager.instance.GetAlpha5 ( ) ) { switchSlotIndex = 5; SlotChange ( ); }

        }
        private void SlotChange ( )
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