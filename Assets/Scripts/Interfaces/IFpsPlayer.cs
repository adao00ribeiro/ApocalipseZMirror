using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public interface IFpsPlayer 
{
    bool lockCursor { get; set; }

    IInventory GetInventory ( );
    IWeaponManager GetWeaponManager ( );
    IFastItems GetFastItems ( );
    void CmdGetInventory ( NetworkConnectionToClient sender = null );
}