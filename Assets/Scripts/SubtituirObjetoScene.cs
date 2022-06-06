using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class SubtituirObjetoScene : MonoBehaviour
{
    public GameObject prefab;
     // Start is called before the first frame update
    void Start()
    {
        foreach (Transform item in transform)
        {
            Instantiate ( prefab , item.position , item.rotation , transform ); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
