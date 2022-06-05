using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;

namespace ApocalipseZ
{
    //adicionar e conter components
    [RequireComponent ( typeof ( NetworkTransform ) )]
    [RequireComponent ( typeof ( Moviment ) )]
    [RequireComponent ( typeof ( WeaponManager ) )]
    public class FpsPlayer : NetworkBehaviour, IFpsPlayer
    {
        public NetworkConnectionToClient conn;
        public GameObject PrefabCanvasFpsPlayer;
        public event System.Action<FpsPlayer> OnLocalPlayerJoined;
        CanvasFpsPlayer CanvasFpsPlayer;

        IMoviment Moviment;
        IWeaponManager WeaponManager;
        public Container WeaponsSlots;
        public Container FastItems;
        public Container Inventory;
        IInteractObjects InteractObjects;
        PlayerStats PlayerStats;
        //--------------------------------------------
        public Vector2 sensitivity = new Vector2(0.5f, 0.5f);
        public Vector2 smoothing = new Vector2(3, 3);
        public bool isClimbing = true;
        private Vector3 previousPos = new Vector3();

        [SerializeField]private Animator AnimatorController;
        [SerializeField]private Animator AnimatorWeaponHolderController;
        [SerializeField] private GameObject[] mesh;
        public Transform pivohead;

        [SyncVar(hook = nameof(PlayerColorChanged))]
        public Color32 playerColor = Color.white;
        // Start is called before the first frame update
        private void Awake ( )
        {
            conn =this.connectionToClient;

            Container[] cont = GetComponents<Container> ( );
            //
            Moviment = GetComponent<Moviment> ( );
            WeaponManager = GetComponent<WeaponManager> ( );

            for ( int i = 0 ; i < cont.Length ; i++ )
            {
                switch ( cont[i].type )
                {
                    case TypeContainer.INVENTORY:
                        Inventory = cont[i];
                        break;
                    case TypeContainer.FASTITEMS:
                        FastItems = cont[i];
                        break;
                    case TypeContainer.WEAPONS:
                        WeaponsSlots = cont[i];
                        break;
                }
            }
            InteractObjects = transform.Find ( "Camera & Recoil" ).GetComponent<InteractObjects> ( );
            AnimatorController = transform.Find ( "Ch35_nonPBR" ).GetComponent<Animator> ( );
            AnimatorWeaponHolderController = transform.Find ( "Camera & Recoil/WeaponCamera/Weapon holder" ).GetComponent<Animator> ( );
            PlayerStats = GetComponent<PlayerStats> ( );



        }
    
        public override void OnStartLocalPlayer ( )
        {
           
            //
            for ( int i = 0 ; i < mesh.Length ; i++ )
            {
                mesh[i].layer = 7;
            }
            //
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            

            GameObject.FindObjectOfType<CinemachineVirtualCamera> ( ).Follow = pivohead;
            WeaponManager.SetFpsPlayer ( this);
            CanvasFpsPlayer = Instantiate ( PrefabCanvasFpsPlayer ).GetComponent<CanvasFpsPlayer> ( );
            InteractObjects.Init ( );
            OnLocalPlayerJoined += CanvasFpsPlayer.Init; ;
            OnLocalPlayerJoined?.Invoke ( this );
            CmdSetupPlayer ("player" ,color );
        }
        void PlayerColorChanged ( Color32 _ , Color32 newPlayerColor )
        {

            for ( int i = 0 ; i < mesh.Length ; i++ )
            {
                Material[] mats =   mesh[i].GetComponent<SkinnedMeshRenderer>().materials;

                for (int j = 0 ; j < mats.Length ; j++ )
                {
                    mats[j].color = newPlayerColor;
                }
            }
        }
        [Command]
        public void CmdSetupPlayer ( string _name , Color _col )
        {
            playerColor = _col;
        }
        [ClientRpc]
        internal void RpcSpawBullet ( SpawBulletTransform spawbulettransform )
        {
            Instantiate ( ScriptableManager.bullet , spawbulettransform.Position , spawbulettransform.Rotation );
           // NetworkServer.Spawn ( Instantiate ( ScriptableManager.bullet , spawbulettransform.Position , spawbulettransform.Rotation ));
            //print ("posicao:" +  spawbulettransform.Position + "rotacao" + spawbulettransform.Rotation);
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
            if ( PlayerStats.isPlayerDead )
            {
                AnimatorController.Play ( "BlendDeath" );
                AnimatorWeaponHolderController.Play ( "Unhide" );
                return;
            }
            Moviment.UpdateMoviment ( );
            InteractObjects.UpdateInteract ( );
            transform.rotation = Quaternion.Euler ( 0 , GameObject.FindObjectOfType<CinemachinePovExtension> ( ).GetStartrotation ( ).x , 0 );

        }
        public void Respaw ( )
        {
            AnimatorController.Play ( "Walk" );
        }
        public void Animation ( )
        {
            //animatorcontroller
            AnimatorController.SetFloat ( "Horizontal" , InputManager.GetMoviment ( ).x );
            AnimatorController.SetFloat ( "Vertical" , InputManager.GetMoviment ( ).y );
            AnimatorController.SetBool ( "IsJump" , !Moviment.isGrounded ( ) );
            AnimatorController.SetBool ( "IsRun" , Moviment.CheckMovement ( ) && InputManager.GetRun ( ) );
            AnimatorController.SetBool ( "IsCrouch" , InputManager.GetCrouch ( ) );
            if (!PlayerStats.isPlayerDead )
            {
                AnimatorController.SetFloat ( "SelectDeath" , InputManager.GetCrouch ( ) ? 0 : Random.Range ( 1 , 5 ) );
            }
                       
            AnimatorWeaponHolderController.SetBool ( "Walk" , Moviment.CheckMovement ( ) && Moviment.isGrounded ( ) && !PlayerStats.isPlayerDead );
            AnimatorWeaponHolderController.SetBool ( "Run" , Moviment.CheckMovement ( ) && InputManager.GetRun ( ) && Moviment.isGrounded ( ) && !PlayerStats.isPlayerDead );
            AnimatorWeaponHolderController.SetBool ( "Crouch" , Moviment.CheckMovement ( ) && InputManager.GetCrouch ( ) && Moviment.isGrounded ( ) && !PlayerStats.isPlayerDead );

        }
     
        public float GetVelocityMagnitude ( )
        {
            var velocity = ((transform.position - previousPos).magnitude) / Time.deltaTime;
            previousPos = transform.position;
            return velocity;
        }

        public IContainer GetInventory ( )
        {
            return Inventory;
        }
        public IContainer GetFastItems ( )
        {
            return FastItems;
        }
        public IContainer GetWeaponsSlots ( )
        {
            return WeaponsSlots;
        }
        public IWeaponManager GetWeaponManager ( )
        {
            return WeaponManager;
        }
        public PlayerStats GetPlayerStats ( )
        {
            return PlayerStats;
        }
        public NetworkConnectionToClient GetConnection ( )
        {
            return conn;
        }

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

        public  IContainer GetContainer ( TypeContainer type  )
        {
            IContainer tempContainer = null;
            switch ( type )
            {
                case TypeContainer.INVENTORY:
                    tempContainer = GetInventory ( );
                    break;
                case TypeContainer.FASTITEMS:
                    tempContainer = GetFastItems ( );
                    break;
                case TypeContainer.WEAPONS:
                    tempContainer = GetWeaponsSlots ( );
                    break;
            }

            return tempContainer;
        }
        #region command
        [Command]
        public  void CmdSpawBullet ( SpawBulletTransform spawbulettransform , NetworkConnectionToClient sender = null )
        {
            NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
            FpsPlayer fpstemp = sender.identity.GetComponent<FpsPlayer> ( );
            //NetworkServer.Spawn ( Instantiate ( ScriptableManager.bullet , spawbulettransform.Position , spawbulettransform.Rotation ) );
            fpstemp.RpcSpawBullet ( spawbulettransform );
        }

       

        #endregion
    }
}