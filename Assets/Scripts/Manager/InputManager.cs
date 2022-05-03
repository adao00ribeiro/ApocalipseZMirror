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
	public bool GetRun()
	{
		return playerControls.Player.Run.triggered;
	}
	public bool GetCrouch()
	{
		return playerControls.Player.Crouch.triggered;
	}
	public bool GetReload()
	{
		return playerControls.Player.Reload.triggered;
	}
	public bool GetFireModeSingle()
	{
		return playerControls.Player.FiremodeSingle.triggered;
	}
	public bool GetFireModeAuto()
	{
		return playerControls.Player.FiremodeAuto.triggered;
	}
	public bool GetUse()
	{
		return playerControls.Player.Use.triggered;
	}
	public bool GetAim()
	{
		return playerControls.Player.Aim.triggered;
	}
	public bool GetDropWeapon()
	{
		return playerControls.Player.DropWeapon.triggered;
	}
    internal Vector2 GetMouseDelta ( )
    {
       return playerControls.Player.Look.ReadValue<Vector2> ( );
    }
}
