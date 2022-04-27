using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameObject gameobject;
    private PlayerControls playerControls;
    private static InputManager _instance;
    public static InputManager instance
    {
        get
        {
        
            return _instance;
        }
    }
    private void Awake ( )
    {
        if (_instance != null && _instance != this)
        {
            Destroy ( this.gameObject);
        }
        else
        {
            _instance = this;
        }
        playerControls = new PlayerControls ( );
    }
    private void OnEnable ( )
    {
        playerControls.Enable ( );
    }
    private void OnDisable ( )
    {
        playerControls.Disable ( );
    }

    public Vector2 GetMoviment ( )
    {
        return playerControls.Player.Move.ReadValue<Vector2> ( );
    }
    public bool GetIsJump ( )
    {
        return playerControls.Player.Jump.triggered;
    }

    internal Vector2 GetMouseDelta ( )
    {
       return playerControls.Player.Look.ReadValue<Vector2> ( );
    }
}
