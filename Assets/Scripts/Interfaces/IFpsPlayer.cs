using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFpsPlayer 
{
    bool lockCursor { get; set; }

    IInventory GetInventory ( );
  
}