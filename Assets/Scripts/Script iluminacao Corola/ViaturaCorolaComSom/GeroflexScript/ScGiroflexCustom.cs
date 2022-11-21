using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScGiroflexCustom : MonoBehaviour
{
    [SerializeField] private enum typeGiroflex {PoliceCar, Ambulance, fireTruck, None};
    [SerializeField] private typeGiroflex type = typeGiroflex.None;

    [SerializeField] private Light lightOne;
    [SerializeField] private Light lightTwo;

    [SerializeField] private float intensity;


    void Start()
    {
        switch (type)
        {
            case typeGiroflex.PoliceCar:
                StartCoroutine("PoliceCar");
                break;
            case typeGiroflex.Ambulance:
                StartCoroutine("Ambulance");
                break;
            case typeGiroflex.fireTruck:
                StartCoroutine("FireTruck");
                break;
            case typeGiroflex.None:
                break;
            default:
                break;
        }

    }

    //LOOP PoliceCar Giroflex  
    private IEnumerator PoliceCar()
    {
        lightTwo.intensity = 0;
        lightOne.intensity = intensity;
        lightOne.intensity -= intensity * Time.deltaTime * 10;
        yield return new WaitForSeconds(0.1f);
        lightOne.intensity = 0;
        lightTwo.intensity = intensity;
        lightTwo.intensity -= intensity * Time.deltaTime * 10;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine("PoliceCar");
            }

    //LOOP Ambulance Giroflex    
    private IEnumerator Ambulance()
    {
        lightTwo.intensity = 0;
        lightOne.intensity -= intensity;
        yield return new WaitForSeconds(0.04f);
        lightOne.intensity = intensity;
        yield return new WaitForSeconds(0.09f);
        lightOne.intensity -= intensity;
        yield return new WaitForSeconds(0.04f);
        lightOne.intensity = intensity;
        yield return new WaitForSeconds(0.09f);
        lightOne.intensity -= intensity;
        yield return new WaitForSeconds(0.04f);
        lightOne.intensity = intensity;
        yield return new WaitForSeconds(0.09f);
        lightOne.intensity -= intensity;
        yield return new WaitForSeconds(0.04f);
        lightTwo.intensity = intensity;
        yield return new WaitForSeconds(0.09f);
        lightTwo.intensity -= intensity;
        yield return new WaitForSeconds(0.04f);
        lightTwo.intensity = intensity;
        yield return new WaitForSeconds(0.09f);
        lightTwo.intensity -= intensity;
        yield return new WaitForSeconds(0.04f);
        lightTwo.intensity = intensity;
        yield return new WaitForSeconds(0.09f);
        StartCoroutine("Ambulance");
    }

    //LOOP Fire Truck Giroflex
    private IEnumerator FireTruck()
    {
        lightTwo.intensity = 0;
        lightOne.intensity = intensity;
        lightOne.intensity -= intensity * Time.deltaTime * 10;
        yield return new WaitForSeconds(0.4f);
        lightOne.intensity = 0;
        lightTwo.intensity = intensity;
        lightTwo.intensity -= intensity * Time.deltaTime * 10;
        yield return new WaitForSeconds(0.4f);
        StartCoroutine("FireTruck");
    }
}
