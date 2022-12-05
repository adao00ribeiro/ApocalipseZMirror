﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using FishNet.Connection;
using TMPro;
using FishNet.Object.Prediction;
using FishNet;

namespace ApocalipseZ
{
    //adicionar e conter components
    public struct MoveData
    {
        public bool Jump;
        public float Horizontal;
        public float Forward;
        public bool IsRun;
        public bool IsCrouch;
        public float RotationX;
        public Vector2 MouseDelta;

    }
    public struct ReconcileData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float VerticalVelocity;
        public bool Grounded;
    }
    [RequireComponent(typeof(Moviment))]
    [RequireComponent(typeof(WeaponManager))]
    public class FpsPlayer : NetworkBehaviour, IFpsPlayer
    {
        Moviment Moviment;
        WeaponManager WeaponManager;
        FastItemsManager FastItemsManager;
        public Inventory Inventory;
        IInteractObjects InteractObjects;
        PlayerStats PlayerStats;
        public FirstPersonCamera FirstPersonCamera;
        //--------------------------------------------
        public bool isClimbing = true;
        private Vector3 previousPos = new Vector3();

        [SerializeField] private Animator AnimatorController;
        [SerializeField] private Animator AnimatorWeaponHolderController;
        [SerializeField] private GameObject[] mesh;
        public Transform pivohead;
        public Light Lanterna;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(PlayerColorChanged))]
        public Color32 playerColor = Color.white;

        //MoveData for client simulation

        // Start is called before the first frame update
        private void Awake()
        {

            Inventory = GetComponent<Inventory>();
            Moviment = GetComponent<Moviment>();
            WeaponManager = GetComponent<WeaponManager>();
            FastItemsManager = GetComponent<FastItemsManager>();
            InteractObjects = transform.Find("Camera & Recoil").GetComponent<InteractObjects>();
            AnimatorWeaponHolderController = transform.Find("Camera & Recoil/Weapon holder").GetComponent<Animator>();
            PlayerStats = GetComponent<PlayerStats>();
            FirstPersonCamera = transform.Find("Camera & Recoil").GetComponent<FirstPersonCamera>();
            WeaponManager.SetFpsPlayer(this);
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            base.TimeManager.OnTick += TimeManager_OnTick;
            base.TimeManager.OnUpdate += TimeManager_OnUpdate;

        }

        private void TimeManager_OnUpdate()
        {
            if (base.IsOwner)
            {
                InteractObjects.UpdateInteract();

                if (InputManager.GetLanterna())
                {
                    Lanterna.enabled = !Lanterna.enabled;
                }
                Animation();
            }

        }

        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            if (base.TimeManager != null)
            {
                base.TimeManager.OnTick -= TimeManager_OnTick;

            }
        }
        private void LateUpdate()
        {
            if (base.IsOwner)
            {
                FirstPersonCamera.UpdateCamera();
            }

        }
        private void TimeManager_OnTick()
        {

            if (base.IsOwner)
            {
                Reconcile(default, false);
                BuildActions(out MoveData md);
                Move(md, false);
            }
            if (base.IsServer)
            {
                Move(default, true);
                ReconcileData rd = new ReconcileData()
                {
                    Position = transform.position,
                    Rotation = transform.rotation,
                    Grounded = Moviment.isGrounded()
                };
                Reconcile(rd, true);
            }
        }



        private void BuildActions(out MoveData moveData)
        {
            moveData = default;
            moveData.Jump = InputManager.GetIsJump();
            moveData.Forward = InputManager.GetMoviment().y;
            moveData.Horizontal = InputManager.GetMoviment().x;
            moveData.IsRun = InputManager.GetRun();
            moveData.IsCrouch = InputManager.GetCrouch();
            moveData.MouseDelta.x = InputManager.GetMouseDelta().x;
            moveData.MouseDelta.y = InputManager.GetMouseDelta().y;
            moveData.RotationX = FirstPersonCamera.GetRotationX();

        }
        [Replicate]
        private void Move(MoveData moveData, bool asServer, bool replaying = false)
        {

            Moviment.GravityJumpUpdate(moveData, (float)base.TimeManager.TickDelta);
            Moviment.MoveTick(moveData, (float)base.TimeManager.TickDelta);

        }
        [Reconcile]
        private void Reconcile(ReconcileData recData, bool asServer)
        {
            //Reset the client to the received position. It's okay to do this
            //even if there is no de-synchronization.
            transform.position = recData.Position;
            transform.rotation = recData.Rotation;
            Moviment.PlayerVelocity.y = recData.VerticalVelocity;
            Moviment.SetIsGround(recData.Grounded);

        }
        public override void OnStartServer()
        {
            base.OnStartServer();

            FirstPersonCamera.GetComponent<Camera>().enabled = false;
            FirstPersonCamera.RemoveAudioListener();

        }
        public override void OnStartClient()
        {
            base.OnStartClient();

            if (base.IsOwner)
            {
                FirstPersonCamera.tag = "MainCamera";
                FirstPersonCamera.GetComponent<Camera>().enabled = true;
                FirstPersonCamera.ActiveCursor(false);
                for (int i = 0; i < mesh.Length; i++)
                {
                    mesh[i].layer = 7;
                }



                Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                CanvasFpsPlayer CanvasFpsPlayer = GameObject.FindObjectOfType<CanvasFpsPlayer>();
                CanvasFpsPlayer.SetFirtPersonCamera(FirstPersonCamera);
                CanvasFpsPlayer.SetPlayerStats(PlayerStats);
                CmdSetupPlayer("player", color);
            }
        }
        void PlayerColorChanged(Color32 _, Color32 newPlayerColor, bool asServer)
        {

            for (int i = 0; i < mesh.Length; i++)
            {
                Material[] mats = mesh[i].GetComponent<SkinnedMeshRenderer>().materials;

                for (int j = 0; j < mats.Length; j++)
                {
                    mats[j].color = newPlayerColor;
                }
            }

            //  GetComponentInChildren<MeshRenderer>().material.color = newPlayerColor;
        }

        [Server]
        public void DroppAllItems()
        {


        }
        [ObserversRpc]
        public void TargetRespaw()
        {
            transform.position = PlayerSpawPoints.Instance.GetPointSpaw();
            AnimatorController.Play("Walk");
            FirstPersonCamera.CameraAlive();
            Moviment.EnableCharacterController();
        }

        [ServerRpc]
        public void CmdDropAllItems(NetworkConnection sender = null)
        {


            // IContainer containerInventory = fpstemp.GetInventory();
            // IContainer containerFastItems = fpstemp.GetFastItems();
            //
            // containerWeapon.TargetGetContainer(opponentIdentity.connectionToClient, TypeContainer.WEAPONS, containerWeapon.GetContainerTemp());

        }


        [ServerRpc]
        public void CmdSetupPlayer(string _name, Color _col)
        {
            playerColor = _col;
        }
        [ObserversRpc]
        internal void RpcSpawBullet(SpawBulletTransform spawbulettransform)
        {
            //Instantiate(ScriptableManager.Instance.GetBullet(spawbulettransform.NameBullet), spawbulettransform.Position, spawbulettransform.Rotation);
            // NetworkServer.Spawn ( Instantiate ( ScriptableManager.bullet , spawbulettransform.Position , spawbulettransform.Rotation ));
            //print ("posicao:" +  spawbulettransform.Position + "rotacao" + spawbulettransform.Rotation);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            //  Destroy(CanvasFpsPlayer.gameObject);
        }
        // Update is called once per frame


        public void Animation()
        {

            //animatorcontroller
            AnimatorController.SetFloat("Horizontal", InputManager.GetMoviment().x);
            AnimatorController.SetFloat("Vertical", InputManager.GetMoviment().y);
            AnimatorController.SetBool("IsJump", !Moviment.isGrounded());
            AnimatorController.SetBool("IsRun", Moviment.CheckMovement() && InputManager.GetRun());
            AnimatorController.SetBool("IsCrouch", InputManager.GetCrouch());

            if (!PlayerStats.IsDead())
            {
                AnimatorController.SetFloat("SelectDeath", InputManager.GetCrouch() ? 0 : Random.Range(1, 5));
            }

            AnimatorWeaponHolderController.SetBool("Walk", Moviment.CheckMovement() && Moviment.isGrounded() && !PlayerStats.IsDead());
            AnimatorWeaponHolderController.SetBool("Run", Moviment.CheckMovement() && InputManager.GetRun() && Moviment.isGrounded() && !PlayerStats.IsDead());
            AnimatorWeaponHolderController.SetBool("Crouch", Moviment.CheckMovement() && InputManager.GetCrouch() && Moviment.isGrounded() && !PlayerStats.IsDead());

        }

        public float GetVelocityMagnitude()
        {
            var velocity = ((transform.position - previousPos).magnitude) / Time.deltaTime;
            previousPos = transform.position;
            return velocity;
        }
        public Moviment GetMoviment()
        {
            return Moviment;
        }

        public WeaponManager GetWeaponManager()
        {
            return WeaponManager;
        }
        public PlayerStats GetPlayerStats()
        {
            return PlayerStats;
        }

        public FirstPersonCamera GetFirstPersonCamera()
        {
            return FirstPersonCamera;
        }
        private InputManager PInputManager;
        public InputManager InputManager
        {
            get
            {
                if (PInputManager == null)
                {
                    PInputManager = GameController.Instance.InputManager;
                }
                return PInputManager;
            }
        }


        #region command
        [ServerRpc]
        public void CmdSpawBullet(SpawBulletTransform spawbulettransform, NetworkConnection sender = null)
        {

        }

        public Inventory GetInventory()
        {
            return Inventory;
        }

        internal FastItemsManager GetFastItemsManager()
        {
            return FastItemsManager;
        }
        #endregion
    }
}