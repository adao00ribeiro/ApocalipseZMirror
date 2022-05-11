using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    public class UIInventory : MonoBehaviour
    {
        public UISlotItem PrefabSlot;
        [SerializeField]private List<UISlotItem> UIItems = new List<UISlotItem>();
        private Transform slotPanel;

        IInventory inventory;
  
        public void SetInventory ( IInventory _inventory)
        {
            inventory = _inventory;
            slotPanel = transform.Find ( "SlotPanel").transform;
            inventory.OnInventoryAltered += UpdateSlots; ;
            UpdateSlots ( );    
        }
        public void UpdateSlots ( )
        {
            foreach ( var item in UIItems )
            {
                Destroy ( item.gameObject);
            }
            for ( int i = 0 ; i < inventory.GetMaxSlots ( ) ; i++ )
            {
                UISlotItem instance = Instantiate(PrefabSlot,slotPanel);
                instance.UpdateSlot ( inventory.GetSlotInventory ( i ) );
                UIItems.Add ( instance );
            }
        }

    }
}