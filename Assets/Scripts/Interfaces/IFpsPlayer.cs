using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public interface IFpsPlayer 
{
    IInventory GetInventory ( );
    IWeaponManager GetWeaponManager ( );
    IFastItems GetFastItems ( );

    //fastitem
    void CmdMoveSlotFastItems ( int idselecionado , int identer , NetworkConnectionToClient sender = null );

    void CmdAddSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );

    void CmdRemoveSlotFastItems ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );
}