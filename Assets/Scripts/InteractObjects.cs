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

        IFpsPlayer player;
        private GameObject useCursor;
        private Text useText;
        private Transform RaycastCollider;
        public LayerMask layer;
        // Start is called before the first frame update
        void Start ( )
        {
            useCursor = GameObject.Find ( "UseCursor" );
            useText = useCursor.GetComponentInChildren<Text> ( );
            useCursor.SetActive ( false );
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
        void Update ( )
        {
            RaycastHit hit;

          
            if ( Physics.Raycast ( transform.position , transform.forward , out hit , distance, layer ) )
            {
                 interact =  hit.collider.gameObject.GetComponent<Item> ( );
                
                if ( interact !=null )
                {
                    RaycastCollider.position = hit.collider.gameObject.transform.position;
                    useCursor.SetActive ( true );
                    useText.text = interact.GetTitle ( );
                   
                    if (InputManager.instance.GetUse())
                    {
                        interact.OnInteract ( player );
                        interact = null;
                    }
                    
                }
             
            }
            else
            {
                RaycastCollider.position = transform.position + transform.forward * distance;
                useCursor.SetActive ( false );
                useText.text = "";
            }
        }

        public void SetFpsPlayer ( IFpsPlayer _player )
        {
            player = _player;
        }
    }
}