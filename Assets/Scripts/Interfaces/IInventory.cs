using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApocalipseZ;
public interface IInventory
{
    bool AddItem( SSlotInventory item );
    public bool CheckFreeSpace( ref int posicao );
    public Item CheckForItem( SItem item );
    public bool CheckIfItemExist( SItem item );
    public void RemoveItem( SItem item , bool destroy);
    public void UseItem( SItem item , bool closeInventory);
}
