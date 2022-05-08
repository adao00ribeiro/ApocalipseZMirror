using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

//adicionar e conter components
[RequireComponent ( typeof ( NetworkTransform ) )]
[RequireComponent ( typeof ( Moviment ) )]
[RequireComponent ( typeof ( WeaponManager ) )]
[RequireComponent ( typeof ( InventoryManager ) )]
public class FpsPlayer : NetworkBehaviour,IFpsPlayer
{
 
    IMoviment Moviment;
    IWeaponManager WeaponManager;
    IInventory Inventory;
    IInventoryMananger InventoryManager;
    InputManager inputManager;
    [SerializeField]private Animator AnimatorController;
    [SerializeField]private Animator AnimatorWeaponHolderController;

    public Transform pivohead;
    private bool PlockCursor;

    public bool lockCursor { get => PlockCursor; set => PlockCursor = value; }

    // Start is called before the first frame update
    void Start()
    {
        if ( !isLocalPlayer )
        {
            return;
        }
        inputManager = InputManager.instance;
        Moviment = GetComponent<Moviment> ( );
        WeaponManager = GetComponent<WeaponManager> ( );
        Inventory = transform.Find ( "Inventory").GetComponent<Inventory> ( );
        AnimatorController = transform.Find ( "Ch35_nonPBR" ).GetComponent<Animator> ( );
        AnimatorWeaponHolderController = transform.Find ( "Camera & Recoil/WeaponCamera/Weapon holder" ).GetComponent<Animator> ( );
      
    }
    public override void OnStartLocalPlayer ( )
    {

        GameObject.FindObjectOfType<CinemachineVirtualCamera> ( ).Follow = pivohead;

    }
    // Update is called once per frame
    void Update()
    {
        if ( !isLocalPlayer )
        {
            return;
        }
	    Animation();
        transform.rotation = Quaternion.Euler ( 0 , GameObject.FindObjectOfType<CinemachinePovExtension> ( ).GetStartrotation ( ).x , 0 );

    }

    public void Animation ( )
    {
        //animatorcontroller
        AnimatorController.SetFloat ( "Horizontal" , inputManager.GetMoviment ( ).x );
        AnimatorController.SetFloat ( "Vertical" , inputManager.GetMoviment ( ).y );
        AnimatorController.SetBool ( "IsJump" , !Moviment.isGrounded() );
        AnimatorController.SetBool("IsRun", Moviment.CheckMovement() && inputManager.GetRun());


        //weaponanimator

        AnimatorWeaponHolderController.SetBool ( "Walk" , Moviment.CheckMovement ( ) && Moviment.isGrounded());
        AnimatorWeaponHolderController.SetBool ( "Run" , Moviment.CheckMovement ( ) && inputManager.GetRun ( ) && Moviment.isGrounded() );
        AnimatorWeaponHolderController.SetBool ( "Crouch" , Moviment.CheckMovement( ) && inputManager.GetCrouch ( ) && Moviment.isGrounded() );

    }

}
