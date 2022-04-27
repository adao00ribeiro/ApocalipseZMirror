using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
[RequireComponent ( typeof ( NetworkTransform ) )]
public class CharacterNetwork : NetworkBehaviour
{
    private CharacterController controller;
    public float Speed = 5f;
    public float jumpSpeed = 3.5f;
    private Vector3 moveDirection = Vector3.zero;
    private float directionY;


    private Transform CameraTransform;
    public Transform pivohead;
    //inputs
    
    private InputManager input;
 
    private void Awake ( )
    {
        
        controller = GetComponent<CharacterController> ( );

        input = InputManager.instance;
        CameraTransform = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnGravityChanged ( string _Old , string _New )
    {
        //called from sync var hook, to update info on screen for all players
        

    }
    public override void OnStartLocalPlayer ( )
    {
        Camera.main.transform.SetParent ( transform );
        Camera.main.transform.localPosition = new Vector3 ( 0 , 2.75f , -1.74f );
        Camera.main.transform.Rotate(new Vector3 (34.38f,0,0 ));
        GameObject.FindObjectOfType<CinemachineVirtualCamera> ( ).Follow = pivohead;

    }
    // Update is called once per frame
    void Update()
    {
     
        if ( !isLocalPlayer ) { return; }

        moveDirection = new Vector3 ( input.GetMoviment().x, 0 , input.GetMoviment ( ).y ) ;
        moveDirection = CameraTransform.forward * moveDirection.z + CameraTransform.right * moveDirection.x;   
        

         transform.rotation = Quaternion.Euler ( 0 , GameObject.FindObjectOfType<CinemachinePovExtension> ( ).GetStartrotation().x , 0 );
        if ( input.GetIsJump() && controller.isGrounded )
        {

            directionY = jumpSpeed;
        }

        directionY += Physics.gravity.y * Time.deltaTime;
        moveDirection.y = directionY;


        controller.Move ( moveDirection * Speed * Time.deltaTime  );
    }
}
