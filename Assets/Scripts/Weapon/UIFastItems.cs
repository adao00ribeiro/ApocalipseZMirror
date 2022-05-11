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

        public void SetFastItems ( IFastItems _fastitems )
        {
            FastItems = _fastitems;
        }
      
     

    }
}