using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
using UnityEngine.Events;
namespace ApocalipseZ
{
    //adicionar e conter components
    [RequireComponent ( typeof ( NetworkTransform ) )]
    [RequireComponent ( typeof ( Moviment ) )]
    [RequireComponent ( typeof ( WeaponManager ) )]
    public class FpsPlayer : NetworkBehaviour, IFpsPlayer
    {
        public static NetworkConnectionToClient networkConnectionToClient;
        public event System.Action<FpsPlayer> OnLocalPlayerJoined;
        IMoviment Moviment;
        IFastItems FastItems;
        IFastItemsManager FastItemsManager;
        IWeaponManager WeaponManager;
        IInventory Inventory;
        IInventoryManager InventoryManager;
        IInteractObjects InteractObjects;
            

        //--------------------------------------------
        public Vector2 sensitivity = new Vector2(0.5f, 0.5f);
        public Vector2 smoothing = new Vector2(3, 3);
        public bool isClimbing = true;
        private Vector3 previousPos = new Vector3();

        [SerializeField]private Animator AnimatorController;
        [SerializeField]private Animator AnimatorWeaponHolderController;
        [SerializeField] private GameObject[] mesh;
        public Transform pivohead;
        private bool PlockCursor;

        public bool lockCursor { get => PlockCursor; set => PlockCursor = value; }


        // Start is called before the first frame update
        private void Start ( )
        {
            if ( isLocalPlayer )
            {
                networkConnectionToClient = this.connectionToClient;
            }
           
               //
            Moviment = GetComponent<Moviment> ( );
            WeaponManager = GetComponent<WeaponManager> ( );

            FastItems = GetComponent<FastItems> ( );
            FastItemsManager  = GameObject.Find ( "Canvas Inventory" ).GetComponent<FastItemsManager> ( );

            Inventory = GetComponent<Inventory> ( );
            InventoryManager = GameObject.Find("Canvas Inventory").GetComponent<InventoryManager>();

            InteractObjects = transform.Find ( "Camera & Recoil" ).GetComponent<InteractObjects> ( );
            AnimatorController = transform.Find ( "Ch35_nonPBR" ).GetComponent<Animator> ( );
            AnimatorWeaponHolderController = transform.Find ( "Camera & Recoil/WeaponCamera/Weapon holder" ).GetComponent<Animator> ( );
            OnLocalPlayerJoined += InventoryManager.SetFpsPlayer; ;
            OnLocalPlayerJoined += FastItemsManager.SetFpsPlayer; ;
            OnLocalPlayerJoined?.Invoke ( this );

        }

        public override void OnStartLocalPlayer ( )
        {
           
            //
            for ( int i = 0 ; i < mesh.Length ; i++ )
            {
                mesh[i].layer = 7;
            }
            //
            GameObject.FindObjectOfType<CinemachineVirtualCamera> ( ).Follow = pivohead;
     

        }
        
        // Update is called once per frame
        void Update ( )
        {
            if ( !isLocalPlayer )
            {
                return;
            }

            if ( lockCursor )
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
           
           
            Animation ( );
            Moviment.UpdateMoviment ( );
            InteractObjects.UpdateInteract ( );
            if ( InputManager.instance.GetUse ( ) )
            {
                CmdGetInventory ( );
            }
            transform.rotation = Quaternion.Euler ( 0 , GameObject.FindObjectOfType<CinemachinePovExtension> ( ).GetStartrotation ( ).x , 0 );

        }
      
        public void Animation ( )
        {
            //animatorcontroller
            AnimatorController.SetFloat ( "Horizontal" , InputManager.instance.GetMoviment ( ).x );
            AnimatorController.SetFloat ( "Vertical" , InputManager.instance.GetMoviment ( ).y );
            AnimatorController.SetBool ( "IsJump" , !Moviment.isGrounded ( ) );
            AnimatorController.SetBool ( "IsRun" , Moviment.CheckMovement ( ) && InputManager.instance.GetRun ( ) );
            AnimatorController.SetBool ( "IsCrouch" , InputManager.instance.GetCrouch ( ) );

            //weaponanimator

            AnimatorWeaponHolderController.SetBool ( "Walk" , Moviment.CheckMovement ( ) && Moviment.isGrounded ( ) );
            AnimatorWeaponHolderController.SetBool ( "Run" , Moviment.CheckMovement ( ) && InputManager.instance.GetRun ( ) && Moviment.isGrounded ( ) );
            AnimatorWeaponHolderController.SetBool ( "Crouch" , Moviment.CheckMovement ( ) && InputManager.instance.GetCrouch ( ) && Moviment.isGrounded ( ) );

        }
        public float GetVelocityMagnitude ( )
        {
            var velocity = ((transform.position - previousPos).magnitude) / Time.deltaTime;
            previousPos = transform.position;
            return velocity;
        }

        public IInventory GetInventory ( )
        {
            return Inventory;
        }
        public IFastItems GetFastItems ( )
        {
            return FastItems;
        }

        public IWeaponManager GetWeaponManager ( )
        {
            return WeaponManager;
        }


        #region CMD

        [Command ( requiresAuthority = true )]
        public void CmdGetInventory ( NetworkConnectionToClient sender = null )
        {

            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetInventory ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).Inventory.GetInventoryTemp ( ) );

        }
        [Command ( requiresAuthority = true )]
        public void CmdMoveSlotInventory ( int idselecionado , int identer , NetworkConnectionToClient sender = null )
        {
            sender.identity.GetComponent<FpsPlayer> ( ).Inventory.MoveItem ( idselecionado , identer );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetInventory ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).Inventory.GetInventoryTemp ( ) );
        }
        [Command ( requiresAuthority = true )]
        public void CmdAddSlotInventory ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
            Inventory.AddItem ( slot.slot );

            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetInventory ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).Inventory.GetInventoryTemp ( ) );

        }
        [Command ( requiresAuthority = true )]
        public void CmdRemoveSlotInventory ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
            Inventory.RemoveItem (slot.slot,true);
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetInventory ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).Inventory.GetInventoryTemp ( ) );

        }
        // fast ITEMS
        [Command ( requiresAuthority = true )]
        public void CmdGetFastItems ( NetworkConnectionToClient sender = null )
        {

            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).FastItems.GetFastItemsTemp ( ) );

        }
        [Command ( requiresAuthority = true )]
        public void CmdMoveSlotFastItems ( int idselecionado , int identer , NetworkConnectionToClient sender = null )
        {
            FastItems.MoveItem ( idselecionado,identer);
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).FastItems.GetFastItemsTemp ( ) );
        }
        [Command ( requiresAuthority = true )]
        public void CmdAddSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
            FastItems.SetFastSlots ( slot.id , slot.slot);
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).FastItems.GetFastItemsTemp ( ) );

        }
        [Command ( requiresAuthority = true )]
        public void CmdRemoveSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
            FastItems.RemoveSlot ( slot.slot );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).FastItems.GetFastItemsTemp ( ) );
        }
        #endregion
        #region TARGERRPC

        [TargetRpc]
        public void TargetGetInventory ( NetworkConnection target , InventoryTemp inventory )
        {
            print ( "get inventory target rpc");
            Inventory.SetMaxSlots ( inventory.maxSlot );
            Inventory.Clear ( );
            for ( int i = 0 ; i < inventory.maxSlot ; i++ )
            {
              Inventory.AddItem ( inventory.slot[i] ,i);
            }
           
        }
        [TargetRpc]
        public void TargetGetFastItems ( NetworkConnection target , InventoryTemp fastSlots )
        {
            FastItems.Clear ( );

            for ( int i = 0 ; i < fastSlots.maxSlot ; i++ )
            {
                FastItems.SetFastSlots (  i , fastSlots.slot[i] );
            }

        }
        [TargetRpc]
        public void TargetMoveSlotInventory ( NetworkConnection target ,int idselecionado , int identer)
        {
           
        }
        [TargetRpc]
        public void TargetAddSlotInventory ( NetworkConnection target)
        {
           
        }
        [TargetRpc]
        public void TargetRemoveSlotInventory ( NetworkConnection target )
        {

        }
        [TargetRpc]
        public void TargetMoveSlotFastItems ( NetworkConnection target , string text )
        {

          

        }
        [TargetRpc]
        public void TargetAddSlotFastItems ( NetworkConnection target )
        {

        }
        [TargetRpc]
        public void TargetRemoveSlotFastItems ( NetworkConnection target )
        {

        }
        #endregion



    }
}