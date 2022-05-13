using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    [CreateAssetMenu ( fileName = "SCP_" , menuName = "ScriptableObjects/Item" , order = 1 )]
    public class ScriptableItem : ScriptableObject
    {
        /// public ETypeItem Type;
        public SItem sitem = new SItem();
              

    }
}
