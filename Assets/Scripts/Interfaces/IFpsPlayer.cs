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

}