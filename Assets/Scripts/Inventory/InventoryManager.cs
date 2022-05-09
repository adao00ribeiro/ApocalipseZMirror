using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    public class InventoryManager : MonoBehaviour,IInventoryMananger
    {
        IFpsPlayer player;

        private Canvas canvas;

        public void InventoryClose ( )
        {
            canvas.enabled = false;
        }

        public void InventoryOpen ( )
        {
            canvas.enabled = true;
        }

        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
        }

      
    }
}