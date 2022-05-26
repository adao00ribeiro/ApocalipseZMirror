using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class UiFastItems : MonoBehaviour
    {
        public UISlotItem Primary;
        public UISlotItem Second;

        public List<UISlotItem> FastSlot = new List<UISlotItem>();
        IFpsPlayer player;

        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
            Primary.SetFpsPlayer ( player );
            Second.SetFpsPlayer ( player );
            FastSlot.ForEach ( ( item ) => {
                item.SetFpsPlayer ( player );
            } );
            player.GetFastItems().OnFastItemsAltered += UpdateSlotsFastItems; ;
            player.GetWeaponManager().OnWeaponAltered += UpdateSlotsWeapons; ;

            UpdateSlotsFastItems ( );

            UpdateSlotsWeapons ( );
        }
     
        public void UpdateSlotsFastItems ( )
        {
            for ( int i = 0 ; i < FastSlot.Count ; i++ )
            {
                FastSlot[i].UpdateSlot ( i ); 
            }
          
        }

        private void UpdateSlotsWeapons ( )
        {
            Primary.UpdateSlot ( 0 );
            Second.UpdateSlot ( 1 );
        }
    }
}