using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    [CreateAssetMenu ( fileName = "SCP_" , menuName = "ScriptableObjects/Item" , order = 1 )]
    public class ScriptableItem : ScriptableObject
    {
        /// public ETypeItem Type;
        public SItem sitem;
              
        public ScriptableItem ( )
        {

            this.sitem.name = "NONE";
            this.sitem.isStackable = true;
            this.sitem.Thumbnail = null;
            this.sitem.Description = "NONE";
            //thisitem.s.isConsumable = true;
            this.sitem.maxStacksize = 4;
            this.sitem.Durability = 0.0f;
            this.sitem.Prefab = null;
        }

    }
}
