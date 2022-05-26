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
        UiFastItems uiFastItems;
        private InputManager PInputManager;
        public InputManager InputManager
        {
            get
            {
                if ( PInputManager == null )
                {
                    PInputManager = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ( );
                }
                return PInputManager;
            }
        }
        // Start is called before the first frame update
        void Awake ( )
        {
            uiFastItems = transform.Find ( "HUD/Weapons end FastSlots UI" ).GetComponent<UiFastItems> ( );
        }

       
        void Update ( )
        {
            SlotInput ( );
        }

        private void SlotInput ( )
        {
            if ( InputManager.GetAlpha3 ( ) ) {  FastItems.SlotChange (3 ); }
            else if ( InputManager.GetAlpha4 ( ) ) {  FastItems.SlotChange (4 ); }
            else if ( InputManager.GetAlpha5 ( ) ) { ; FastItems.SlotChange (5 ); }

        }
        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
            FastItems = player.GetFastItems ( );
            FastItems.SetInventory ( player.GetInventory ( ) );
            uiFastItems.SetFpsPlayer ( player);
           
        }
    }
}