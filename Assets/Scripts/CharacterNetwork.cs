using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

namespace ApocalipseZ
{


	[RequireComponent ( typeof ( CharacterController ) )]
	[RequireComponent ( typeof ( NetworkTransform ) )]
	public class CharacterNetwork : NetworkBehaviour, ICharacterNetwork
	{
		[Header("Moviment end Jump")]
		private float Speed = 5f;
		public float Walk = 3f;
		public float Run = 5f;
		public float crouchSpeed = 0.4f;
		public float jumpSpeed = 3.5f;
		public float CrouchHeight = 0.5f;

		private bool Crouch = false;
		public bool isClimbing = false;

		[Header("Controller")]
		private CharacterController controller;
		[SerializeField]private Animator AnimatorController;
		[SerializeField]private Animator AnimatorWeaponHolderController;

		private Vector3 moveDirection = Vector3.zero;
		private float directionY;
		public Vector2 sensitivity = new Vector2(0.5f, 0.5f);
		private Vector3 previousPos = new Vector3();
		[SerializeField] private GameObject[] mesh;

		private Transform CameraTransform;
		public Transform pivohead;


		//inputs

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
		internal bool lockCursor;

		private void Awake ( )
		{

			controller = GetComponent<CharacterController> ( );
			AnimatorController = transform.Find ( "Ch35_nonPBR" ).GetComponent<Animator> ( );
			AnimatorWeaponHolderController = transform.Find ( "Camera & Recoil/WeaponCamera/Weapon holder" ).GetComponent<Animator> ( );
			CameraTransform = Camera.main.transform;
		}
		// Start is called before the first frame update
		void Start ( )
		{
			if ( isLocalPlayer )
			{
				for ( int i = 0 ; i < mesh.Length ; i++ )
				{
					mesh[i].layer = 7;
				}
			}
		}

		public override void OnStartLocalPlayer ( )
		{

			GameObject.FindObjectOfType<CinemachineVirtualCamera> ( ).Follow = pivohead;

		}
		public float GetVelocityMagnitude ( )
		{
			var velocity = ((transform.position - previousPos).magnitude) / Time.deltaTime;
			previousPos = transform.position;
			return velocity;
		}
		// Update is called once per frame
		void Update ( )
		{

			if ( !isLocalPlayer )
			{
				return;
			}

			Animation ( );


			moveDirection = new Vector3 ( InputManager.GetMoviment ( ).x , 0 , InputManager.GetMoviment ( ).y );
			moveDirection = CameraTransform.forward * moveDirection.z + CameraTransform.right * moveDirection.x;

			transform.rotation = Quaternion.Euler ( 0 , GameObject.FindObjectOfType<CinemachinePovExtension> ( ).GetStartrotation ( ).x , 0 );


			if ( InputManager.GetIsJump ( ) && controller.isGrounded )
			{
				directionY = jumpSpeed;
			}
			Speed = Walk;
			Speed = InputManager.GetRun ( ) ? Run : Speed;
			Speed = InputManager.GetCrouch ( ) ? crouchSpeed : Speed;
			controller.height = InputManager.GetCrouch ( ) ? CrouchHeight : 1.8f;
			directionY += Physics.gravity.y * Time.deltaTime;
			moveDirection.y = directionY;
			controller.Move ( moveDirection * Speed * Time.deltaTime );
		}
		bool CheckMovement ( )
		{
			if ( InputManager.GetMoviment ( ).x > 0 || InputManager.GetMoviment ( ).x < 0 || InputManager.GetMoviment ( ).y > 0 || InputManager.GetMoviment ( ).y < 0 )
			{
				return true;
			}
			return false;
		}
		#region Animation

		public void Animation ( )
		{
			//animatorcontroller
			AnimatorController.SetFloat ( "Horizontal" , InputManager.GetMoviment ( ).x );
			AnimatorController.SetFloat ( "Vertical" , InputManager.GetMoviment ( ).y );
			AnimatorController.SetBool ( "IsJump" , !controller.isGrounded );


			//weaponanimator

			AnimatorWeaponHolderController.SetBool ( "Walk" , CheckMovement ( ) && controller.isGrounded );
			AnimatorWeaponHolderController.SetBool ( "Run" , CheckMovement ( ) && InputManager.GetRun ( ) && controller.isGrounded );
			AnimatorWeaponHolderController.SetBool ( "Crouch" , CheckMovement ( ) && InputManager.GetCrouch ( ) && controller.isGrounded );



		}

		#endregion
	}
}