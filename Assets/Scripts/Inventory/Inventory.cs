using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace ApocalipseZ
{
    [System.Serializable]
    public struct SSlotInventory
    {

        public SItem item ;
        public int Quantity;
        public SSlotInventory (string none)
        {
            item = new SItem ( none);
            Quantity = 0;
        }
        public bool Compare ( SItem other )
        {
            if ( item.Compare(other) )
            {
                return true;
            }

            return false;
        }
    

    }
public class Inventory : MonoBehaviour,IInventory
    {
        [System.Serializable]
        public class OnAddItem : UnityEvent { }

        [SerializeField]private List<SSlotInventory> Items = new List<SSlotInventory>();

        [SerializeField]private int maxSlot = 6;

        public bool debug = true;

        public bool isOpen = true;

        public OnAddItem onAddItem;
        // Start is called before the first frame update
        void Start ( )
        {
            for ( int i = 0 ; i < maxSlot ; i++ )
            {
                Items.Add ( new SSlotInventory ("NONE"));
            }
        }

        // Update is called once per frame
        void UpdateInventory ( )
        {
            if (InputManager.instance.GetInventory())
            {
                isOpen = !isOpen;
            }
        }

        public bool AddItem ( SSlotInventory slot )
        {
            int posicao = 0;
            if ( CheckFreeSpace (ref posicao) == false )
            {
                return false;
            }
            Items.Insert (posicao, slot );
            
            if ( debug ) Debug.Log ( "Added item: " + slot.item.name );

            //Events
           // item.onPickupEvent.Invoke ( );
            onAddItem.Invoke ( );
            return true;
        }

        public Item CheckForItem ( SItem item )
        {
            throw new System.NotImplementedException ( );
        }

        public bool CheckFreeSpace ( ref int posicao)
        {
            bool isfreespace = false;

            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                if (Items[i].Compare(new SItem("NONE")))
                {
                    posicao = i;
                    isfreespace = true;
                    break;
                }
            }
            print ( isfreespace );
            return isfreespace;
        }

        public bool CheckIfItemExist ( SItem item )
        {
            bool Exist = false;

            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                if ( Items[i].Compare ( item ) )
                {
                    Exist = true;
                    break;
                }
            }
            return Exist;
        }

        public void RemoveItem ( SItem item , bool destroy )
        {
            for ( int i = 0 ; i < Items.Count ; i++ )
            {
                if ( Items[i].Compare( item ))
                {
                    Items.RemoveAt ( i );
                }
            }
        }

   

        public void UseItem ( SItem item , bool closeInventory )
        {
            throw new System.NotImplementedException ( );
        }

       
    }
}