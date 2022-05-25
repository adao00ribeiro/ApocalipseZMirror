using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApocalipseZ;
using System;

public interface IFastItems {

    public event Action OnFastItemsAltered;
    void RemoveSlot ( SSlotInventory slot );
    void SetFastSlots ( int id , SSlotInventory slot );
    void SlotChange ( int switchSlotIndex );
    SSlotInventory GetSlotFastItems ( int id );
    void SetInventory ( IInventory inventory);
    void MoveItem ( int id , int v );
    InventoryTemp GetFastItemsTemp ( );
    void Clear ( );
}
