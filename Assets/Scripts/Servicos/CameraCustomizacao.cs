using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum stateCamera
{
    INICIAL,
    CUSTOMIZACAO,
}
public class CameraCustomizacao : MonoBehaviour
{
    private Camera cam;
    private float field;
    private Vector3 direction;
    public stateCamera state;
    private float newField;

    public Vector3 posicaoInicial;
    private void Awake()
    {
        posicaoInicial = transform.position;
        cam = GetComponent<Camera>();
        field = 70;
    }
    
    // Update is called once per frame
    void Update()
    {

        if (state == stateCamera.INICIAL)
        {
            field = Mathf.Lerp(field, 70, Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(field, cam.fieldOfView, Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(7.4f,0,0), Time.deltaTime);
        }
            if (state == stateCamera.CUSTOMIZACAO)
        {
         
            field =  Mathf.Lerp(field, newField, Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(field, cam.fieldOfView, Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);
        }
    }

    public void SetZoom(Vector3 Posicao , float _field)
    {
        newField = _field;
        direction = Posicao - cam.transform.position;
    }
}
