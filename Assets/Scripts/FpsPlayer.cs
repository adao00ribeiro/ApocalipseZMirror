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
         
            //
            Moviment = GetComponent<Moviment> ( );
            WeaponManager = GetComponent<WeaponManager> ( );

            FastItems = GetComponent<FastItems> ( );
            FastItemsManager  = GameObject.Find ( "Canvas Inventory" ).GetComponent<FastItemsManager> ( );

            Inventory = transform.Find ( "Inventory" ).GetComponent<Inventory> ( );
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

            if (InputManager.instance.GetAlpha3())
            {
                CmdGetInventory ( this );
            }
            Animation ( );
            Moviment.UpdateMoviment ( );
            InteractObjects.UpdateInteract ( );
            transform.rotation = Quaternion.Euler ( 0 , GameObject.FindObjectOfType<CinemachinePovExtension> ( ).GetStartrotation ( ).x , 0 );

        }
        [Command]
        void CmdGetInventory ( FpsPlayer item )
        {

            NetworkIdentity opponentIdentity = item.GetComponent<NetworkIdentity>();
            TargetGetInventory ( opponentIdentity.connectionToClient , Inventory.GetInventoryTemp()) ;

        }
        [TargetRpc]
        public void TargetGetInventory ( NetworkConnection target , InventoryTemp inventory )
        {

            Inventory.SetMaxSlots ( inventory.maxSlot);

            Inventory.Clear ( );


            for ( int i = 0 ; i < inventory.maxSlot ; i++ )
            {
                if ( !inventory.slot[i].ItemIsNull())
                {
                    Inventory.AddItem ( inventory.slot[i] );
                }
                
               
            }
           

        }
        public void Animation ( )
        {
            //animatorcontroller
            AnimatorController.SetFloat ( "Horizontal" , InputManager.instance.GetMoviment ( ).x );
            AnimatorController.SetFloat ( "Vertical" , InputManager.instance.GetMoviment ( ).y );
            AnimatorController.SetBool ( "IsJump" , !Moviment.isGrounded ( ) );
            AnimatorController.SetBool ( "IsRun" , Moviment.CheckMovement ( ) && InputManager.instance.GetRun ( ) );


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
    }
}