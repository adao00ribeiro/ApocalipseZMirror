using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ApocalipseZ
{
   
    public class InteractObjects : MonoBehaviour, IInteractObjects
    {
        [Tooltip("The distance within which you can pick up item")]
        public float distance = 1.5f;

       [SerializeField]private Item interact;

        UiFpsScopeCursorReticles UiFpsScopeCursorReticles;
        private Transform RaycastCollider;
        public LayerMask layer;
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
        // Start is called before the first frame update
        void Start ( )
        {
            UiFpsScopeCursorReticles = GameObject.FindObjectOfType<UiFpsScopeCursorReticles> ( );
            RaycastCollider = GameObject.CreatePrimitive (PrimitiveType.Sphere).transform;
            RaycastCollider.name = "COLLIDERINTERACT";
            RaycastCollider.gameObject.tag = "noCollider";
            RaycastCollider.gameObject.layer = 8;
            RaycastCollider.transform.SetParent ( this.transform);
            RaycastCollider.GetComponent<SphereCollider> ( ).isTrigger = true;
            RaycastCollider.GetComponent<SphereCollider>().radius = 0.1f;
            Destroy ( RaycastCollider.GetComponent<MeshFilter>());
            Destroy ( RaycastCollider.GetComponent<MeshRenderer> ( ) );
        }

        // Update is called once per frame
       public void UpdateInteract ( )
        {
          
            RaycastHit hit;

          
            if ( Physics.Raycast ( transform.position , transform.forward , out hit , distance, layer ) )
            {
                 interact =  hit.collider.gameObject.GetComponent<Item> ( );
                
                if ( interact !=null )
                {
                    RaycastCollider.position = hit.collider.gameObject.transform.position;
                    UiFpsScopeCursorReticles.EnableCursor ( );
                    UiFpsScopeCursorReticles.SetUseText ( interact.GetTitle ( ) );
                 
                   
                    if (InputManager.GetUse())
                    {
                        interact.CmdSetDoorState ( );
                        interact = null;
                        UiFpsScopeCursorReticles.SetUseText ( "" );
                    }
                    
                }
             
            }
            else
            {
                RaycastCollider.position = transform.position + transform.forward * distance;
                UiFpsScopeCursorReticles.DisableCursor( );
                UiFpsScopeCursorReticles.SetUseText ("");
            }
        }

    }
}