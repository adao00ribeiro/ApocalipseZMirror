using ApocalipseZ;
using System;

public interface IWeaponManager
{
    public event Action OnWeaponAltered;
    bool SetSlot ( int id , SSlotInventory sSlotInventory );
    SSlotInventory GetSlot ( int id );
    void RemoveSlot ( SSlotInventory sSlotInventory );
    void MoveItem ( int id , int v );
}