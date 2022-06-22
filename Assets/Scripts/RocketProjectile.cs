using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace ApocalipseZ
{
    
    public class RocketProjectile : NetworkBehaviour
    {
        public GameObject PrefabEffectExplosion;
      
        [HideInInspector]
        public float initialVelocity = 360;
        [HideInInspector]
        public float airResistance = 0.1f;

        private float time;

        private float livingTime = 5f;

        Vector3 lastPosition;

        private void OnEnable ( )
        {
            GetComponent<Rigidbody> ( ).AddForce ( transform.forward * initialVelocity );

            lastPosition = transform.position;
        }


        private void Update ( )
        {
            time += Time.deltaTime;

            RaycastHit hit;
            if ( Physics.Linecast ( lastPosition , transform.position , out hit ) )
            {
                HitFXManager.Instance.ApplyFX ( hit );
                PlayerStats stat = hit.collider.GetComponent<PlayerStats>();
                if ( stat )
                {
                    stat.CmdTakeDamage (100 );
                }
                Instantiate ( PrefabEffectExplosion , transform.position , Quaternion.identity );
                NetworkBehaviour.Destroy ( gameObject );
            }

            lastPosition = transform.position;

            if ( time > livingTime )
            {
                NetworkBehaviour.Destroy ( gameObject );
            }
        }


        private void OnDisable ( )
        {
            time = 0;
            GetComponent<Rigidbody> ( ).velocity = Vector3.zero;
            transform.position = Vector3.zero;
        }
    }

}