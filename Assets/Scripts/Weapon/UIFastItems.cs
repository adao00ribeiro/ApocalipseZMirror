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

        IFastItems FastItems;
        IWeaponManager WeaponManager;
        public void SetFastItems ( IFastItems _fastitems )
        {
            FastItems = _fastitems;
            FastItems.OnFastItemsAltered += UpdateSlots; ;

            Primary.SetFastItems ( FastItems );
            Second.SetFastItems ( FastItems );
            FastSlot.ForEach ( ( item ) => { 
            item.SetFastItems ( FastItems );
            } );

            UpdateSlots ( );
        }
        public void UpdateSlots ( )
        {
            for ( int i = 0 ; i < FastSlot.Count ; i++ )
            {
                FastSlot[i].UpdateSlotFastItems ( i ); 
            }
          
        }

        internal void SetWeaponManager ( IWeaponManager weaponManager )
        {
            this.WeaponManager = weaponManager;
        }

    }
}