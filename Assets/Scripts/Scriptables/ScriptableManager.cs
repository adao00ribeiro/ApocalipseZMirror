using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class ScriptableManager :MonoBehaviour
    {
        [SerializeField]private static ScriptableItem[] ItemsWeapons;
        [SerializeField]private static ScriptableItem[] ItemsConsumable;

        private void Start ( )
        {
            ItemsWeapons = Resources.LoadAll<ScriptableItem> ( "Scriptables/ItemWeaponData" );
            ItemsConsumable = Resources.LoadAll<ScriptableItem> ( "Scriptables/ItemsConsumableData" );
        }
      
        public static void Print ( )
        {
            for ( int i = 0 ; i < ItemsWeapons.Length ; i++ )
            {
                Debug.Log ( ItemsWeapons[i].sitem.name);
            }
        }
        public static ScriptableItem GetScriptable ( string guidid )
        {
            ScriptableItem temp = null;

            for ( int i = 0 ; i < ItemsWeapons.Length ; i++ )
            {
                if ( ItemsWeapons[i].sitem.GuidId.ToString ( ) == guidid )
                {
                    temp = ItemsWeapons[i];
                    break;
                }
            }

            return temp;
        }
     
    }
}