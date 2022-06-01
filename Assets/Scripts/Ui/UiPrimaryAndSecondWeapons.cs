using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ {
    public class UiPrimaryAndSecondWeapons : MonoBehaviour
    {
        [SerializeField]private UISlotItem UiPrimaryWeapon;
        [SerializeField]private UISlotItem UiSecondWeapon;

        IFpsPlayer player;

        private void Awake ( )
        {
            UiPrimaryWeapon = transform.Find ( "Container/Primary Weapon Slot" ).GetComponent<UISlotItem>();
            UiSecondWeapon = transform.Find ( "Container/Second Weapon Slot" ).GetComponent<UISlotItem> ( );
        }
        private void OnEnable ( )
        {
            if ( player == null )
            {
                return;
            }
            CommandsFpsPlayer.CmdGetContainer ( TypeContainer.WEAPONS , player.GetConnection ( ) );
        }

        public void UpdateSlots ( )
        {
            if ( player == null )
            {
                return;
            }
            UiPrimaryWeapon.UpdateSlot ( );
            UiSecondWeapon.UpdateSlot ( );
        }

        internal void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
            UiPrimaryWeapon.SetFpsPlayer ( _player );
            UiPrimaryWeapon.SetContainer ( _player.GetWeaponsSlots ( ) );

            UiSecondWeapon.SetFpsPlayer ( _player );
            UiSecondWeapon.SetContainer ( _player.GetWeaponsSlots ( ) );

            player.GetWeaponsSlots ( ).OnContainerAltered += UpdateSlots; ;
            UpdateSlots ( );
        }
    }
}