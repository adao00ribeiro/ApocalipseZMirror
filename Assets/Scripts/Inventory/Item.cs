using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    [System.Serializable]
    public struct SItem
    {


        [SerializeField]public string name ;


        [SerializeField]public bool isStackable;


        [SerializeField]public Sprite Thumbnail;


        [SerializeField]public string Description;


        [SerializeField]public bool isConsumable ;


        [SerializeField]public int maxStacksize ;


        [SerializeField]public float Durability ;


        [SerializeField]public Item Prefab;


        public SItem ( string name)
        {
          
            this.name = name;
            this.isStackable = true;
            this.Thumbnail = null;
            this.Description = "NONE";
            this.isConsumable = true;
            this.maxStacksize = 4;
            this.Durability = 0.0f;
            this.Prefab = null;
        }

        internal bool Compare ( SItem sItem )
        {
       if ( this.name           == sItem .name              &&
            this.isStackable    == sItem .isStackable       &&
            this.Thumbnail      == sItem .Thumbnail         &&
            this.Description    == sItem .Description       &&
            this.isConsumable   == sItem .isConsumable      &&
            this.maxStacksize   == sItem .maxStacksize      &&
            this.Durability     == sItem .Durability        &&
            this.Prefab         == sItem.Prefab        )
            {
                return true;
            }
            return false;
        }
    }
        public class Item : MonoBehaviour,IInteract
    {
        [SerializeField]private  ScriptableItem scriptableitem;

        [SerializeField]private int dropQuantity;
        // Start is called before the first frame update
        void Start ( )
        {

        }
        
        // Update is called once per frame
        void Update ( )
        {

        }
        public void EndFocus ( )
        {
            print ("end focus" );
        }

        public string GetTitle ( )
        {
            return "TITUASDLKALSDK";
        }

        public void OnInteract (IFpsPlayer player)
        {   
            IInventory inventory = player.GetInventory();
            SSlotInventory slot = new SSlotInventory();
            slot.item = scriptableitem.sitem;
            slot.Quantity = dropQuantity;
         
            if ( inventory.AddItem ( slot) )
            {
                Destroy ( gameObject);
            }
        }

        public void StartFocus ( )
        {
            print ( "StartFocus" );
        }

        private void OnTriggerEnter ( Collider other )
        {
            if ( other.CompareTag("noCollider"))
            {
                StartFocus ( );
            }
        }
        private void OnTriggerExit ( Collider other )
        {
            if ( other.CompareTag ( "noCollider" ) )
            {
                EndFocus ( );
            }
        }
    }
}