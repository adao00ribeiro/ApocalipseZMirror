using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class ScriptableManager :MonoBehaviour
    {
        public static GameObject bullet;
        public static GameObject Rocket;
        [SerializeField]private static ScriptableItem[] ItemsWeapons;
        [SerializeField]private static ScriptableItem[] ItemsConsumable;
        [SerializeField]private static ScriptableTextureSounds ScriptableTextureSounds;
        private void Awake ( )
        {
            bullet = Resources.Load<GameObject> ( "Prefabs/Weapons/Gun bullet" );
            Rocket = Resources.Load<GameObject> ( "Prefabs/Weapons/Rocket" );
            ItemsWeapons = Resources.LoadAll<ScriptableItem> ( "Scriptables/ItemWeaponData" );
            ItemsConsumable = Resources.LoadAll<ScriptableItem> ( "Scriptables/ItemsConsumableData" );
            ScriptableTextureSounds = Resources.Load<ScriptableTextureSounds> ( "Scriptables/SCP_TextureSound" );
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
             for ( int i = 0 ; i < ItemsConsumable.Length ; i++ )
            {
                if ( ItemsConsumable[i].sitem.GuidId.ToString ( ) == guidid )
                {
                    temp = ItemsConsumable[i];
                    break;
                }
            }

            return temp;
        }
        public static ScriptableItem[] GetItemsWeapons ( )
        {
            return ItemsWeapons;
        }
	    public static ScriptableItem[] GetItemsConsumable ( )
	    {
		    return ItemsConsumable;
	    }
        public static ScriptableTextureSounds GetScriptableTextureSounds ( )
        {
            return ScriptableTextureSounds;
        }
    }
}