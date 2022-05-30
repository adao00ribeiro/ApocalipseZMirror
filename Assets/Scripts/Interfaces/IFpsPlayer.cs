using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public interface IFpsPlayer 
{
    IContainer GetInventory ( );
    IWeaponManager GetWeaponManager ( );
    IContainer GetFastItems ( );

}