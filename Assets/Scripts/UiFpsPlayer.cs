using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiFpsPlayer : MonoBehaviour
{
    [SerializeField]private GameObject useCursor;
    [SerializeField]private Text useText;
    // Start is called before the first frame update
    void Start()
    {
        useCursor = GameObject.Find ( "UseCursor" );
        useText = useCursor.GetComponentInChildren<Text> ( );
        useCursor.SetActive ( false );
    }

    public void EnableCursor ( )
    {
        useCursor.SetActive ( true);
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
