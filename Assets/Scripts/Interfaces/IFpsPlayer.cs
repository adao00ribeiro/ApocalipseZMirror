using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public interface IFpsPlayer 
{
    NetworkConnectionToClient GetConnection ( );
    IContainer GetInventory ( );
    IContainer GetWeaponsSlots ( );
    IContainer GetFastItems ( );


    [Command]
    void CmdSpawBullet ( SpawBulletTransform spawBulletTransform , NetworkConnectionToClient networkConnectionToClient );
}