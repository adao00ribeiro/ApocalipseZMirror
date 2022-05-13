using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class FastItemsManager : MonoBehaviour, IFastItemsManager
    {
        IFpsPlayer player;
        public int switchSlotIndex = 0;
        IFastItems FastItems;
        UIFastItems uiFastItems;
        // Start is called before the first frame update
        void Awake ( )
        {
            uiFastItems = transform.Find ( "HUD/Weapons end FastSlots UI" ).GetComponent<UIFastItems> ( );
        }

       
        void Update ( )
        {
            SlotInput ( );
        }

        private void SlotInput ( )
        {
            if ( InputManager.instance.GetAlpha3 ( ) ) {  FastItems.SlotChange (3 ); }
            else if ( InputManager.instance.GetAlpha4 ( ) ) {  FastItems.SlotChange (4 ); }
            else if ( InputManager.instance.GetAlpha5 ( ) ) { ; FastItems.SlotChange (5 ); }

        }
        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
            FastItems = player.GetFastItems ( );

            uiFastItems.SetFastItems ( FastItems );
            uiFastItems.SetWeaponManager ( player.GetWeaponManager());
        }
    }
}