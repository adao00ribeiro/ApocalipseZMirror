using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class ScPlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Camera cam;

    [SerializeField] private float walkSpeed, runSpeed, walkSlowSpeed, crochSpeed, crawlSpeed;
    [SerializeField] private float mouseSensitivity;

    private float jump, xRotation;
    private bool run, stop, walkingSlow, crouch, crawl;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Rotation();
        Moviment();

    }

    void Moviment()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical") ;

        if (moveX != 0 || moveZ != 0) stop = false;
        else stop = true;
       
        if(!walkingSlow && !stop && !run && !crouch && !crawl)
        { 
            run = false;
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            move.Normalize();
            controller.Move(move * walkSpeed * Time.deltaTime);
        }
        if (walkingSlow)
        {
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            move.Normalize();
            controller.Move(move * walkSlowSpeed * Time.deltaTime);
        }
        if(run)
        {
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * runSpeed * Time.deltaTime);
            move.Normalize();
        }
        if(crouch)
        {
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * crochSpeed * Time.deltaTime);
            move.Normalize();
        }
        if (crawl)
        {
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * crawlSpeed * Time.deltaTime);
            move.Normalize();
        }


        //VERIFICA SE O PLAYER ESTÁ APERTANDO PARA ANDAR LENTO
        if (Input.GetKey(KeyCode.CapsLock)){ walkingSlow = true;            
        } else { walkingSlow = false; }
        //VERIFICA SE O PLAYER ESTÁ APERTANDO PARA CORRER
        if (!stop && Input.GetKey(KeyCode.LeftShift)) { run = true; 
        }else { run = false; }
        //VERIFICA SE O PLAYER ESTA APERTANDO PARA AGAIXAR
        if (Input.GetKey(KeyCode.LeftControl)) { crouch = true; 
        }else { crouch = false; }
        //VERIFICA SE O PLAYER ESTA APERTANDO PARA RASTEJAR
        if (Input.GetKeyDown(KeyCode.Z)) { crawl = !crawl; }




    }

    void Rotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100 * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler( xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
