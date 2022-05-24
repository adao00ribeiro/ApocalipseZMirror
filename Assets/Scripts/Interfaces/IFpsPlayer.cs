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

    void CmdMoveSlotInventory ( int idselecionado , int identer , NetworkConnectionToClient sender = null );

    void CmdAddSlotInventory ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );

    void CmdRemoveSlotInventory ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );

    //fastitem
    void CmdMoveSlotFastItems ( int idselecionado , int identer , NetworkConnectionToClient sender = null );

    void CmdAddSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );

    void CmdRemoveSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );
}