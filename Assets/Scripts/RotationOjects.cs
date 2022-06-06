using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationOjects : MonoBehaviour
{
    public string nameObject;
    public float speed;
    public bool IsOpen;
    [SerializeField]private Vector3 QuaternionAberto;
    [SerializeField]private Vector3 QuaternionFechado;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( IsOpen )
        {
            transform.localRotation = Quaternion.RotateTowards ( transform.localRotation ,  Quaternion.Euler( QuaternionAberto ) , speed * Time.deltaTime );
        }
        else
        {
            transform.localRotation = Quaternion.RotateTowards ( transform.localRotation , Quaternion.Euler ( QuaternionFechado ) , speed * Time.deltaTime );
        }
    }
}
