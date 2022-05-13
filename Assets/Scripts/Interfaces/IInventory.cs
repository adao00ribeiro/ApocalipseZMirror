using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApocalipseZ;
public interface IInventory
{
    public event System.Action OnInventoryAltered;
    SSlotInventory GetSlotInventory ( int index);
    bool AddItem( SSlotInventory item );
    public bool CheckFreeSpace(ref int posicao);
    public bool CheckIfItemExist( SSlotInventory item );
    public void RemoveItem( SSlotInventory item , bool destroy);
    public void UseItem( SSlotInventory item , bool closeInventory);
    public int GetMaxSlots ( );
    public void SetMaxSlots ( int maxslot);
    void MoveItem ( int id , int idmove );
}
