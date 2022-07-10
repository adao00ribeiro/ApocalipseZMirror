using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace ApocalipseZ
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grenade : NetworkBehaviour
    {
        public float explosionTimer;
        public float explosionForce;
        public float damageRadius;
        public float damage;

        public GameObject explosionEffects;

        Collider[] colliders;
        GameObject effects_temp;
    
        void OnEnable ( )
        {
            effects_temp = Instantiate ( explosionEffects );
            effects_temp.SetActive ( false );
            StartCoroutine ( Timer ( explosionTimer ) );
        }

        IEnumerator Timer ( float explosionTimer )
        {
            yield return new WaitForSeconds ( explosionTimer );
            print ( "Coroutine ended" );
            Explosion ( );
        }

        void Explosion ( )
        {
            effects_temp.transform.position = transform.position;
            effects_temp.transform.rotation = transform.rotation;

            effects_temp.SetActive ( true );

            Destroy ( gameObject );
        }
    }
}
