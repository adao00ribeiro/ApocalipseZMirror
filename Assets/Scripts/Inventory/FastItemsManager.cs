using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace ApocalipseZ
{
    public class FastItemsManager : NetworkBehaviour, IFastItemsManager
    {
        IFpsPlayer player;
        public int switchSlotIndex = 0;
        IContainer FastItems;

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

        void Update ( )
        {
            if ( InputManager.GetAlpha3 ( ) )
            {
                SlotChange ( 0 );
            }
            else if ( InputManager.GetAlpha4 ( ) )
            {
                SlotChange ( 1);
            }
            else if ( InputManager.GetAlpha5 ( ) )
            {
                SlotChange ( 2 );
            }
        }

        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
            FastItems = player.GetFastItems ( );
   
        }
        public void SlotChange ( int slotindex )
        {

            FastItems.UseItem ( slotindex );
        }

    }
}