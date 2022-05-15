using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class UIFastItems : MonoBehaviour
    {
        public UISlotItem Primary;
        public UISlotItem Second;

        public List<UISlotItem> FastSlot = new List<UISlotItem>();

        IInventory inventory;
        IFastItems FastItems;
        IWeaponManager WeaponManager;
        public void SetInventory ( IInventory _inventory )
        {
            inventory = _inventory;
            Primary.SetInventory ( inventory );
            Second.SetInventory ( inventory );
            FastSlot.ForEach ( ( item ) => {
                item.SetInventory ( inventory );
            } );
        }


        public void SetFastItems ( IFastItems _fastitems )
        {
            FastItems = _fastitems;
            FastItems.OnFastItemsAltered += UpdateSlotsFastItems; ;

            Primary.SetFastItems ( FastItems );
            Second.SetFastItems ( FastItems );
            FastSlot.ForEach ( ( item ) => { 
            item.SetFastItems ( FastItems );
            } );

            UpdateSlotsFastItems ( );
        }
        public void UpdateSlotsFastItems ( )
        {
            for ( int i = 0 ; i < FastSlot.Count ; i++ )
            {
                FastSlot[i].UpdateSlotFastItems ( i ); 
            }
          
        }

        internal void SetWeaponManager ( IWeaponManager weaponManager )
        {
            this.WeaponManager = weaponManager;
            weaponManager.OnWeaponAltered += UpdateSlotsWeapons; ;
            Primary.SetWeaponManager ( WeaponManager );
            Second.SetWeaponManager ( WeaponManager );
            FastSlot.ForEach ( ( item ) => {
                item.SetWeaponManager ( WeaponManager );
            } );
            UpdateSlotsWeapons ( );
        }

        private void UpdateSlotsWeapons ( )
        {
            Primary.UpdateSlotWeapons ( 0 );
            Second.UpdateSlotWeapons ( 1 );
        }
    }
}