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
        public GameObject PrefabCanvasFpsPlayer;
        public event System.Action<FpsPlayer> OnLocalPlayerJoined;
        CanvasFpsPlayer CanvasFpsPlayer;

        IMoviment Moviment;
        IFastItems FastItems;
        IWeaponManager WeaponManager;
        IInventory Inventory;
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
        // Start is called before the first frame update
        private void Awake ( )
        {
        
               //
            Moviment = GetComponent<Moviment> ( );
            WeaponManager = GetComponent<WeaponManager> ( );
            FastItems = GetComponent<FastItems> ( );
            Inventory = GetComponent<Inventory> ( );
            InteractObjects = transform.Find ( "Camera & Recoil" ).GetComponent<InteractObjects> ( );
            AnimatorController = transform.Find ( "Ch35_nonPBR" ).GetComponent<Animator> ( );
            AnimatorWeaponHolderController = transform.Find ( "Camera & Recoil/WeaponCamera/Weapon holder" ).GetComponent<Animator> ( );
           

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
         
            CanvasFpsPlayer = Instantiate ( PrefabCanvasFpsPlayer ).GetComponent<CanvasFpsPlayer> ( );
            OnLocalPlayerJoined += CanvasFpsPlayer.Init; ;
            OnLocalPlayerJoined?.Invoke ( this );
           
        }
        public override void OnStopLocalPlayer ( )
        {

            Destroy ( CanvasFpsPlayer.gameObject );
        }
        // Update is called once per frame
        void Update ( )
        {
            if ( !isLocalPlayer )
            {
                return;
            }
            Animation ( );
            Moviment.UpdateMoviment ( );
            InteractObjects.UpdateInteract ( );
            transform.rotation = Quaternion.Euler ( 0 , GameObject.FindObjectOfType<CinemachinePovExtension> ( ).GetStartrotation ( ).x , 0 );

        }
      
        public void Animation ( )
        {
            //animatorcontroller
            AnimatorController.SetFloat ( "Horizontal" , InputManager.GetMoviment ( ).x );
            AnimatorController.SetFloat ( "Vertical" , InputManager.GetMoviment ( ).y );
            AnimatorController.SetBool ( "IsJump" , !Moviment.isGrounded ( ) );
            AnimatorController.SetBool ( "IsRun" , Moviment.CheckMovement ( ) && InputManager.GetRun ( ) );
            AnimatorController.SetBool ( "IsCrouch" , InputManager.GetCrouch ( ) );

            //weaponanimator

            AnimatorWeaponHolderController.SetBool ( "Walk" , Moviment.CheckMovement ( ) && Moviment.isGrounded ( ) );
            AnimatorWeaponHolderController.SetBool ( "Run" , Moviment.CheckMovement ( ) && InputManager.GetRun ( ) && Moviment.isGrounded ( ) );
            AnimatorWeaponHolderController.SetBool ( "Crouch" , Moviment.CheckMovement ( ) && InputManager.GetCrouch ( ) && Moviment.isGrounded ( ) );

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

      
        // fast ITEMS
        [Command]
        public void CmdGetFastItems ( NetworkConnectionToClient sender = null )
        {

            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).FastItems.GetFastItemsTemp ( ) );

        }
        [Command]
        public void CmdMoveSlotFastItems ( int idselecionado , int identer , NetworkConnectionToClient sender = null )
        {
            FastItems.MoveItem ( idselecionado,identer);
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).FastItems.GetFastItemsTemp ( ) );
        }
        [Command]
        public void CmdAddSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
            ScriptableItem item = ScriptableManager.GetScriptable(slot.slot.guidid) ;
            SSlotInventory slotnovo;
            if ( item )
            {
                 slotnovo = new SSlotInventory(item.sitem,slot.slot.Quantity);
            }
            else
            {
                 slotnovo = new SSlotInventory(null,0);
            }
           

            FastItems.SetFastSlots ( slot.id , slotnovo );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).FastItems.GetFastItemsTemp ( ) );

        }
        [Command]
        public void CmdRemoveSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null )
        {
        //    FastItems.RemoveSlot ( slot.slot );
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            TargetGetFastItems ( opponentIdentity.connectionToClient , sender.identity.GetComponent<FpsPlayer> ( ).FastItems.GetFastItemsTemp ( ) );
        }
        #endregion
        #region TARGERRPC

     
        [TargetRpc]
        public void TargetGetFastItems ( NetworkConnection target , InventoryTemp fastSlots )
        {
          

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

        private InputManager PInputManager;
        public InputManager InputManager
        {
            get
            {
                if ( PInputManager == null )
                {
                    PInputManager = GameObject.Find ( "InputManager" ).GetComponent<InputManager> ( );
                }
                return PInputManager;
            }
        }

    }
}