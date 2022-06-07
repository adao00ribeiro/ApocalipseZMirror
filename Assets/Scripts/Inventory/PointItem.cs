﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ramdom = UnityEngine.Random;
namespace ApocalipseZ
{
    public class PointItem : MonoBehaviour
    {
        public  ItemType type;

        public GameObject GetPrefab ( )
        {
            GameObject objeto = null;
            ScriptableItem[]items  ;
            ScriptableManager.GetItemsWeapons ( );

            switch ( type )
            {
                case ItemType.consumable:
                    items = ScriptableManager.GetItemsConsumable ( );
                    objeto = items[Random.Range ( 1 , items.Length )].sitem.Prefab;
                    break;
                case ItemType.weapon:
                    items = ScriptableManager.GetItemsWeapons ( );
                    objeto = items[Random.Range ( 1 , items.Length )].sitem.Prefab;
                    break;
            }
            return objeto;
        }

    }
}