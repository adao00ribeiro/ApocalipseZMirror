using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    void AddItem(Item item);
    public bool CheckFreeSpace();
    public Item CheckForItem(Item item);
    public bool CheckIfItemExist(string name);
    public void RemoveItem(string name, bool destroy);
    public void RemoveItem(Item item, bool destroy);
    public void UseItem(Item item, bool closeInventory);
}
