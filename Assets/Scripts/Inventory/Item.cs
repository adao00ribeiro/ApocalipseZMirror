using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace ApocalipseZ
{
    public enum ItemType : byte { none, weapon, ammo, consumable }
    [System.Serializable]
    public class SItem
    {
        [SerializeField] public string GuidId =  System.Guid.NewGuid().ToString ();

        [SerializeField]public ItemType Type;

        [SerializeField]public string name ;
               

        [SerializeField]public bool isStackable;


        [SerializeField]public Sprite Thumbnail;


        [SerializeField]public string Description;

        [SerializeField]public int maxStacksize ;

        [SerializeField]public int Ammo ;

        [SerializeField] public int addHealth;
        [SerializeField] public int addSatiety;
        [SerializeField] public int  addHydratation;

        [SerializeField]public float Durability ;


        [SerializeField]public GameObject Prefab;


        public SItem ()
        {
            this.Type = ItemType.none;
            this.name = "NONE";
            this.isStackable = true;
            this.Thumbnail = null;
            this.Description = "NONE";
            this.maxStacksize = 4;
            this.Ammo = 0;
            this.Durability = 0.0f;
            this.Prefab = null;
        }

        internal bool Compare ( SItem sItem )
        {
            if ( this.Type == sItem.Type&&
                 this.name == sItem.name &&
                 this.isStackable == sItem.isStackable &&
                 this.Thumbnail == sItem.Thumbnail &&
                 this.Description == sItem.Description &&
                 this.maxStacksize == sItem.maxStacksize &&
                 this.Ammo == sItem.Ammo &&
                 this.Durability     == sItem .Durability        &&
                 this.Prefab         == sItem.Prefab        )
            {
                return true;
            }
            return false;
        }
    }

    [RequireComponent ( typeof ( NetworkTransform ) )]
    public class Item : NetworkBehaviour,IInteract
    {
        [SerializeField]private  ScriptableItem scriptableitem;
        private int Ammo;
        [SerializeField]private int dropQuantity;

        public bool IsServerSpaw = false;
        private void OnEnable ( )
        {
            transform.position += Vector3.up * 2; 
        }
        private void Awake ( )
        {
            Ammo = scriptableitem.sitem.Ammo;
        }
        [Server]
        void Start ( )
        {
            if (!IsServerSpaw )
            {
                NetworkBehaviour.Destroy ( gameObject , 30 );
            }
            
        }
        
        // Update is called once per frame
        void Update ( )
        {

        }
        public void SetAmmo (int _ammo )
        {
            Ammo = _ammo;
        }
        public void EndFocus ( )
        {
            print ("end focus" );
        }

        public string GetTitle ( )
        {
	        return scriptableitem.sitem.name;
        }

        
        public void OnInteract (IFpsPlayer player)
        {   
            IContainer inventory = player.GetInventory();
            SSlotInventory slot = new SSlotInventory();
            slot.SetSItem(scriptableitem.sitem);
            slot.SetAmmo ( Ammo );
            slot.SetQuantity ( dropQuantity);
            Vector3 point = transform.position;
            if ( inventory.AddItem ( slot) )
            {
                SoundManager.instance.Pickup ( );
                if ( IsServerSpaw )
                {
                    Timer.Instance.Add ( ( ) => {
                       
                        SpawObjects.Spawn ( ScriptableManager.Instance.GetPrefab ( scriptableitem.sitem.Type ) , point );
                    } , 4 );
                }
                NetworkBehaviour.Destroy ( gameObject );
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

        [Command ( requiresAuthority = false )]
        public void CmdInteract ( NetworkConnectionToClient sender = null )
        {
            OnInteract ( sender.identity.GetComponent<FpsPlayer>());
        }
    }
}