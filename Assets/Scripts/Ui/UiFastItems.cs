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
        private void OnEnable ( )
        {
            if ( player == null )
            {
                return;
            }
            player.GetFastItems ( ).CmdGetFastItems ( );
        }
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
                FastSlot[i].UpdateSlot (  ); 
            }
          
        }

        private void UpdateSlotsWeapons ( )
        {
            Primary.UpdateSlot ( );
            Second.UpdateSlot ( );
        }
    }
}