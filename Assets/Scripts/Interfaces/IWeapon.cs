using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    DataArmsWeapon WeaponSetting { get; }
    int CurrentAmmo { get; set; }
    GameObject gameObject { get; }
    string WeaponName { get; }
    bool Fire();
    bool ReloadBegin();

    void InvokeRealodEnd();
    bool Aim(bool v);
    bool SetAim { get; }
}
