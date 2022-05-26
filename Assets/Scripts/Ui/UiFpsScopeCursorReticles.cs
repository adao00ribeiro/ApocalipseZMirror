using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ApocalipseZ
{
    public class UiFpsScopeCursorReticles : MonoBehaviour
    {
        [SerializeField]private GameObject useCursor;
        [SerializeField]private Text useText;
        public bool UseNonPhysicalReticle = true;
        [Tooltip("Scope image used for riffle aiming state")]
        public GameObject scopeImage;
        [Tooltip("Crosshair image")]
        public GameObject reticleDynamic;
        public GameObject reticleStatic;
        // Start is called before the first frame update
        void Start ( )
        {
            useCursor = transform.Find ( "UseCursor" ).gameObject;
            useText = useCursor.GetComponentInChildren<Text> ( );
            useCursor.SetActive ( false );

            scopeImage = transform.Find ( "Scope" ).gameObject;
            reticleDynamic = transform.Find ( "Reticles/DynamicReticle" ).gameObject;
            reticleStatic = transform.Find ( "Reticles/StaticReticle" ).gameObject;
            scopeImage.SetActive ( false );
            if ( UseNonPhysicalReticle )
            {
                reticleStatic.SetActive ( true );
                reticleDynamic.SetActive ( false );
            }
            else
            {
                reticleStatic.SetActive ( false );
                reticleDynamic.SetActive ( true );
            }
        }

        // Update is called once per frame
        void Update ( )
        {

        }
        public void EnableCursor ( )
        {
            useCursor.SetActive ( true );
        }
        public void DisableCursor ( )
        {
            useCursor.SetActive ( false );
        }
        internal void SetUseText ( string text )
        {
            useText.text = text;
        }
    }
}