using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    
    public float explosionForce;
    public float damageRadius;
    public float damage;
    Collider[] colliders;
    GameObject effects_temp;
    public AudioSource audioSource;
    private void OnEnable ( )
    {
        audioSource = GetComponent<AudioSource> ( );
        audioSource.Play ( );
        transform.LookAt ( Camera.main.transform.position );
        Explosions ( );
    }
   
    void Explosions ( )
    {

        colliders = Physics.OverlapSphere ( transform.position , damageRadius );

        foreach ( Collider collider in colliders )
        {

            IStats stats =  collider.GetComponent<IStats> ( );

            if ( stats != null )
            {
                stats.CmdTakeDamage ( ( int ) damage );
            }


            if ( collider.GetComponent<Rigidbody> ( ) )
            {
                collider.GetComponent<Rigidbody> ( ).AddExplosionForce ( explosionForce , transform.position , damageRadius );
            }
        }
        Destroy ( gameObject ,1);
    }
}
