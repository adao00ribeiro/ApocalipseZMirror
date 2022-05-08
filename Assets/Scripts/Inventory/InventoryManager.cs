﻿using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.ImageEffects;


public class InventoryManager : MonoBehaviour,IInventoryMananger
{
    [System.Serializable]
    public class OnInventoryOpen : UnityEvent { }
    [System.Serializable]
    public class OnInventoryClose : UnityEvent { }

    Canvas canvas;
    IFpsPlayer FpsPlayer;
    public Blur blurEffect;

    public static bool showInventory = false;
    public bool isOpen = false;

    public OnInventoryOpen OnOpen;
    public OnInventoryClose OnClose;

    private void Start ( )
    {
        canvas = GetComponent<Canvas> ( );
        InventoryClose ( );
    }

    private void Update ( )
    {
        if ( InputManager.instance.GetInventory ( ) && !PlayerStats.isPlayerDead )
        {
            showInventory = !showInventory;
        }
        if (FpsPlayer == null)
        {
            return;
        }
        if ( showInventory )
        {
            InventoryOpen ( );
        }
        else
        {
            InventoryClose ( );
        }
    }

    public void InventoryOpen ( )
    {
        if ( isOpen )
            return;
        else
        {
            canvas.enabled = true;
            FpsPlayer.lockCursor = false;
            blurEffect.enabled = true;
            OnOpen.Invoke ( );
            Time.timeScale = 0;
            isOpen = true;

        }
    }
    public void InventoryClose ( )
    {
        if ( !isOpen )
            return;
        else
        {
            canvas.enabled = false;
            FpsPlayer.lockCursor = true;
            blurEffect.enabled = false;
            OnClose.Invoke ( );
            Time.timeScale = 1;
            isOpen = false;
        }
    }
    public void SetFpsPlayer(IFpsPlayer player)
    {

        FpsPlayer = player;
        blurEffect = Camera.main.GetComponent<Blur>();
    }


}
