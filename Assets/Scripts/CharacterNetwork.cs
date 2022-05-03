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
    private Animator AnimatorController;
    public float Speed = 5f;
    public float jumpSpeed = 3.5f;
    private Vector3 moveDirection = Vector3.zero;
    private float directionY;

    [SerializeField] private GameObject[] mesh; 
        
    private Transform CameraTransform;
    public Transform pivohead;
    //inputs
    
    private InputManager input;
 
    private void Awake ( )
    {
        
        controller = GetComponent<CharacterController> ( );
        AnimatorController = GetComponentInChildren<Animator>();
        input = InputManager.instance;
        CameraTransform = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].layer = 7;
            }
        }
    }

    public override void OnStartLocalPlayer ( )
    {
      
        GameObject.FindObjectOfType<CinemachineVirtualCamera> ( ).Follow = pivohead;

    }
    // Update is called once per frame
    void Update()
    {
     
        if ( !isLocalPlayer ) { return; }
        moveDirection = new Vector3 ( input.GetMoviment().x, 0 , input.GetMoviment ( ).y ) ;
        moveDirection = CameraTransform.forward * moveDirection.z + CameraTransform.right * moveDirection.x;
        AnimatorController.SetFloat("Horizontal", input.GetMoviment().x);
        AnimatorController.SetFloat("Vertical", input.GetMoviment().y);
        AnimatorController.SetBool("IsJump", !controller.isGrounded);
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
