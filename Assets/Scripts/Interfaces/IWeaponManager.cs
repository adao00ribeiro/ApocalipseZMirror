using ApocalipseZ;
using System;
using Mirror;
public interface IWeaponManager
{

    public event Action<Weapon> OnActiveWeapon;
 
    void SetFpsPlayer ( FpsPlayer player );
    void DesEquipWeapon ( );
    Weapon GetActiveWeapon ( );

    void TargetDesEquipWeapon ( NetworkConnection target );
}