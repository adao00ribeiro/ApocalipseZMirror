using ApocalipseZ;
using System;

public interface IWeaponManager
{

    public event Action<Weapon> OnActiveWeapon;
 
    void SetFpsPlayer ( FpsPlayer player );

    Weapon GetActiveWeapon ( );
}