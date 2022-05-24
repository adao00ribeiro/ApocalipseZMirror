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
        IFpsPlayer player;
        IInventory inventory;
        IFastItems fastItems;
        IWeaponManager WeaponManager;

        public void SetFpsPlayer ( IFpsPlayer _player)
        {
            player = _player;
        }
        public void SetInventory ( IInventory _inventory)
        {
            inventory = _inventory;
            slotPanel = transform.Find ( "SlotPanel").transform;
            inventory.OnInventoryAltered += UpdateSlots; ;
            UpdateSlots ( );    
        }
        public void SetFastItems ( IFastItems fastItems)
        {
            this.fastItems = fastItems;
        }
        public void SetWeaponManager (IWeaponManager weaponmanager )
        {
            WeaponManager = weaponmanager;
        }
        public void UpdateSlots ( )
        {
          
            foreach ( UISlotItem item in UIItems )
            {
              
                Destroy ( item.gameObject);
            }
            UIItems.Clear();
            for ( int i = 0 ; i < inventory.GetMaxSlots ( ) ; i++ )
            {
                UISlotItem instance = Instantiate(PrefabSlot,slotPanel);
                instance.SetInventory(inventory);
                instance.SetFastItems ( fastItems );
                instance.SetWeaponManager ( WeaponManager );
                instance.UpdateSlotInventory ( i );
                UIItems.Add ( instance );
            }
        }

    }
}