using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApocalipseZ;
public interface IInventory
{
    public event System.Action OnInventoryAltered;
    SSlotInventory GetSlotInventory ( int index);
    bool AddItem( SSlotInventory item );
    public bool CheckFreeSpace( ref int posicao );
    public Item CheckForItem( SItem item );
    public bool CheckIfItemExist( SItem item );
    public void RemoveItem( SItem item , bool destroy);
    public void UseItem( SSlotInventory item , bool closeInventory);
    public int GetMaxSlots ( );
    public void SetMaxSlots ( int maxslot);
}
