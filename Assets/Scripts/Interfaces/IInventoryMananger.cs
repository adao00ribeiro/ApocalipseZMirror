using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryMananger
{
    void InventoryOpen();

    void InventoryClose();

    void SetFpsPlayer(IFpsPlayer player);
}