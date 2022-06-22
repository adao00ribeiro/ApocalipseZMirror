using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
namespace ApocalipseZ
{
    [System.Serializable]
    public struct BulletsContainer
    {
        public string name;
        public GameObject Prefab;
 

        public BulletsContainer ( string name , GameObject gameObject ) : this ( )
        {
            this.name = name;
            this.Prefab = gameObject;
        }
    }
    public class ScriptableManager :MonoBehaviour
    {
        private static ScriptableManager _instance;
        public static ScriptableManager Instance
        {
            get
            {

                return _instance;
            }
        }

        public BulletsContainer[] BulletsContainer;
        [SerializeField]private static ScriptableItem[] ItemsWeapons;
        [SerializeField]private static ScriptableItem[] ItemsConsumable;
        [SerializeField]private static ScriptableTextureSounds ScriptableTextureSounds;
        private void Awake ( )
        {
            //registrar 
            GameObject[] bullets =  Resources.LoadAll<GameObject> ( "Prefabs/Weapons/Bullets" );
            BulletsContainer = new BulletsContainer[bullets.Length];
            for ( int i = 0 ; i < bullets.Length ; i++ )
            {
                BulletsContainer[i] = new BulletsContainer ( bullets[i].name , bullets[i]);
            }
            ItemsWeapons = Resources.LoadAll<ScriptableItem> ( "Scriptables/ItemWeaponData" );
            ItemsConsumable = Resources.LoadAll<ScriptableItem> ( "Scriptables/ItemsConsumableData" );
            ScriptableTextureSounds = Resources.Load<ScriptableTextureSounds> ( "Scriptables/SCP_TextureSound" );
        }
        private void Start ( )
        {
            if ( _instance != null && _instance != this )
            {
                Destroy ( this.gameObject );
            }
            else
            {
                _instance = this;
            }
        }
        public  void Print ( )
        {
            for ( int i = 0 ; i < ItemsWeapons.Length ; i++ )
            {
                Debug.Log ( ItemsWeapons[i].sitem.name);
            }
        }
        public  ScriptableItem GetScriptable ( string guidid )
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
        public  ScriptableItem[] GetItemsWeapons ( )
        {
            return ItemsWeapons;
        }
	    public  ScriptableItem[] GetItemsConsumable ( )
	    {
		    return ItemsConsumable;
	    }
        public  ScriptableTextureSounds GetScriptableTextureSounds ( )
        {
            return ScriptableTextureSounds;
        }

        public GameObject GetBullet ( string nameBullet)
        {
            GameObject bullettemp = null;
            foreach ( BulletsContainer item in BulletsContainer )
            {
                if ( nameBullet ==item.name)
                {
                    bullettemp = item.Prefab;
                    break;
                }
            }
            return bullettemp;
        }

        internal GameObject GetPrefab ( ItemType type )
        {

            GameObject objeto = null;
            ScriptableItem[]items;

            switch ( type )
            {
                case ItemType.consumable:
                    items = ScriptableManager.Instance.GetItemsConsumable ( );
                    objeto = items[Random.Range ( 0 , items.Length )].sitem.Prefab;
                    break;
                case ItemType.weapon:
                    items = ScriptableManager.Instance.GetItemsWeapons ( );
                    objeto = items[Random.Range ( 0 , items.Length )].sitem.Prefab;
                    break;
            }
            return objeto;
        }
    }
}