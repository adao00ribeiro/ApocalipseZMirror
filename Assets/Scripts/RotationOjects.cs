using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace ApocalipseZ
{
    [RequireComponent ( typeof ( NetworkTransform ) )]
    public class RotationOjects : NetworkBehaviour, IInteract
    {
        public string nameObject;
        public float speed;
        [SyncVar(hook = nameof(IsOpenChanged))]
        public bool IsOpen;

        public AudioClip OpenClip;
        public AudioClip CloseClip;
        void IsOpenChanged ( bool _ , bool newIsOpen )
        {
            if ( newIsOpen )
            {
                Sound.Instance.PlayOneShot ( transform.position , OpenClip );
            }
            else
            {
                Sound.Instance.PlayOneShot ( transform.position , CloseClip );
            }
           
        }

        [SerializeField]private Vector3 QuaternionAberto;
        [SerializeField]private Vector3 QuaternionFechado;


        private void Start ( )
        {
            GetComponent<NetworkTransform> ( ).syncPosition = false;
        }
        [Server]
        // Update is called once per frame
        void FixedUpdate ( )
        {
            if ( IsOpen )
            {
                transform.localRotation = Quaternion.RotateTowards ( transform.localRotation , Quaternion.Euler ( QuaternionAberto ) , speed * Time.deltaTime );
            }
            else
            {
                transform.localRotation = Quaternion.RotateTowards ( transform.localRotation , Quaternion.Euler ( QuaternionFechado ) , speed * Time.deltaTime );
            }
        }
       

        [Command ( requiresAuthority = false )]
        public void CmdInteract ( NetworkConnectionToClient sender = null )
        {
            OnInteract ( sender.identity.GetComponent<FpsPlayer> ( ) );
        }

        public void OnInteract ( IFpsPlayer player )
        {
            IsOpen = !IsOpen;
        }

        public void StartFocus ( )
        {
            throw new System.NotImplementedException ( );
        }

        public void EndFocus ( )
        {
            throw new System.NotImplementedException ( );
        }

        public string GetTitle ( )
        {
            return nameObject;
        }
    }
}