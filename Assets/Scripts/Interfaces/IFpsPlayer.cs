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

    IMoviment GetMoviment ( );
    PlayerStats GetPlayerStats ( );

    void Respaw ( );
    [Command]
    void CmdSpawBullet ( SpawBulletTransform spawBulletTransform , NetworkConnectionToClient networkConnectionToClient );
    FirstPersonCamera GetFirstPersonCamera ( );
    IWeaponManager GetWeaponManager ( );
}