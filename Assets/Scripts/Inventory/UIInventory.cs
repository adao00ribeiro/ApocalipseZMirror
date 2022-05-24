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
       

        public void SetFpsPlayer ( IFpsPlayer _player)
        {
            player = _player;
          
            slotPanel = transform.Find ( "SlotPanel" ).transform;
            player.GetInventory().OnInventoryAltered += UpdateSlots; ;
            UpdateSlots ( );

        }
       
        public void UpdateSlots ( )
        {
          
            foreach ( UISlotItem item in UIItems )
            {
              
                Destroy ( item.gameObject);
            }
            UIItems.Clear();
            for ( int i = 0 ; i < player.GetInventory().GetMaxSlots ( ) ; i++ )
            {
                UISlotItem instance = Instantiate(PrefabSlot,slotPanel);
                instance.SetFpsPlayer (player );
                instance.UpdateSlot ( i );
                UIItems.Add ( instance );
            }
        }

    }
}